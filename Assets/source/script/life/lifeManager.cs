using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class lifeManager : MonoBehaviour
{
    public float lifeLimit;
    public float offsetHigh;
    [Header("lifeLine")]
    public bool isShowLifeFull;
    public bool isShowLifeNotFull;
    public bool isShowText;
    public GameObject lifeLinePreferb;
    private lifeLine lifeLineController;
    public delegate void DealDead();//失败时调用的方法
    private float nowlife;
    DealDead m_dealDeadFunc;
    public float Nowlife { get => nowlife;}

    // Start is called before the first frame update
    void Start()
    {
        nowlife = lifeLimit;
        m_dealDeadFunc = null;
        if (isShowLifeFull || isShowLifeNotFull)
        {
            lifeLinePreferb = Instantiate(lifeLinePreferb, transform.position + Vector3.up * offsetHigh, lifeLinePreferb.transform.rotation);
            lifeLinePreferb.transform.SetParent(transform);
            lifeLineController = lifeLinePreferb.GetComponent<lifeLine>();
            lifeLineController.init();
            updateLifeLine();
        }
    }
    float tm;
    // Update is called once per frame
    void Update()
    {
        /*tm += Time.deltaTime;
        if (tm > 4)
        {
            tm = 0;
            beAttack(Random.Range(5, 20));
        }*/
    }

    public void resetLife()
    {
        nowlife = lifeLimit;
        updateLifeLine();
    }

    public void setDeadDeal(DealDead dealDead)
    {
        m_dealDeadFunc = dealDead;
    }

    public void beAttack(float damage)
    {
        if(nowlife > 0)
        {
            nowlife -= damage;
            if (nowlife <= 0)
            {
                nowlife = 0;
                m_dealDeadFunc?.Invoke();
            }
            updateLifeLine();
        }
    }

    private void updateLifeLine()
    {
        //if ((nowlife >= lifeLimit && isShowLifeFull) || (nowlife < lifeLimit && isShowLifeNotFull))
        {
            Debug.Log(nowlife / lifeLimit);
            if (isShowText)
                lifeLineController.setLineAndValue(nowlife / lifeLimit, (int)nowlife + "/" + (int)lifeLimit);
            else lifeLineController.setLine(nowlife / lifeLimit);
        }

    }
}
