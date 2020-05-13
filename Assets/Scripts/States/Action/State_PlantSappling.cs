using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class State_PlantSappling : BaseState
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
        agent.curObjective.GetComponent<ManageTree>().PlantTree();
    }

    public override void State_Exit()
    {
		base.State_Exit();
    }

    public void CanTransition_ToIdle(TransitionResponse response)
    {
        response.CanTransition = true; //Change the true value when you want to add animations(Maybe make a bool to tick on once animation is done.).
    }
}
