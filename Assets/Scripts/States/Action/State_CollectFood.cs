using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class State_CollectFood : BaseState
{
    public virtual void State_Init()
    {

    }

    public virtual void State_Update()
    {

    }

    public virtual void State_Enter()
    {

    }

    public virtual void State_Exit()
    {

    }

    public void CanTransition_ToMoveHungryTree(TransitionResponse response)
    {
        //response.CanTransition = IdleTimeRemaining <= 0;
    }
}
