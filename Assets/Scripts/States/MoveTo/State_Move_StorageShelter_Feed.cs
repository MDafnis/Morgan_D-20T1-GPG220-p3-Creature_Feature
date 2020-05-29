using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class State_Move_StorageShelter_Feed : BaseState
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
        GameObject storedObject = TerrainGenerator.instance.hungryTrees[Random.Range(0, TerrainGenerator.instance.hungryTrees.Count)];
        storedObject.GetComponent<ManageTree>().currentObjective = true;
        agent.curObjective = storedObject;
        TerrainGenerator.instance.hungryTrees.Remove(storedObject);
    }
    public override void State_Exit()
    {
        base.State_Exit();
    }

    public void CanTransition_ToCollectFood(TransitionResponse response)
    {
        response.CanTransition = agent.ReachedDestination;
    }
}
