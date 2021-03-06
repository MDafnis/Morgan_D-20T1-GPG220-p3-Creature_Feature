﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class State_DecideTreeConsumption : BaseState
{
    private int randomChoice = 0;
    private bool reDecide = false;

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
        randomChoice = Random.Range(1, 3); // The number will never hit the max.
        reDecide = false;
        switch (randomChoice)
        {
            case 1:
                if (TerrainGenerator.instance.thirstyTrees.Count > 0)
                {

                }
                else
                {
                    reDecide = true;
                }
                break;
            case 2:
                if (TerrainGenerator.instance.hungryTrees.Count > 0)
                {

                }
                else
                {
                    reDecide = true;
                }
                break;
        }
    }

    public override void State_Exit()
    {
		base.State_Exit();
    }

    public void CanTransition_ToMoveToWaterShed(TransitionResponse response)
    {
        response.CanTransition = reDecide != true && randomChoice == 1;
    }

    public void CanTransition_ToIdle(TransitionResponse response)
    {
        response.CanTransition = reDecide;
        agent.skipWaitingPeriod = true;
    }

    public void CanTransition_ToMoveToStorage(TransitionResponse response)
    {
        response.CanTransition = reDecide != true && randomChoice == 2;
    }
}
