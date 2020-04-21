using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoomerangController : MonoBehaviour
{
    public GameObject model;//模型
    public Transform displayTransform;//生成位置
    public Transform cameraLookPos;//屏幕中心
    public float bronTime;  //生成时间
    public float preAtkTime;//扔出硬直
    public float speed;//扔出速度

    [Header("TrackBall")]
    public Transform ballBox;
    public GameObject ballPrfb;
    public float ballBronDis;
    private ObjectPool ballPool;
    private List<GameObject> balls;
    private float lastBallBronTime;

    private Rigidbody rb;
    private MeshRenderer mr;
    private Material mt;
    private AtkState atkState;
    private float hasBronTime;
    private bool isCollised;//飞行时发生了碰撞

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

        clearBall();
        atkState = AtkState.bron;
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
       /* float sl = Vector3.Distance(cameraLookPos.position - new Vector3(0, cameraLookPos.position.y, 0), displayTransform.position - new Vector3(0, displayTransform.position.y, 0));
        float su = cameraLookPos.position.y - displayTransform.position.y;
        float a = -9.8f ;

        //联立s=vt+at^2/2 ，0-v=at得-at^/2=s => t=sqrt(-2s/a)
        float t = su > 0 ? Mathf.Sqrt(-2 * su / a) : 0.1f ;
        float vl = sl / t;//水平速度
        float vu = -a * t * (su>0?1:0);//向上速度
        Debug.Log("time:" + t);
        Vector3 velocity = (transform.forward-new Vector3(0,transform.forward.y,0)).normalized * vl;
        velocity.y = vu;*/
        rb.velocity = (cameraLookPos.position - displayTransform.position).normalized * speed;
        rb.useGravity = true;
        atkState = AtkState.fly;
        isCollised = false;
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
            }
        }

        if (atkState == AtkState.fly && rb.velocity != Vector3.zero ) 
        {
            if (lastBallBronTime > ballBronDis)
            {
                lastBallBronTime = 0;
                balls.Add(ballPool.create(transform));
            }
            else lastBallBronTime += Time.deltaTime;

            if(!isCollised) transform.forward = rb.velocity.normalized;
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
        isCollised = true;
        if (atkState == AtkState.fly && collision.transform.tag == "moster")
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
            clearBall();
            mr.enabled = false;
        }
    }

    private void clearBall()
    {
        for(int i = 0; i < balls.Count; i++)
        {
            ballPool.destroy(balls[i]);
        }
        balls.Clear();
    }
}
