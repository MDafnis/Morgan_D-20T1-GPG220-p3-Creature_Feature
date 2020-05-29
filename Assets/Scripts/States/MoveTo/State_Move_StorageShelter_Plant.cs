using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class State_Move_StorageShelter_Plant : BaseState
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
        agent.SetDestination(TerrainGenerator.instance.storageShelter.transform.position);
        GameObject storedObject = TerrainGenerator.instance.treePlot[Random.Range(0, TerrainGenerator.instance.treePlot.Count)];
        agent.curObjective = storedObject;
        TerrainGenerator.instance.treePlot.Remove(storedObject);
    }
    public override void State_Exit()
    {
        base.State_Exit();
    }

    public void CanTransition_ToCollectSappling(TransitionResponse response)
    {
        response.CanTransition = agent.ReachedDestination;
    }
}
