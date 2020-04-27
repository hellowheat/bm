using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateMachine
{
    // Start is called before the first frame update
    private Dictionary<string, FSMState> states;
    private FSMState lastState { get; set; }
    private FSMState currentState { get; set; }

    public StateMachine()
    {
        states = new Dictionary<string, FSMState>();
        lastState = null;
        currentState = null;
    }

    public void Add(string stateString,FSMState state)
    {
        states.Add(stateString, state);
        if (states.Count == 1) TranslateState(stateString);
    }

    public void TranslateState(string str)
    {
        if (states.ContainsKey(str))
        {
            FSMState newState = states[str];
            if (currentState != newState)
            {
                if (currentState != null)
                {
                    currentState.OnExit();
                    lastState = currentState;
                }
                currentState = states[str];
                currentState.OnEnter();
            }

        }
    }

    public void Update()
    {
        if (currentState != null)currentState.OnHold();
    }
}
