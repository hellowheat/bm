using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class lifeManager : MonoBehaviour
{
    public float lifeLimit;
    public float offsetHigh;
    public float offsetRotate;
    [Header("lifeLine")]
    public bool useLifeLine;
    public GameObject lifeLinePrefab;
    public bool isShowFullLife;
    public bool isShowLine;
    public bool isShowText;
    private lifeLine lifeLineController;
    [Header("lifeUI")]
    public bool useLifeUI;
    public GameObject lifeUIBox;
    public GameObject lifeUIPrefab;
    private ObjectPool lifeUIPool;


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
        if (useLifeLine)
        {
            lifeLinePrefab = Instantiate(lifeLinePrefab, transform.position + Vector3.up * offsetHigh, Quaternion.Euler(lifeLinePrefab.transform.rotation.eulerAngles + new Vector3(0, offsetRotate, 0)));
            lifeLinePrefab.transform.SetParent(transform);
            lifeLineController = lifeLinePrefab.GetComponent<lifeLine>();
            lifeLineController.init(lifeLimit, isShowFullLife, isShowLine, isShowText);
        }
        if (useLifeUI)
        {
            lifeUIPool = new ObjectPool(lifeUIBox, transform);
        }
        updateLifeShow();
    }
    float tm;
    void Update()
    {

    }

    public void resetLife()
    {
        nowlife = lifeLimit;
        updateLifeShow();
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
            updateLifeShow();
        }
    }

    public void beTreat()
    {
        
    }

    private void updateLifeShow()
    {
        if(useLifeLine)
            lifeLineController.setLife(nowlife);
    }
}
