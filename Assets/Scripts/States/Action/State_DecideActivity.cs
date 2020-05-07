using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class State_DecideActivity : BaseState
{
    private int randomChoice;
    private bool reDecide = false;

    public override void State_Init()
    {

    }

    public override void State_Update()
    {

    }

    public override void State_Enter()
    {
        randomChoice = Random.Range(1, 2);
        switch (randomChoice)
        {
            case 1:
                if(TerrainGenerator.instance.treePlot.Count == 0)
                {
                    reDecide = true;
                }
                else
                {

                }
                break;
            case 2:
                if(TerrainGenerator.instance.plantedTrees.Count > 0)
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

    }

    public void CanTransition_ToMoveToStorage(TransitionResponse response)
    {
        response.CanTransition = reDecide != true && randomChoice == 1;
    }

    public void CanTransition_ToReDecide(TransitionResponse response)
    {
        response.CanTransition = reDecide && randomChoice == 1 || reDecide && randomChoice == 2;
    }

    public void CanTransition_ToDecideTreeConsumption(TransitionResponse response)
    {
        response.CanTransition = reDecide != true && randomChoice == 2;
    }
}
