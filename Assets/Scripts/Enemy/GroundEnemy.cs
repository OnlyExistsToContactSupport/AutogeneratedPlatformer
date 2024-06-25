using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class GroundEnemy: MonoBehaviour, IEnemyBehaviour
{
    // Patrol
    private NavMeshAgent navMeshAgent;
    public Transform[] waypoints;
    private int waypointIndex;
    private Vector3 currentWaypointPosition;
    private float patrolSpeed;

    // Chase
    private Transform player;
    private bool isPlayerInSightRange;
    private float sightRange;
    private int fovAngle;
    private float chaseSpeed;

    //Attack
    private bool isPlayerInAttackRange;
    private bool hasAttacked;

    // Animações
    private Animator animator;

    private EnemyHealthBar healthBar;


    // Start is called before the first frame update
    void Start()
    {
        this.AddComponent<NavMeshAgent>();
        navMeshAgent = GetComponent<NavMeshAgent>();
        navMeshAgent.baseOffset = 0;
        navMeshAgent.speed = 3.5f;
        navMeshAgent.angularSpeed = 120f;
        navMeshAgent.acceleration = 8f;
        navMeshAgent.stoppingDistance = 0f;
        navMeshAgent.autoBraking = true;
        navMeshAgent.radius = 0.5f;
        navMeshAgent.height = 2.2f;
        navMeshAgent.avoidancePriority = 50;
        navMeshAgent.autoTraverseOffMeshLink = true;
        navMeshAgent.autoRepath = true;


        waypointIndex = 0;
        currentWaypointPosition = waypoints[waypointIndex].position;
        navMeshAgent.SetDestination(currentWaypointPosition);

        isPlayerInSightRange = false;
        isPlayerInAttackRange = false;

        hasAttacked = false;

        // Visão do player
        sightRange = 40;

        // Angulo de visão
        fovAngle = 100;

        patrolSpeed = 3.5f;
        chaseSpeed = 5f;

        animator = GetComponent<Animator>();

        healthBar = GetComponentInChildren<EnemyHealthBar>();
        healthBar.SetMaxHealth(healthBar.groundEnemyMaxHealth);
    }

    // Update is called once per frame
    void Update()
    {
        CanSeePlayer();

        if (!isPlayerInSightRange && !isPlayerInAttackRange)
        {
            Patrol();
        }

        if(isPlayerInSightRange && !isPlayerInAttackRange)
        {
            ChasePlayer();
        }

        if (isPlayerInSightRange && isPlayerInAttackRange)
        {
            AttackPlayer();
        }
    }
    public void SetPlayer(GameObject playerObject)
    {
        player = playerObject.transform;
    }
    private void UpdateDestination()
    {
        currentWaypointPosition = waypoints[waypointIndex].position;
        navMeshAgent.SetDestination(currentWaypointPosition);
    }
    private void UpdateWaypoint()
    {
        ++waypointIndex;
        if(waypointIndex >= waypoints.Length)
        {
            waypointIndex = 0;
        }
    }

    public void Patrol()
    {
        animator.SetBool("isWalking", true);
        animator.SetBool("isRunning", false);

        navMeshAgent.speed = patrolSpeed;
        // Se não estiver a ir em direçção a um waypoint
        if (!waypoints.Any(x => x.Equals(navMeshAgent.destination)))
        {
            navMeshAgent.SetDestination(currentWaypointPosition);
        }
        // Se chegou ao waypoint, mudar destino
        if (navMeshAgent.remainingDistance < 1)
        {
            UpdateWaypoint();
            UpdateDestination();
        }
    }

    public void ChasePlayer()
    {
        Debug.Log(player);
        animator.SetBool("isWalking", false);
        animator.SetBool("isRunning", true);

        navMeshAgent.speed = chaseSpeed;
        navMeshAgent.SetDestination(player.position);
    }

    public void AttackPlayer()
    {
        // Parar de mexer
        animator.SetBool("isWalking", false);
        animator.SetBool("isRunning", false);
        navMeshAgent.SetDestination(transform.position);
    
        transform.LookAt(player);

        if (!hasAttacked)
        {
            animator.Play("Attack");
                        
            hasAttacked = true; // Set hasAttacked to true after dealing damage

            // Voltar a atacar passado algum tempo (2 segundos)
            Invoke(nameof(ResetAttack), 2f);
        }

    }
    private void ResetAttack()
    {
        hasAttacked = false;
    }
    public void TakeDamage(int damage)
    {
        healthBar.TakeDamage(damage);
    }
    // Desenhar visão do inimigo (Debug)
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, sightRange);
    }

    private void CanSeePlayer()
    {
        Collider[] rangeChecks = Physics.OverlapSphere(transform.position, sightRange);
        bool playerFound = false;

        foreach (Collider collider in rangeChecks)
        {
            if (collider.transform.tag.Equals("Player"))
            {
                Vector3 directionToTarget = (collider.transform.position - transform.position).normalized;

                if (Vector3.Angle(transform.forward, directionToTarget) < fovAngle / 2)
                {
                    float distanceToTarget = Vector3.Distance(transform.position, collider.transform.position);

                    if (distanceToTarget < sightRange)
                    {
                        playerFound = true;

                        if (player == null)
                            player = collider.transform;

                        break;
                    }
                }
            }

        }

        isPlayerInSightRange = playerFound;

    }

    // Chamado pela animação
    public void DealDamage()
    {
        // Como é ataque melee, tem de se verificar se o player está em range
        if(isPlayerInAttackRange)
        {
            GameObject.FindGameObjectWithTag("HealthBar").GetComponent<HealthBar>().TakeDamage(EnemyDamage.groundEnemyDamage);
        }

    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag.Equals("Player"))
            isPlayerInAttackRange = true;
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.tag.Equals("Player"))
            isPlayerInAttackRange = false;
    }
}
