using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class avaterInterface : OutInterface
{
    Animator animator;
    public override void init()
    {
        animator = GetComponent<Animator>();
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
}
