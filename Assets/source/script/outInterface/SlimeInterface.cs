using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlimeInterface : OutInterface
{
    Animator animator;
    Vector3 beAtkPos;
    public override void init()
    {
        animator = GetComponent<Animator>();
        beAtkPos = Vector3.zero;
    }

    public override void beAttack(float damage,GameObject atkGb)
    {
        if (atkGb != null) beAtkPos = atkGb.transform.position;
        else beAtkPos = transform.position + transform.forward;
        if (!life.isDead)
        {
            life.beAttack(damage);
            if (!life.isDead)
                animator.SetTrigger("BeHit");
        }
    }

    public override Vector3 getBeAtkPos()
    {
        return beAtkPos;
    }
}
