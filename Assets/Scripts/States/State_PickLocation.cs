using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class State_PickLocation : BaseState
{
    protected bool LocationPicked = false;
    protected Vector3 Location = Vector3.zero;

    public override void State_Init()
    {
        base.State_Init();
    }

    public override void State_Update()
    {
        base.State_Update();

        if(!LocationPicked)
        {
            LocationPicked = true;

            // Not ideal - Recommended using a blackboard system instead
            var randomPlantLoc = TerrainGenerator.instance.sphereList[Random.Range(0, TerrainGenerator.instance.sphereList.Count)];
            GetComponent<FSMCharacter>().LocationToRequest = new Vector2Int((int)randomPlantLoc.transform.position.x, (int)randomPlantLoc.transform.position.z);
        }
    }

    public override void State_Enter()
    {
        base.State_Enter();

        LocationPicked = false;
    }

    public override void State_Exit()
    {
        base.State_Exit();
    }

    public void CanTransition_ToMoveTo(TransitionResponse response)
    {
        response.CanTransition = LocationPicked;
    }
}
