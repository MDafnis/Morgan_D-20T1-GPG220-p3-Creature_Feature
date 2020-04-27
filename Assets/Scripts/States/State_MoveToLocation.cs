using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class State_MoveToLocation : BaseState
{
    public float LocationReachedTresheld = 0.1f;
    public float MovementTime = 5f;
    protected float MovementProgress = -1f;

    public override void State_Init()
    {
        base.State_Init();
    }
    public override void State_Update()
    {
        base.State_Update();

        // This logic below would not be present if we ahd a navigation + pathfinding system.
        // If we had navigation + pathfinding this function wouldn't need to do anything.
        MovementProgress += Time.deltaTime / MovementTime;
        transform.position = Vector3.Lerp(StartLocation, TargetLocation, MovementProgress);
    }
    public override void State_Enter()
    {
        base.State_Enter();

        // This logic below would not be present if we had a navigation + pathfinding system.
        // if we had navigation + pathfinding this function wouldn't need to do anything.

        // Not ideal - Recommended using a blackboard system instead
        TargetLocation = GetComponent<FSMCharacter>().LocationToRequest;
        StartLocation = transform.position;
        MovementProgress = 0f;
    }
    public override void State_Exit()
    {
        base.State_Exit();
    }

    public void CanTransition_ToIdle(TransitionResponse response)
    {
        // This logic below would not be present if we had a navigation + pathfinding system.
        // If we had navigation + pathfinding this function would instead ask the navigation system if the destination had been reached.

        response.CanTransition = Vector3.Distance(transform.position, TargetLocation) <= LocationReachedTresheld;
    }
}
