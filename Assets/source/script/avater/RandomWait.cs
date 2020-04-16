using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomWait : MonoBehaviour
{
    private Animator animator;      //自身动画机
    private float waitTime;         //站立时间（从上个休息动画开始）
    private AnimatorStateInfo lastAnimatorStateInfo;//动画机第一层layer上个动画
    void Start()
    {
        animator = GetComponent<Animator>();
        lastAnimatorStateInfo = animator.GetCurrentAnimatorStateInfo(0);
    }

    // Update is called once per frame
    void Update()
    {
        waitTime += Time.deltaTime;
        if (animator.GetBool("isAtk")) waitTime = 0;
        if (waitTime >= 4)
        {
            waitTime = 0;
            int rand = Random.Range(0, 4);
            if (rand != 0)
            {
                animator.SetBool("wait0" + rand.ToString(), true);
                waitTime = -2;
            }
        }
        for (int i = 1; i <= 3; i++)
        {
            if (animator.GetBool("wait0" + i))
            {
                AnimatorStateInfo animatorStateInfo = animator.GetCurrentAnimatorStateInfo(0);

                if (lastAnimatorStateInfo.fullPathHash != animatorStateInfo.fullPathHash)
                {
                    lastAnimatorStateInfo = animatorStateInfo;
                    animator.SetBool("wait0" + i, false);
                }
            }
        }
    }
}
