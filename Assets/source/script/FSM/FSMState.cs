using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

public class FSMState
{
    protected GameObject gameObject;
    protected Animator animator;
    protected string stateName;
    protected string changeString;
    public string stateString { get { return stateName; } }
    public FSMState(GameObject gameObject) { 
        this.gameObject = gameObject;
        changeString = stateName;
        animator = gameObject.GetComponent<Animator>();
    }
    virtual public void OnEnter()
    {
        Debug.Log(stateString + " enter");
        changeString = stateName;
        animator.SetTrigger(stateName);
    }
    virtual public void OnHold() { }
    virtual public void OnExit() { }
    virtual public bool CanChange() { return false; }
    virtual public bool CanEnter(FSMState currentState) { return false; }
    virtual public string ChangeString() { return changeString; }


}

public class staticFunction
{
    public static bool canSeeObject(GameObject gameObject,GameObject goalObject,float radius,float angle)
    {
        //能够看到目标
        float param = Vector3.Distance(gameObject.transform.position, goalObject.transform.position);
        if (param > radius) return false;//距离不够
        param = Vector3.Angle(gameObject.transform.forward, goalObject.transform.position - gameObject.transform.position);
        if (param > angle) return false;//角度不够
        if (Physics.Linecast(gameObject.transform.position, goalObject.transform.position, ~(1 << 8)))
            return false;//有遮挡
        return true;
    }

    public static bool canFeelObject(GameObject gameObject, GameObject goalObject, float feelRadius)
    {
        float param = Vector3.Distance(gameObject.transform.position, goalObject.transform.position);
        if (param > feelRadius) return false;//距离不够
        return true;
    }
}