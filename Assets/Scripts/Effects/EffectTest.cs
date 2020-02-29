using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectTest : MonoBehaviour
{
    public BurnEffect effect1;
    public BurnEffect effect2;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            StartDie();
        }
        else if (Input.GetKeyDown(KeyCode.S))
        {
            Reset();
        }
    }

    private void Reset()
    {
        effect1.Reset();
        effect2.Reset();
    }

    private void StartDie()
    {
        StartCoroutine(StartDieRoutine());
    }

    private IEnumerator StartDieRoutine()
    {
        effect1.StartDie();
        yield return new WaitForSeconds(1);
        effect2.StartDie();
    }
}
