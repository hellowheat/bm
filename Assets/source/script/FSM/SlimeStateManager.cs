using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class SlimeStateManager : AvatarStateManager
{
    public TrackMap trackMap;
    public GameObject atkObject;
    public float seeRadius;//可视距离
    public float seeAngle;//可视宽度
    NavMeshAgent agent;
    Animator animator;
    enum SlimeState
    {
        Idle, Patrol, Accidents, Follow, Atk, Find, Dead
        //站立，巡逻，警惕，追击，攻击，寻找，死亡
    }
    
    protected override void init()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        stateMachine.Add(new moster_slime.Idle(gameObject));
        stateMachine.Add(new moster_slime.Patrol(gameObject, trackMap));
        stateMachine.Add(new moster_slime.Accidents(gameObject, atkObject,seeRadius,seeAngle));
        stateMachine.Add(new moster_slime.Follow(gameObject, atkObject,seeRadius,seeAngle));
        stateMachine.Add(new moster_slime.Atk(gameObject));
        stateMachine.Add(new moster_slime.Find(gameObject));
        stateMachine.Add(new moster_slime.Dead(gameObject));
    }
    
     protected void updateState() {
       
    }

}
