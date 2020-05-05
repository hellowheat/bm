using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AvaterInterface : OutInterface
{
    AtkLogic atkLogic;
    GameObject beAtkObj;
    Animator animator;
    HeadHitSystem propHit;
    public override void init()
    {
        atkLogic = GetComponent<AtkLogic>();
        animator = GetComponent<Animator>();
        propHit = GetComponent<HeadHitSystem>();
        beAtkObj = null;
    }


    public override void beAttack(float damage, GameObject atkObj)
    {
        beAtkObj = atkObj;
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
