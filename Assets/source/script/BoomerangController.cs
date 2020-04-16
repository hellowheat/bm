using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoomerangController : MonoBehaviour
{
    public GameObject model;//模型
    public Transform displayTransform;//生成位置
    public Transform cameraLookPos;//屏幕中心
    public float speed;     //初始速度
    public float bronTime;  //生成时间
    public float preAtkTime;//扔出硬直

    private Rigidbody rb;
    private MeshRenderer mr;
    private Material mt;
    private AtkState atkState;
    private float hasBronTime;

    private enum AtkState
    {
        rest,bron, drop,fly,wait,//休息，产生,扔出，飞行，等待回收
       
    }
    public void AttackPrepare()
    {
        rb.useGravity = false;
        rb.velocity = Vector3.zero;
        rb.freezeRotation = true;
        rb.freezeRotation = false;

        transform.position = displayTransform.position;
        transform.forward = cameraLookPos.position - displayTransform.position;

        hasBronTime = 0;
        mr.enabled = true;
        mt.SetFloat("_threshold", 0);

        atkState = AtkState.bron;
    }

    public void AttackPrepareHold()
    {
        transform.position = displayTransform.position;
        transform.forward = cameraLookPos.position - displayTransform.position;

    }

    public void AttackStart()
    {

        mt.SetFloat("_threshold", 1);
        rb.velocity = Vector3.zero;

        atkState = AtkState.drop;
        StartCoroutine(drop());
    }

    IEnumerator drop()
    {
        float waitTime = 0;
        while (waitTime < preAtkTime)
        {
            yield return 0;
            waitTime += Time.deltaTime;
        }
        rb.velocity = transform.forward * speed;
        rb.useGravity = true;
        atkState = AtkState.fly;
        //rb.AddForce(transform.forward * speed, ForceMode.Force);
    }

    void pick(AtkLogic atkL)
    {
        atkL.pickBoomerang();
        mr.enabled = false;
    }

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        mt = model.GetComponent<Renderer>().material;
        mr = model.GetComponent<MeshRenderer>();
        atkState = AtkState.rest;
    }


    void Update()
    {
       if (mt.GetFloat("_threshold") != 1)
        {
            if(hasBronTime< bronTime)
            {
                hasBronTime += Time.deltaTime;
                mt.SetFloat("_threshold", hasBronTime / bronTime);
            }
            else
            {
                mt.SetFloat("_threshold", 1);
            }
        }
        
       if(atkState == AtkState.wait)
        {

        }
    }
    private float waitTime = 0;
    private void OnCollisionStay(Collision collision)
    {
        if (atkState == AtkState.fly && rb.velocity == Vector3.zero)
        {
            atkState = AtkState.wait;
            if (waitTime < 3) waitTime++;
        }

    }
    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log(collision.transform.tag);
        if(atkState == AtkState.fly && collision.transform.tag == "moster")
        {
            //碰撞到怪物
        }else if(atkState == AtkState.fly && collision.transform.tag == "ground")
        {
            //碰撞到地面
            atkState = AtkState.wait;
        }
        else if((atkState == AtkState.wait || atkState == AtkState.fly) && collision.transform.tag == "avater")
        {
            pick(collision.gameObject.GetComponent<AtkLogic>());
            atkState = AtkState.rest;
            mr.enabled = false;
        }
    }
}
