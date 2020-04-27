using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

public class FSMState
{
    GameObject gameObject;
    public FSMState(GameObject gameObject) { this.gameObject = gameObject; }
    virtual public void OnEnter() { }
    virtual public void OnHold() { }
    virtual public void OnExit() { }

}
