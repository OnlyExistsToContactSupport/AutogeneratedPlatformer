using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyGroundMovement : MonoBehaviour
{
    NavMeshAgent enemy;

    public Transform[] waypoints;
    private int waypointIndex;

    private Vector3 currentWaypointPosition;

    // Start is called before the first frame update
    void Start()
    {
        enemy = GetComponent<NavMeshAgent>();
        waypointIndex = 0;
        currentWaypointPosition = waypoints[waypointIndex].position;

        enemy.SetDestination(currentWaypointPosition);
    }

    // Update is called once per frame
    void Update()
    {
        if(Vector3.Distance(transform.position, currentWaypointPosition) < 1)
        {
            UpdateWaypoint();
            UpdateDestination();
        }
    }

    private void UpdateDestination()
    {
        currentWaypointPosition = waypoints[waypointIndex].position;
        enemy.SetDestination(currentWaypointPosition);
    }
    private void UpdateWaypoint()
    {
        ++waypointIndex;
        if(waypointIndex >= waypoints.Length)
        {
            waypointIndex = 0;
        }
    }
}
