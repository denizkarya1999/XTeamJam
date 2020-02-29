using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyPatrolBehavior : MonoBehaviour
{
    public float minDistanceToCompletePatrol = 5.0f;
    public float ladderClimbSpeed = 0.5f;
    public float walkSpeed = 3.5f;
    public float runSpeed = 4.2f;

    private int activePatrolPointIndex = 0;
    private List<PatrolPointBehavior> patrolPoints = new List<PatrolPointBehavior>();
    private NavMeshAgent navAgent;

    private bool crossingNavLink = false;
    private bool isRunning = false;

    // Start is called before the first frame update
    void Start()
    {
        navAgent = GetComponent<NavMeshAgent>();
        navAgent.speed = walkSpeed;

        //Expecting the patrol points and the actual enemy pawn to be under a common root
        patrolPoints.AddRange(transform.parent.gameObject.GetComponentsInChildren<PatrolPointBehavior>());

        

    }

    // Update is called once per frame
    void Update()
    {
        if(patrolPoints.Count <= 0)
        {
            //There's no points to patrol, so don't do anything.
            return;
        }

        UpdateCurrentPatrolPoint();
        UpdateCurrentSpeed();
    }

    private void UpdateCurrentSpeed()
    {
        if (navAgent.isOnOffMeshLink && !crossingNavLink)
        {
            crossingNavLink = true;
            navAgent.speed = ladderClimbSpeed;
        }
        else if (navAgent.isOnNavMesh && crossingNavLink)
        {
            crossingNavLink = false;
            navAgent.velocity = Vector3.zero;
            navAgent.speed = isRunning ? runSpeed : walkSpeed;
        }
        else
        {
            navAgent.speed = isRunning ? runSpeed : walkSpeed;
        }
    }

    private void UpdateCurrentPatrolPoint()
    {
        PatrolPointBehavior currentPoint = patrolPoints[activePatrolPointIndex];
        
        // Reached Patrol Point
        if (IsAtLocation(currentPoint.gameObject.transform.position))
        {
            PatrolPointBehavior.ActionStatus status = currentPoint.DoPatrolReached();
            if(status == PatrolPointBehavior.ActionStatus.COMPLETE || status == PatrolPointBehavior.ActionStatus.ERROR)
            {
                // Move on to the next point
                activePatrolPointIndex = (activePatrolPointIndex + 1) % patrolPoints.Count;
            }
        }
        else
        {
            navAgent.destination = currentPoint.gameObject.transform.position;
        }
    }


    private bool IsAtLocation(Vector2 loc)
    {
        Vector2 currentLoc = gameObject.transform.position;

        float distanceFromPoint = (loc - currentLoc).magnitude;

        return minDistanceToCompletePatrol > distanceFromPoint;
    }


}
