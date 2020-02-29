using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyPatrolBehavior : MonoBehaviour
{
    enum AIState
    {
        Patrolling,
        Sleeping,
        Afraid,
        Aggressive
    }

    public float minDistanceToCompletePatrol = 5.0f;
    public float ladderClimbSpeed = 0.5f;
    public float walkSpeed = 3.5f;
    public float runSpeed = 4.2f;

    public bool isNight = false;

    private int activePatrolPointIndex = 0;
    private List<PatrolPointBehavior> patrolPoints = new List<PatrolPointBehavior>();


    private int activeSleepPointIndex = -1; //-1 indicates we're searching for a place to sleep
    private List<SleepPointBehavior> sleepPoints = new List<SleepPointBehavior>();
    private NavMeshAgent navAgent;

    private AIState currState = AIState.Patrolling;

    private bool crossingNavLink = false;
    private bool isRunning = false;

    // Start is called before the first frame update
    void Start()
    {
        navAgent = GetComponent<NavMeshAgent>();
        navAgent.speed = walkSpeed;

        //Expecting the patrol points and the actual enemy pawn to be under a common root
        patrolPoints.AddRange(transform.parent.gameObject.GetComponentsInChildren<PatrolPointBehavior>());

        //Expecting the sleep points and the actual enemy pawn to be under a common root
        sleepPoints.AddRange(transform.parent.gameObject.GetComponentsInChildren<SleepPointBehavior>());
    }

    // Update is called once per frame
    void Update()
    {
        switch(currState)
        {
            case AIState.Patrolling:
                UpdatePatrolling();
                break;
            case AIState.Sleeping:
                UpdateSleeping();
                break;
            case AIState.Afraid:
                break;
            case AIState.Aggressive:
                break;
        }

        UpdateCurrentSpeed();
    }

    private void UpdatePatrolling()
    {
        if (patrolPoints.Count <= 0)
        {
            //There's no points to patrol, so don't do anything.
            return;
        }

        UpdateCurrentPatrolPoint();
    }

    private void UpdateSleeping()
    {
        if (sleepPoints.Count <= 0)
        {
            //There's no points to patrol, so don't do anything.
            return;
        }

        UpdateCurrentSleepPoint();
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
        PatrolPointBehavior.ActionStatus status = HandlePatrolPoint(currentPoint);
        if (status == PatrolPointBehavior.ActionStatus.COMPLETE || status == PatrolPointBehavior.ActionStatus.ERROR)
        {
            // Move on to the next point
            activePatrolPointIndex = (activePatrolPointIndex + 1) % patrolPoints.Count;
        }

        if(isNight)
        {
            currentPoint.InterruptAction();
            currState = AIState.Sleeping;
        }
    }

    private void UpdateCurrentSleepPoint()
    {
        //-1 indicates we're searching for a place to sleep. Choose closest.
        if(activeSleepPointIndex < 0)
        {
            float minDistance = 9999999.0f; //Large number for initial minimum value
            for(int i = 0; i < sleepPoints.Count; ++i)
            {
                float distanceToPoint = GetDistanceToPatrolPoint(sleepPoints[i]);
                if(minDistance > distanceToPoint)
                {
                    minDistance = distanceToPoint;
                    activeSleepPointIndex = i;
                }
            }
        }

        SleepPointBehavior currentPoint = sleepPoints[activeSleepPointIndex];
        currentPoint.isNight = isNight; //Cascade the nighttime flag to the point. Not the best integration, but it works.

        // Reached Patrol Point
        PatrolPointBehavior.ActionStatus status = HandlePatrolPoint(currentPoint);
        if(status == PatrolPointBehavior.ActionStatus.COMPLETE || status == PatrolPointBehavior.ActionStatus.ERROR)
        {
            currState = AIState.Patrolling; //We woke up. Time to patrol.
            activeSleepPointIndex = -1; //Next time we go to sleep, we will need to search for a place to sleep
        }

        if(!isNight)
        {
            currentPoint.InterruptAction();
            activeSleepPointIndex = -1;
            currState = AIState.Patrolling;
        }
    }

    private PatrolPointBehavior.ActionStatus HandlePatrolPoint(PatrolPointBehavior point)
    {
       
        if (IsAtLocation(point.gameObject.transform.position))
        {
            // Reached Patrol Point. Do action
            return point.DoPointReached();
        }

        //Still moving to point.
        navAgent.destination = point.gameObject.transform.position;
        return PatrolPointBehavior.ActionStatus.NOT_STARTED;
    }

    private float GetDistanceToPatrolPoint(PatrolPointBehavior point)
    {
        Vector2 currentLoc = gameObject.transform.position;
        Vector2 pointLoc = point.transform.position;

        return (pointLoc - currentLoc).magnitude;
    }

    private bool IsAtPoint(ref PatrolPointBehavior point)
    {
        return IsAtLocation(point.transform.position);
    }

    private bool IsAtLocation(Vector2 loc)
    {
        Vector2 currentLoc = gameObject.transform.position;

        float distanceFromPoint = (loc - currentLoc).magnitude;

        return minDistanceToCompletePatrol > distanceFromPoint;
    }


}
