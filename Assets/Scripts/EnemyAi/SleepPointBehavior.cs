using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SleepPointBehavior : PatrolPointBehavior
{

    public bool isNight = false;

    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
        

    }

    // Update is called once per frame. Using inherited version from parent
    //void Update()
    //{


    //}


    // Called once per frame from base class Update function.
    protected override void ExecuteAction()
    {
        //Default behavior to just wait until morning.
        switch (status)
        {
            case ActionStatus.NOT_STARTED:
            case ActionStatus.COMPLETE:
            case ActionStatus.ERROR:
                //Do Nothing in these states
                break;
            case ActionStatus.STARTED:
                status = ActionStatus.IN_PROGRESS;
                break;
            case ActionStatus.IN_PROGRESS:
                if (!isNight)
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



}
