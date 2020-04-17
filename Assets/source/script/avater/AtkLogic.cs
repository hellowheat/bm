using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class AtkLogic : MonoBehaviour
{
    public BoomerangController boomerang;//武器控制
    public int initNumber;  //初始数量
    public float atkCD;//攻击cd
    public MeshRenderer centerArrow;//中心箭头
    public MeshRenderer centerLoading;//中心加载
    [Header("boomerangUI")]
    public GameObject boomerangIcon;//飞镖图标
    public GameObject boomerangIconBox;//飞镖图标显示盒子
    
    private float waitAtkTime;  //距离上次攻击时间
    private AnimatorStateInfo lastAnimatorStateInfo_2;//动画机第二层layer上个动画
    private AtkNumberControl atkNumberControl;//武器数量控制
    private Animator animator;      //自身动画机s
    private Transform character;    //角色模型的transform
    private GameObject forwardObject;//面向的对象

    void Start()
    {
        isAtkPrepared = false;
        waitAtkTime = 10;
        atkNumberControl = new AtkNumberControl(boomerangIcon,boomerangIconBox,initNumber);
    }
    
    void Update()
    {
        waitAtkTime += Time.deltaTime;

        if (animator.GetBool("atkRelease"))
        {
            AnimatorStateInfo animatorStateInfo_2 = animator.GetCurrentAnimatorStateInfo(1);
            if (lastAnimatorStateInfo_2.fullPathHash != animatorStateInfo_2.fullPathHash)
            {
                lastAnimatorStateInfo_2 = animatorStateInfo_2;
                animator.SetBool("atkRelease", false);
            }
        }

        if (waitAtkTime < atkCD)
        {
            centerLoading.enabled = true;
            centerLoading.material.SetFloat("_Progress", waitAtkTime / atkCD);
        }
        else if (centerLoading.enabled == true) centerLoading.enabled = false;
    }

    public void setParam(Animator animator, Transform character, GameObject forwardObject)
    {
        this.animator = animator;
        this.character = character;
        this.forwardObject = forwardObject;
        lastAnimatorStateInfo_2 = animator.GetCurrentAnimatorStateInfo(1);
    }

    private bool isAtkPress = false;
    public void attackControl()
    {
        if (Input.GetButton("Attack"))
        {
            //按着攻击键
            isAtkPress = true;
            AttackBtnHold();
        }
        else if (isAtkPress)
        {
            //松开攻击键
            isAtkPress = false;
            AttackBtnRelease();
        }

    }

    private bool isAtkPrepared=false; //攻击是否准备
    public int AttackBtnHold()
    {
        if (atkNumberControl.hasNumber() && waitAtkTime > atkCD)
        {
            if (!isAtkPrepared)//之前没有准备好，刚刚才准备好
            {
                boomerang.AttackPrepare();
                centerArrow.enabled = true;
                centerLoading.enabled = false;
                isAtkPrepared = true;
            }
            else
            {
                boomerang.AttackPrepareHold();
            }
            return 0;
        }
        else if (!atkNumberControl.hasNumber()) return 1;//数量不够
        else
        {
            return 2;//cd不够
        }
    }
    public int AttackBtnRelease()
    {
        centerArrow.enabled = false;//取消中心显示
        isAtkPrepared = false;//未准备标记
        if (waitAtkTime > atkCD)
        {
            if (atkNumberControl.pop())
            {
                boomerang.AttackStart();
                waitAtkTime = 0;
                character.transform.DOLocalRotate(new Vector3(0, forwardObject.transform.rotation.eulerAngles.y, 0), 0.3f);
                lastAnimatorStateInfo_2 = animator.GetCurrentAnimatorStateInfo(1);
                animator.SetBool("atkRelease", true);
                return 0;
            }
            else return 1;//数量不够

        }
        else return 2;//cd不够
        
    }

    internal void pickBoomerang()
    {
        atkNumberControl.push();
    }

    class AtkNumberControl
    {
        public int number { get { return nowAtkNumber; } }
        private int nowAtkNumber;      //武器数量
        private int maxNumber;      //最大显示数量
        private List<GameObject> icons;
        private int showIconNumber;
        private GameObject box;
        public AtkNumberControl(GameObject preferb,GameObject box,int initNumber)
        {
            icons = new List<GameObject>();
            nowAtkNumber = initNumber;
            showIconNumber = 0;
            maxNumber = 5;
            for (int i = 0; i < maxNumber; i++)
            {
                GameObject gbj = GameObject.Instantiate(preferb,
                    box.transform.position + new Vector3(i*preferb.GetComponent<RectTransform>().rect.width/2,0,0),
                    box.transform.rotation,
                    box.transform);
                gbj.transform.localScale = Vector3.zero;
                icons.Add(gbj);
            }
            updateIcon();
        }
        public void push()
        {
            if(nowAtkNumber != -1)
            {
                nowAtkNumber++;
                updateIcon();
            }
        }

        public bool pop()
        {
            if (nowAtkNumber != 0)
            {
                if(nowAtkNumber != -1)
                {
                    nowAtkNumber--;
                    updateIcon();
                }
                return true;
            }
            else return false;
        }

        public bool hasNumber()
        {
            if (nowAtkNumber != 0)return true;
            else return false;
        }

        void updateIcon()
        {
            for(int i = 0; i < maxNumber; i++)
            {
                bool needIcon = (i == 0 && nowAtkNumber == -1) || nowAtkNumber > i;
                bool hasIcon = showIconNumber > i;
                string text=" ";
                if (nowAtkNumber == -1 && i == 0) text = "∞";
                else if (nowAtkNumber > maxNumber && i == maxNumber - 1) text = "+";
                if (needIcon && !hasIcon)
                {
                    //显示
                    icons[i].transform.DOScale(Vector3.one, 0.3f);
                }else if(!needIcon && hasIcon)
                {
                    icons[i].transform.DOScale(Vector3.zero, 0.3f);
                }
                icons[i].transform.GetChild(0).GetComponent<Text>().text = text;
            }
            showIconNumber = nowAtkNumber;
        }
    }
}
