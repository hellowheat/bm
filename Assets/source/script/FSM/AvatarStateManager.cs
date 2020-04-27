using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class AvatarStateManager : MonoBehaviour
{
    protected StateMachine stateMachine;
    void Start()
    {
        stateMachine = new StateMachine();
        init();
    }

    // Update is called once per frame
    void Update()
    {
        stateMachine.Update();
        updateState();
    }

    virtual protected void init(){}
    virtual protected void updateState() {}


}
