using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class State_Move_ChargingStation : BaseState
{
    private float previousDestinationTreshold;
    private float previousPointReachedThreshold;

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
        previousDestinationTreshold = agent.DestinationThreshold;
        previousPointReachedThreshold = agent.PointReachedThreshold;

        agent.DestinationThreshold = 7;
        agent.PointReachedThreshold = 7f;

        agent.SetDestination(TerrainGenerator.instance.chargingStation.transform.position);
    }
    public override void State_Exit()
    {
        agent.DestinationThreshold = previousDestinationTreshold;
        agent.PointReachedThreshold = previousPointReachedThreshold;
    }

    public void CanTransition_ToCharge(TransitionResponse response)
    {
        response.CanTransition = agent.ReachedDestination;
    }
}
