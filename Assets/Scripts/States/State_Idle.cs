using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class State_Idle : BaseState
{
    public float IdleTime_Min = 5f;
    public float IdleTime_Max = 10f;

    protected float IdleTimeRemaining = -1f;

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

        IdleTimeRemaining = Random.Range(IdleTime_Min, IdleTime_Max);
    }

    public override void State_Exit()
    {
        base.State_Exit();
    }

    public void CanTransition_ToPickLocation(TransitionResponse response)
    {
        response.CanTransition = IdleTimeRemaining <= 0f;
    }
}
