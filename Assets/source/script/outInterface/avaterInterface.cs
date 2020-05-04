using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class avaterInterface : OutInterface
{
    AtkLogic atkLogic;
    Animator animator;
    headHitSystem propHit;
    public override void init()
    {
        atkLogic = GetComponent<AtkLogic>();
        animator = GetComponent<Animator>();
        propHit = GetComponent<headHitSystem>();
    }


    public override void beAttack(float damage)
    {
        Debug.Log("avater beAttack");
        life.beAttack(damage);
    }

    public override bool isDead()
    {
        return life.isDead;
    }

    public override void pushProp(string propStr)
    {
        if(propStr == "boomerang")
        {
            atkLogic.pickBoomerang();
            propHit.Push(0);
        }
    }
}
