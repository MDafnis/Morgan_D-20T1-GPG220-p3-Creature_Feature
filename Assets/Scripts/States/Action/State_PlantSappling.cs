using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class State_PlantSappling : BaseState
{
    public override void State_Init()
    {
        //TerrainGenerator terrain = GetComponent<TerrainGenerator>();
        //for (int i = 0; i < terrain.treePlot.Count; i++)
        //{
        //
        //}
    }

    public override void State_Update()
    {

    }

    public override void State_Enter()
    {

    }

    public override void State_Exit()
    {

    }

    public void CanTransition_ToIdle(TransitionResponse response)
    {
        //response.CanTransition = IdleTimeRemaining <= 0;
    }
}
