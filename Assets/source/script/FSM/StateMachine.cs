using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateMachine
{
    // Start is called before the first frame update
    private List<FSMState> states;
    private FSMState lastState;
    public FSMState currentState;

    public StateMachine()
    {
        states = new List<FSMState>();
        lastState = null;
        currentState = null;
    }

    public void Add(FSMState state)
    {
        states.Add(state);
        if (states.Count == 1) TranslateState(state.stateString);
    }

    public void TranslateState(string str)
    {
        if(currentState == null || str != currentState.stateString)
        {
            foreach(FSMState state in states)
            {
                if(state.stateString == str && currentState != state)
                {
                    if (currentState != null)
                    {
                        currentState.OnExit();
                        lastState = currentState;
                    }
                    currentState = state;
                    currentState.OnEnter();
                    break;
                }
            }
        }
        
    }

    public void Update()
    {
        if (currentState != null) currentState.OnHold();
        if (currentState.CanChange())
        {
            TranslateState(currentState.ChangeString());
        }

        //强制进入某状态
        foreach (FSMState state in states)
        {
            if (currentState != state && state.CanEnter())
            {
                TranslateState(state.stateString);
                break;
            }
        }
    }

    public bool isCurrentState(string str)
    {
        return str == currentState.stateString;
    }
}
