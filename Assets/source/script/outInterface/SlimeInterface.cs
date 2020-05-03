using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlimeInterface : OutInterface
{
    Animator animator;

    public override void init()
    {
        animator = GetComponent<Animator>();
    }

    public override void beAttack(float damage)
    {
        if (!life.isDead)
        {
            Debug.Log("Slime beAttack");
            life.beAttack(damage);
            if (!life.isDead)
                animator.SetTrigger("BeHit");
        }
    }
}
