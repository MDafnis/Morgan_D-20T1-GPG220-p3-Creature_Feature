using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class State_Charge : BaseState
{
    private float IdleTimeRemaining = 2f;

    public override void State_Init()
    {
        base.State_Init();
    }

    public override void State_Update()
    {
		base.State_Update();
        IdleTimeRemaining -= Time.deltaTime;
    }

    public override void State_Enter()
    {
		base.State_Enter();
    }

    public override void State_Exit()
    {
		base.State_Exit();
        agent.curCharge = 100;
    }

    public void CanTransition_ToIdle(TransitionResponse response)
    {
        response.CanTransition = IdleTimeRemaining <= 0;
    }
}