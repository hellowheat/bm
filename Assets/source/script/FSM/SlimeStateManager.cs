using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class SlimeStateManager : AvatarStateManager
{
    public TrackMap trackMap;
    public GameObject atkObject;
    [Header("Patrol")]
    public float patrolSpeed;
    [Header("Accident Player")]
    public float seeRadius;//可视距离
    public float seeAngle;//可视宽度
    public float feelRadius;//感知范围
    [Header("Fllow")]
    public float allowLoseTime;
    public float followSpeed;
    [Header("Attack")]
    public float atkDamage;
    public float atkRadius;
    public float atkAngle;
    public float atkCD;
    public float atkPreTime;
    public float atkDamageCalcTime;
    [Header("Find")]
    public float findTime;
    public float findAngle;
    //Idle, Patrol, Accidents, Follow, Atk, Find, Dead
    //站立，巡逻，警惕，追击，攻击，寻找，死亡
    protected override void init()
    {
        stateMachine.Add(new moster_slime.Idle(gameObject));
        stateMachine.Add(new moster_slime.Patrol(gameObject, trackMap, patrolSpeed)) ;
        stateMachine.Add(new moster_slime.Accidents(gameObject, atkObject,seeRadius,seeAngle, feelRadius,findAngle));
        stateMachine.Add(new moster_slime.Follow(gameObject, atkObject, seeRadius, seeAngle, allowLoseTime, followSpeed)) ;
        stateMachine.Add(new moster_slime.Atk(gameObject,atkObject,atkCD,atkPreTime,atkDamage,atkRadius,atkAngle, atkDamageCalcTime));
        stateMachine.Add(new moster_slime.Find(gameObject,atkObject,findTime,findAngle));
        stateMachine.Add(new moster_slime.Dead(gameObject));
    }
    

}
