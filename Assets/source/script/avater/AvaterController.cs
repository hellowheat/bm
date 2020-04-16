using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using System;

public class AvaterController : MonoBehaviour
{
    // Start is called before the first frame update
    public float speed=10;
    public Transform character;    //角色模型的transform
    public GameObject forwardObject;//面向的对象
    
    private bool allowOperater;     //允许操作
    private Rigidbody rb;           //自身刚体（自身包含角色模型和相机）
    private Animator animator;      //自身动画机s
    private AtkLogic atkLog;//攻击逻辑

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();
        allowOperater = true;
        atkLog = GetComponent<AtkLogic>();
        atkLog.setParam(animator, character, forwardObject) ;
    }

    // Update is called once per frame
    void Update()
    {
        if (allowOperater)
        {
            moveControl();
            atkLog.attackControl();
        }

    }


    void moveControl()
    {
        float v = Input.GetAxis("Vertical");
        float h = Input.GetAxis("Horizontal");
        Vector3 velocity = forwardObject.transform.forward * v * speed
            + Quaternion.AngleAxis(90, Vector3.up) * forwardObject.transform.forward * h * speed;
        velocity.y = 0;
        rb.velocity = velocity;
        if (velocity != Vector3.zero)character.forward = velocity.normalized;
        animator.SetFloat("speed", velocity.magnitude);
    }



}
