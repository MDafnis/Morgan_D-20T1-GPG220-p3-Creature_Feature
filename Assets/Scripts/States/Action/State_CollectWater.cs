﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class State_CollectWater : BaseState
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

    public void CanTransition_ToMoveThirstyTree(TransitionResponse response)
    {
        //response.CanTransition = IdleTimeRemaining <= 0;
    }
}
