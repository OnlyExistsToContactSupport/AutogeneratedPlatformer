using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SocialPlatforms;
using static UnityEngine.GraphicsBuffer;

public class EnemyGroundMovement : MonoBehaviour, IEnemyBehaviour
{
    private NavMeshAgent navMesh;
    public Transform[] waypoints;
    private int waypointIndex;
    private Vector3 currentWaypointPosition;


    private Transform player;
    public bool isPlayerInSightRange;
    public bool isPlayerInAttackRange;
    private float sightRange;
    private float attackRange;
    private int fovAngle;

    public GameObject projectile;
    private bool hasAttacked;


    // Start is called before the first frame update
    void Start()
    {
        navMesh = GetComponent<NavMeshAgent>();
        waypointIndex = 0;
        currentWaypointPosition = waypoints[waypointIndex].position;
        navMesh.SetDestination(currentWaypointPosition);

        player = GameObject.FindWithTag("Player").transform;
        isPlayerInSightRange = false;
        isPlayerInAttackRange = false;

        hasAttacked = false;

        sightRange = 30;
        attackRange = 10;
        fovAngle = 100;
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
        navMesh.SetDestination(currentWaypointPosition);
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
        // Se chegou ao waypoint, mudar destino
        if (navMesh.remainingDistance < 1)
        {
            UpdateWaypoint();
            UpdateDestination();
        }
    }

    public void ChasePlayer()
    {
        navMesh.SetDestination(player.position);
    }

    public void AttackPlayer()
    {
        // Parar de mexer
        navMesh.SetDestination(transform.position);
    
        transform.LookAt(player);

        if (!hasAttacked)
        {
            Rigidbody rb = Instantiate(projectile, transform.position, Quaternion.identity).GetComponent<Rigidbody>();
            rb.AddForce(transform.forward * 32f, ForceMode.Impulse);

            hasAttacked = true;

            Destroy(rb.gameObject, 3f);

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
}
