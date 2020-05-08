using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class State_CollectSappling : BaseState
{
    public override void State_Init()
    {

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

    public void CanTransition_ToMoveTreePlot(TransitionResponse response)
    {
       response.CanTransition = true; //Change the true value when you want to add animations(Maybe make a bool to tick on once animation is done.).
    }
}
