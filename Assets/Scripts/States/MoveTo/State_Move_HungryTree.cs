using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class State_Move_HungryTree : BaseState
{
    public float LocationReachedTresheld = 0.1f;

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
        GameObject storedLocation = TerrainGenerator.instance.treePlot[Random.Range(0, TerrainGenerator.instance.treePlot.Count)];
        agent.SetDestination(storedLocation.transform.position);
        TerrainGenerator.instance.treePlot.Remove(storedLocation);
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
