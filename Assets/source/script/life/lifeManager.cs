using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class lifeManager : MonoBehaviour
{
    public float lifeLimit;
    public float offsetHigh;
    public float offsetRotate;
    [Header("lifeLine")]
    public bool isShowFullLife;
    public bool isShowLine;
    public bool isShowText;
    public GameObject lifeLinePreferb;
    private lifeLine lifeLineController;
    public delegate void DealDead();//失败时调用的方法
    private float nowlife;
    DealDead m_dealDeadFunc;
    public float Nowlife { get => nowlife;}
    public bool isDead { get => nowlife <= 0; }

    // Start is called before the first frame update
    void Start()
    {
        nowlife = lifeLimit;
        m_dealDeadFunc = null;
        lifeLinePreferb = Instantiate(lifeLinePreferb, transform.position + Vector3.up * offsetHigh, Quaternion.Euler(lifeLinePreferb.transform.rotation.eulerAngles + new Vector3(0, offsetRotate, 0))) ;
        lifeLinePreferb.transform.SetParent(transform);
        lifeLineController = lifeLinePreferb.GetComponent<lifeLine>();
        lifeLineController.init(lifeLimit,isShowFullLife,isShowLine,isShowText);
        updateLifeLine();
    }
    float tm;
    void Update()
    {

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

    public void beTreat()
    {
        
    }

    private void updateLifeLine()
    {
         lifeLineController.setLife(nowlife);
    }
}
