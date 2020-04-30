using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FSM : MonoBehaviour
{

    public List<BaseState> allStates;
    public BaseState initialState;

    protected BaseState currentState;
    protected BaseState nextState;

    // Start is called before the first frame update
    void Start()
    {
        currentState = initialState;

        foreach(BaseState state in allStates)
        {
            state.State_Init();
        }
    }

    // Update is called once per frame
    void Update()
    {
        // check all of the transitions for the state
        //BaseState newState = currentState.CheckTransitions();

        // have a new state?
        if(nextState != null)
        {
            currentState.State_Exit();

            currentState = nextState;

            currentState.State_Enter();
        }

        currentState.State_Update();
    }
}
