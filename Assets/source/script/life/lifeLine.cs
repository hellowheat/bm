using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class lifeLine : MonoBehaviour
{
    Renderer render;
    TextMesh valueMesh;
    float goalRadio;
    float nowRadio;
    float speed;
    
    void Start()
    {
        render = GetComponent<Renderer>();
        valueMesh = transform.GetChild(0).gameObject.GetComponent<TextMesh>();
    }

    // Update is called once per frame
    void Update()
    {
        if (render && render.enabled)
        {
            if (nowRadio != goalRadio)
            {
                float deltaValue = speed * Time.deltaTime;
                float distance = goalRadio - nowRadio;
                if (deltaValue > Mathf.Abs(distance))
                    nowRadio = goalRadio;
                else nowRadio += deltaValue * Mathf.Sign(distance);
                render.material.SetFloat("_Threshold", nowRadio);
            }
        }
    }

    public void init()
    {
        render = GetComponent<Renderer>();
        valueMesh = transform.GetChild(0).gameObject.GetComponent<TextMesh>();
        speed = 0.3f;
        nowRadio = 1;
    }

    public void setShow(bool isShowLine, bool isShowText)
    {
        render.enabled = isShowLine;
        valueMesh.gameObject.SetActive(isShowText);
    }

    public void setLine(float radio)
    {
        goalRadio = radio;
    }

    public void setLineAndValue(float radio, string value)
    {
        setLine(radio);
        valueMesh.text = value;
    }

}
