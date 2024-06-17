using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
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
    public bool isPlayerInSightRange;
    private float sightRange;
    private int fovAngle;
    private float chaseSpeed;

    //Attack
    private float attackRange;
    public bool isPlayerInAttackRange;
    public GameObject projectile;
    private bool hasAttacked;

    // Animações
    private Animator animator;


    // Start is called before the first frame update
    void Start()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
        waypointIndex = 0;
        currentWaypointPosition = waypoints[waypointIndex].position;
        navMeshAgent.SetDestination(currentWaypointPosition);

        player = GameObject.FindWithTag("Player").transform;
        isPlayerInSightRange = false;
        isPlayerInAttackRange = false;

        hasAttacked = false;

        // Visão do player
        sightRange = 40;
        // Range de ataque
        attackRange = 5;
        // Angulo de visão
        fovAngle = 100;

        patrolSpeed = 3.5f;
        chaseSpeed = 5f;

        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        CanSeePlayer();
        CanAttackPlayer();

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
        animator.SetBool("isWalking", false);
        animator.SetBool("isRunning", true);

        navMeshAgent.speed = chaseSpeed;
        navMeshAgent.SetDestination(player.position);
    }

    public void AttackPlayer()
    {
        // Parar de mexer
        navMeshAgent.SetDestination(transform.position);
    
        transform.LookAt(player);

        if (!hasAttacked)
        {
            animator.Play("Attack");

            // Voltar a atacar passado algum tempo (2 segundos)
            Invoke(nameof(ResetAttack), 2f);
        }

    }
    private void ResetAttack()
    {
        hasAttacked = false;
    }
    public void TakeDamage(float damage)
    {

    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
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
                        break;
                    }
                }
            }

        }

        isPlayerInSightRange = playerFound;
    }
    private void CanAttackPlayer()
    {
        isPlayerInAttackRange = Vector3.Distance(transform.position, player.position) <= attackRange;
    }

    public void DealDamage()
    {
        throw new NotImplementedException();
    }
}
