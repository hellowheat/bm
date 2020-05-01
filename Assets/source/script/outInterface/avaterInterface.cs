using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class avaterInterface : OutInterface
{
    lifeManager life;
    Animator animator;
    void Start()
    {
        animator = GetComponent<Animator>();
        life = GetComponent<lifeManager>();
    }

    void Update()
    {
        
    }

    public override void beAttack(float damage)
    {
        Debug.Log("avater beAttack");
        life.beAttack(damage);
    }
}
