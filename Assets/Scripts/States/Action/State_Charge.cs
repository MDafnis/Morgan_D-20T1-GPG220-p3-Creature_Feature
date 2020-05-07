using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class State_Charge : BaseState
{
    private float IdleTimeRemaining = 2f;

    public override void State_Init()
    {
        
    }

    public override void State_Update()
    {
        IdleTimeRemaining -= Time.deltaTime;
        if (IdleTimeRemaining <= 0)
        {

        }
    }

    public override void State_Enter()
    {

    }

    public override void State_Exit()
    {

    }

    public void CanTransition_ToIdle(TransitionResponse response)
    {
        response.CanTransition = IdleTimeRemaining <= 0;
    }
}