using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class State_Charge : BaseState
{
    private IEnumerator Charge;
    private bool charging;

    public override void State_Init()
    {
        Charge = WaitforCharge();
        charging = true;
    }

    public override void State_Update()
    {

    }

    public override void State_Enter()
    {
        StartCoroutine(Charge);
    }

    public override void State_Exit()
    {

    }

    private IEnumerator WaitforCharge()
    {
        while (true)
        {
            yield return new WaitForSecondsRealtime(2);
            FindObjectOfType<FSMCharacter>().curCharge += 100;
            charging = false;
            break;
        }
    }

    public void CanTransition_ToIdle(TransitionResponse response)
    {
        response.CanTransition = !charging;
    }
}