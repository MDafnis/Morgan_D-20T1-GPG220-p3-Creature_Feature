using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class State_Move_HungryTree : BaseState
{
    public override void State_Init()
    {
        base.State_Init();
    }
    public override void State_Update()
    {
        base.State_Update();
    }
    public override void State_Enter()
    {
        base.State_Enter();
        agent.SetDestination(agent.curObjective.transform.position);
    }
    public override void State_Exit()
    {
        base.State_Exit();
    }

    public void CanTransition_ToFeedTree(TransitionResponse response)
    {
        response.CanTransition = agent.ReachedDestination;
    }
}
