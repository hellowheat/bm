using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace moster_slime
{
    public class Idle :FSMState
    {
        float stayTime;
        float allowStayTime; 
        public Idle(GameObject gameObject)
            :base(gameObject)
        {
            stateName = "Idle"; 
            allowStayTime = 1;
        }
        public override void OnEnter()
        {
            base.OnEnter();
            stayTime = 0;
        }
        override public void OnHold() { stayTime += Time.deltaTime; }
        override public void OnExit() { }

        public override bool CanChange()
        {
            if(allowStayTime < stayTime)
            {
                changeString = "Patrol";
                return true;
            }
            return false;
        }
    }
    public class Patrol : FSMState
    {
        NavMeshAgent agent;
        TrackMap trackMap;
        int posIndex;
        float patrolSpeed;

        public Patrol(GameObject gameObject, TrackMap trackMap,float patrolSpeed)
            : base(gameObject)
        {
            agent = gameObject.GetComponent<NavMeshAgent>();
            stateName = "Patrol";
            this.trackMap = trackMap;
            this.patrolSpeed = patrolSpeed;
            gameObject.transform.position = trackMap.getPosition(posIndex);
        }
        public override void OnEnter()
        {
            List<int> nearList = trackMap.getNear(posIndex);
            posIndex = nearList[Random.Range(0, nearList.Count)];
            agent.SetDestination(trackMap.getPosition(posIndex));
            agent.speed = patrolSpeed;
            base.OnEnter();
        }
        public override bool CanChange()
        {
            if (!agent.hasPath)
            {
                changeString = "Idle";
                return true;
            }
            return base.CanChange();
        }
    }
    public class Accidents : FSMState
    {
        GameObject goalGameObject;
        float seeRadius,seeAngle,feelRadius,findAngle;
        float accidentsTime = 0.5f;
        float stayTime;
        float rotateSpeed;
        public Accidents(GameObject gameObject, GameObject goalGameObject,float seeRadius, float seeAngle,float feelRadius,float findAngle)
            : base(gameObject)
        {
            stateName = "Accidents";
            rotateSpeed = 90;
            this.goalGameObject = goalGameObject;
            this.seeRadius = seeRadius;
            this.seeAngle = seeAngle;
            this.feelRadius = feelRadius;
            this.findAngle = findAngle;
        }

        public override void OnEnter()
        {
            base.OnEnter();
            gameObject.GetComponent<NavMeshAgent>().ResetPath();
            stayTime = 0;
        }
        public override void OnHold()
        {
            stayTime += Time.deltaTime;
            Vector3 v1 = gameObject.transform.forward;
            Vector3 v2 = goalGameObject.transform.position - gameObject.transform.position;
            float cross = v1.x * v2.z - v1.z * v2.x;
            float angle = Vector3.Angle(v1,v2);
            if (Time.deltaTime * rotateSpeed > angle)
            {
                v2.y = 0;
                if (v2 != Vector3.zero)gameObject.transform.forward = v2;
            }
            else
            {
                gameObject.transform.Rotate(Vector3.up, Time.deltaTime * rotateSpeed * -Mathf.Sign(cross)) ;
            }
            
        }
        public override bool CanChange()
        {
            if(stayTime > accidentsTime)
            {
                changeString = "Follow";
                return true;
            }
            return false;
        }
        public override bool CanEnter(FSMState currentState)
        {
            if (currentState.stateString != "Idle"
                && currentState.stateString != "Patrol"
                && currentState.stateString != "Find")
                return false;
            if(currentState.stateString == "Find") return staticFunction.canSeeObject(gameObject, goalGameObject, seeRadius*2, findAngle) || staticFunction.canFeelObject(gameObject, goalGameObject, feelRadius);
            else return staticFunction.canSeeObject(gameObject, goalGameObject, seeRadius, seeAngle) || staticFunction.canFeelObject(gameObject,goalGameObject,feelRadius);
        }

        
    }
    public class Follow : FSMState
    {
        NavMeshAgent agent;
        GameObject goalGameObject;
        Vector3 goalPosition;
        float followSpeed;
        float seeRadius,findAngle;
        float flashGoalPositonDis;//隔一定时间更新一下目标位置
        float flashGoalPositonTime;//统计已经隔了多少时间了
        float allowNoGoalTime;//丢失目标时间
        public Follow(GameObject gameObject, GameObject goalGameObject, float seeRadius, float findAngle, float allowLoseTime, float followSpeed)
            : base(gameObject)
        {
            stateName = "Follow";
            flashGoalPositonDis = 0.5f;
            allowNoGoalTime = allowLoseTime;
            this.seeRadius = seeRadius;
            this.findAngle = findAngle;
            this.goalGameObject = goalGameObject;
            this.followSpeed = followSpeed;
            agent = gameObject.GetComponent<NavMeshAgent>();
        }
        public override void OnEnter()
        {
            base.OnEnter();
            flashGoalPositonTime = 0;
            goalPosition = goalGameObject.transform.position;
            agent.SetDestination(goalPosition);
            agent.speed = followSpeed;
        }
        public override void OnHold()
        {
            flashGoalPositonTime += Time.deltaTime;
            if (flashGoalPositonTime > flashGoalPositonDis && staticFunction.canSeeObject(gameObject, goalGameObject, seeRadius, findAngle))
            {
                flashGoalPositonTime = 0;
                goalPosition = goalGameObject.transform.position;
                agent.SetDestination(goalPosition);
                
            }
        }

        public override bool CanChange()
        {
            if(flashGoalPositonTime > allowNoGoalTime || !agent.hasPath)
            {
                changeString = "Find";
                return true;
            }
            return false;
        }
    }
    public class Atk : FSMState
    {
        float atkRadius,atkAngle,atkDamage;
        float atkAnimationTime,atkDamageCalcTime;
        bool hasDamage;
        float atkCD,atkPre;
        float cacheTime;
        float findCd, lastFindTime;
        NavMeshAgent agent;
        GameObject goalGameObject;
        OutInterface goalLifeManager;
        AnimatorStateInfo lastStateInfo;
        int atkState;//0:冷却中，在Idle或其他状态。
            //1：等待跳转到startAtk动画。
            //2:执行startAtk状态，抬头前摇。
            //3:等待跳转到atk动画，
            //4：撞击状态

        public Atk(GameObject gameObject,GameObject goalGameObject,float atkCD,float atkPre,float atkDamage,float atkRadius,float atkAngle,float atkDamageCalcTime)
            : base(gameObject) 
        {
            agent = gameObject.GetComponent<NavMeshAgent>();
            goalLifeManager = goalGameObject.GetComponent<OutInterface>();
            stateName = "Atk";
            atkAnimationTime = 0.4f; 
            this.atkDamageCalcTime = atkDamageCalcTime;
            this.goalGameObject = goalGameObject;
            this.atkDamage = atkDamage;
            this.atkRadius = atkRadius;
            this.atkAngle = atkAngle;
            this.atkCD = atkCD;
            this.atkPre = atkPre;
        }
        public override void OnEnter()
        {
            cacheTime = atkCD;
            changeString = stateString;
            agent.ResetPath();
            hasDamage = true;
            lastFindTime = 0;
            atkState = 0;
            findCd = 0.5f;
            Debug.Log("atk enter");
        }

        public override void OnHold()
        {

            lastFindTime += Time.deltaTime;
            cacheTime += Time.deltaTime;
            if (atkState == 0)
            {
                Debug.Log("atkState in 0");
                if (cacheTime > atkCD)
                {
                    atkState = 1;
                    animator.SetTrigger("Atk");
                    lastStateInfo = animator.GetCurrentAnimatorStateInfo(0);
                }
            }else if(atkState == 1)
            {
                //等待跳转到startAtk动画
                if (animator.GetCurrentAnimatorStateInfo(0).fullPathHash != lastStateInfo.fullPathHash)
                {
                    lastStateInfo = animator.GetCurrentAnimatorStateInfo(0);
                    atkState = 2;
                    cacheTime = 0;
                }
            }
            else if (atkState == 2)
            {
                Debug.Log("atkState in 2,cacheTIme:"+cacheTime);
                if (cacheTime > atkPre)
                {
                    lastStateInfo = animator.GetCurrentAnimatorStateInfo(0);
                    animator.SetTrigger("AtkHasPre");
                    atkState = 3;
                    cacheTime = 0;
                    hasDamage = false;
                }
            }else if(atkState == 3)
            {
                Debug.Log("atkState in 3");
                if (animator.GetCurrentAnimatorStateInfo(0).fullPathHash != lastStateInfo.fullPathHash)
                {
                    lastStateInfo = animator.GetCurrentAnimatorStateInfo(0);
                    atkState = 4;
                    cacheTime = 0;
                }
            }
            else if(atkState == 4)
            {
                Debug.Log("atkState in 4");
                //攻击中
                if (!hasDamage && cacheTime > atkDamageCalcTime && isGoalInAtkRadius())
                {
                    hasDamage = true;
                    goalLifeManager.beAttack(atkDamage);
                }

                if(animator.GetCurrentAnimatorStateInfo(0).fullPathHash != lastStateInfo.fullPathHash)
                {
                    atkState = 0;
                    cacheTime = 0;
                }
            }
        }
        public override bool CanEnter(FSMState currentState)
        {
            if(currentState.stateString == "Follow" && isGoalInAtkRadius()) 
            {
                return true;
            }
            return false;
        }

        public override bool CanChange()
        {
            if (lastFindTime > findCd)
            {
                lastFindTime = 0;
                if ((atkState != 4) && !isGoalInAtkRadius()) 
                {
                    changeString = "Find";
                    return true;
                }
            }
            return false;
        }

        bool isGoalInAtkRadius()
        {
            return staticFunction.canSeeObject(gameObject, goalGameObject, atkRadius * 2 / 3, atkAngle);
        }
    }
    public class Find : FSMState
    {
        GameObject goalGameObject;
        float findTime,findAngle;
        float hasFoundTime;
        public Find(GameObject gameObject,GameObject goalObject,float findTime,float findAngle)
            : base(gameObject)
        {
            stateName = "Find";
            this.goalGameObject = goalObject;
            this.findAngle = findAngle;
            this.findTime = findTime;
        }
        public override void OnEnter()
        {
            base.OnEnter();
            gameObject.GetComponent<NavMeshAgent>().ResetPath();
            hasFoundTime = 0;
        }
        public override void OnHold()
        {
            hasFoundTime += Time.deltaTime;
            
        }
        public override bool CanChange()
        {
            if(hasFoundTime > findTime)
            {
                changeString = "Idle";
                return true;
            }
            return false;
        }
    }
    public class Dead : FSMState
    {
        public Dead(GameObject gameObject)
            : base(gameObject)
        {
            stateName = "Dead";
        }

    }

}
