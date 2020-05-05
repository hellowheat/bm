using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoomerangController : MonoBehaviour
{
    public GameObject model;//模型
    public ParticleSystem particle;//粒子系统
    public Transform displayTransform;//生成位置
    public Transform cameraLookPos;//屏幕中心
    public float bronTime;  //生成时间
    public float preAtkTime;//扔出硬直
    public float speed;//扔出速度
    public float damage;//武器伤害


    [Header("TrackBall")]
    public Transform ballBox;
    public GameObject ballPrfb;
    public float maxBallNumber;
    public float ballBronDis;

    [Header("Trail")]
    public TrailRenderer trailRenderer;
    private ObjectPool ballPool;
    private List<GameObject> balls;
    private float lastBallBronTime;

    private Rigidbody rb;
    private MeshRenderer mr;
    private Material mt;
    private AtkState atkState;
    private float hasBronTime;
    private bool hasDamage;
    private bool hasCollised;//飞行时发生了碰撞

    private enum AtkState
    {
        rest,bron, drop,fly,wait,//休息，产生,扔出，飞行，等待回收
       
    }
    public void AttackPrepare()
    {
        //初始化武器刚体及位置
        rb.useGravity = false;
        rb.velocity = Vector3.zero;
        rb.freezeRotation = true;
        rb.freezeRotation = false;
        transform.position = displayTransform.position;
        transform.forward = cameraLookPos.position - displayTransform.position;

        //初始化变量
        hasBronTime = 0;
        mr.enabled = true;
        mt.SetFloat("_threshold", 0);
        atkState = AtkState.bron;

        //初始化场景特效
        clearTrailTrack();
        particle.Play();
    }

    public void AttackPrepareHold()
    {
        transform.position = displayTransform.position;
        Vector3 forward = cameraLookPos.position - displayTransform.position;
        forward.y = 0;
        transform.forward = forward;

    }

    public void AttackStart()
    {
        particle.Stop();

        mt.SetFloat("_threshold", 1);
        rb.velocity = Vector3.zero;
        hasDamage = false;

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
        rb.velocity = (cameraLookPos.position - displayTransform.position).normalized * speed;
        rb.useGravity = true;
        atkState = AtkState.fly;
        trailRenderer.enabled = true;
        hasCollised = false;
    }

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        mt = model.GetComponent<Renderer>().material;
        mr = model.GetComponent<MeshRenderer>();
        ballPool = new ObjectPool(ballPrfb,ballBox);
        atkState = AtkState.rest;
        balls = new List<GameObject>();
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
                particle.Stop();

            }
        }

        if (atkState == AtkState.fly && rb.velocity != Vector3.zero ) 
        {
            if(balls.Count < maxBallNumber)
            {
                if (lastBallBronTime > ballBronDis)
                {
                    lastBallBronTime = 0;
                    balls.Add(ballPool.create(transform));
                }
                else lastBallBronTime += Time.deltaTime;
            }

            if(!hasCollised) transform.forward = rb.velocity.normalized;
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
        hasCollised = true;
        
        if (atkState == AtkState.fly && collision.transform.CompareTag("moster"))
        {
            //碰撞到怪物
            if (!hasDamage)
            {
                collision.gameObject.GetComponent<OutInterface>().beAttack(damage, gameObject) ;
                hasDamage = true;
            }
        }
        else if(atkState == AtkState.fly && collision.transform.CompareTag("ground"))
        {
            //碰撞到地面
            atkState = AtkState.wait;
        }
        else if((atkState == AtkState.wait || atkState == AtkState.fly) && collision.transform.CompareTag("avater"))
        {
            collision.gameObject.GetComponent<OutInterface>().pushProp("boomerang");
            atkState = AtkState.rest;
            clearTrailTrack();
            mr.enabled = false;
        }
    }

    private void clearTrailTrack()
    {
        for(int i = 0; i < balls.Count; i++)
        {
            ballPool.destroy(balls[i]);
        }
        balls.Clear();
        trailRenderer.enabled = false;
    }
}
