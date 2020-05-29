using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class State_Move_WaterShelter : BaseState
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
        agent.SetDestination(TerrainGenerator.instance.waterShelter.transform.position);
        GameObject storedObject = TerrainGenerator.instance.thirstyTrees[Random.Range(0, TerrainGenerator.instance.thirstyTrees.Count)];
        storedObject.GetComponent<ManageTree>().currentObjective = true;
        agent.curObjective = storedObject;
        TerrainGenerator.instance.thirstyTrees.Remove(storedObject);
    }
    public override void State_Exit()
    {
        base.State_Exit();
    }

    public void CanTransition_ToWaterTree(TransitionResponse response)
    {
        response.CanTransition = agent.ReachedDestination;
    }
}
