using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class lifeLine : MonoBehaviour
{
    public float speed;
    Renderer render;
    TextMesh textValueMesh;
    float goalValue;
    float nowValue;
    float lifeLimit;
    bool isFullShow;
    bool isLineShow;
    bool isTextShow;


    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if ((isLineShow || isTextShow) && nowValue != goalValue)
        {
            float deltaValue = speed * Time.deltaTime;
            float distance = goalValue - nowValue;
            if (deltaValue > Mathf.Abs(distance))
                nowValue = goalValue;
            else nowValue += deltaValue * Mathf.Sign(distance);
            if(isLineShow)
                render.material.SetFloat("_Threshold2", nowValue/lifeLimit);
            if(isTextShow)
                textValueMesh.text = (int)nowValue + "/" + (int)lifeLimit;

            if(!isFullShow)
            {//隐藏满血
                setShow(isLineShow && nowValue < lifeLimit, isTextShow && nowValue < lifeLimit);
            }
        }
    }

    public void init(float lifeLimit,bool isFullShow,bool isLineShow,bool isTextShow)
    {
        render = GetComponent<Renderer>();
        textValueMesh = transform.GetChild(0).gameObject.GetComponent<TextMesh>();
        this.lifeLimit = lifeLimit;
        nowValue = lifeLimit;
        goalValue = lifeLimit;

        this.isFullShow = isFullShow;
        this.isLineShow = isLineShow;
        this.isTextShow = isTextShow;
        if (isFullShow) setShow(isLineShow, isTextShow);
        else setShow(false, false);
    }

    public void setShow(bool isShowLine, bool isShowText)
    {
        render.enabled = isShowLine;
        textValueMesh.gameObject.SetActive(isShowText);
    }

    public void setLife(float goalValue)
    {
        if (isLineShow)
        {
            render.material.SetFloat("_Threshold", goalValue / lifeLimit);
            if(goalValue > this.goalValue)//加血
            {
                nowValue = goalValue;
                render.material.SetFloat("_Threshold2", goalValue / lifeLimit);
                if (isFullShow)
                {//隐藏满血
                    setShow(isLineShow && nowValue < lifeLimit, isTextShow && nowValue < lifeLimit);
                }
            }
        }
        this.goalValue = goalValue;
    }


}
