using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PatrolPointBehavior : MonoBehaviour
{
    public enum ActionStatus
    {
        NOT_STARTED,
        COMPLETE,
        IN_PROGRESS,
        STARTED,
        ERROR
    }

    public float waitTime = 4.0f;

    private ActionStatus status = ActionStatus.NOT_STARTED;
    private float accumulatedWaitTime = 0.0f;

    // Start is called before the first frame update
    void Start()
    {
        //Disabled by default. Activates when started. For performance so patrol points aren't ticking all the time
        enabled = false;
    }

    // Update is called once per frame
    void Update()
    {

        //Default behavior to just wait around at the patrol point. Doing nothing interesting.
        switch(status)
        {
            case ActionStatus.NOT_STARTED:
            case ActionStatus.COMPLETE:
            case ActionStatus.ERROR:
                //Do Nothing in these states
                break;
            case ActionStatus.STARTED:
                accumulatedWaitTime = 0.0f;
                status = ActionStatus.IN_PROGRESS;
                break;
            case ActionStatus.IN_PROGRESS:
                accumulatedWaitTime += Time.deltaTime;
                if(accumulatedWaitTime > waitTime)
                {
                    status = ActionStatus.COMPLETE;
                }
                break;
            default:
                //Error! status is some weird unexpected number.
                status = ActionStatus.ERROR;
                break;
        }
    }

    // Function called by the patrolling character. Will be called periodically. Kicks off and completes the behavior or will just say it's in progress.
    public virtual ActionStatus DoPatrolReached()
    {
        if(status == ActionStatus.COMPLETE || status == ActionStatus.ERROR)
        {
            ActionStatus returnStatus = status;
            ResetAction(); //Resets so ready to run again later.
            return returnStatus; //Returning so the caller knows the action just ended.
        }

        // Kick off the action.
        if(status == ActionStatus.NOT_STARTED)
        {
            
            status = ActionStatus.STARTED;
            enabled = true;
            return status;
        }

        // Other states changed through the actual update behavior for the object
        return status;
    }

    private void ResetAction()
    {
        enabled = false;
        status = ActionStatus.NOT_STARTED;
    }

}
