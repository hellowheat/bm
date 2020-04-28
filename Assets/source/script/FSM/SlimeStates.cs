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

        public Patrol(GameObject gameObject, TrackMap trackMap)
            : base(gameObject)
        {
            agent = gameObject.GetComponent<NavMeshAgent>();
            stateName = "Patrol";
            this.trackMap = trackMap;
            posIndex = 0;
            gameObject.transform.position = trackMap.getPosition(posIndex);
        }
        public override void OnEnter()
        {
            List<int> nearList = trackMap.getNear(posIndex);
            posIndex = nearList[Random.Range(0, nearList.Count)];
            agent.SetDestination(trackMap.getPosition(posIndex));
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
        float seeRadius;
        float seeAngle;
        float accidentsTime = 0.4f;
        float stayTime;
        public Accidents(GameObject gameObject, GameObject goalGameObject,float seeRadius, float seeAngle)
            : base(gameObject)
        {
            stateName = "Accidents";
            this.goalGameObject = goalGameObject;
            this.seeRadius = seeRadius;
            this.seeAngle = seeAngle;
        }

        public override void OnEnter()
        {
            base.OnEnter();
            stayTime = 0;
        }
        public override void OnHold()
        {
            stayTime += Time.deltaTime;
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
        public override bool CanEnter()
        {
            return staticFunction.canSeeObject(gameObject, goalGameObject, seeRadius, seeAngle);
        }

        
    }
    public class Follow : FSMState
    {
        NavMeshAgent agent;
        GameObject goalGameObject;
        Vector3 goalPosition;
        float seeRadius;
        float seeAngle;
        float flashGoalPositonDis;//隔一定时间更新一下目标位置
        float flashGoalPositonTime;
        float allowNoGoalTime;
        public Follow(GameObject gameObject, GameObject goalGameObject, float seeRadius, float seeAngle)
            : base(gameObject)
        {
            stateName = "Follow";
            flashGoalPositonDis = 0.5f;
            allowNoGoalTime = 1f;
            this.seeRadius = seeRadius;
            this.seeAngle = seeAngle;
            this.goalGameObject = goalGameObject; 
            agent = gameObject.GetComponent<NavMeshAgent>();
        }
        public override void OnEnter()
        {
            base.OnEnter();
            flashGoalPositonTime = 0;
            goalPosition = goalGameObject.transform.position;
            agent.SetDestination(goalPosition);
        }
        public override void OnHold()
        {
            flashGoalPositonTime += Time.deltaTime;
            if (flashGoalPositonTime > flashGoalPositonDis && staticFunction.canSeeObject(gameObject, goalGameObject, seeRadius, seeAngle))
            {
                flashGoalPositonTime = 0;
                goalPosition = goalGameObject.transform.position;
                agent.SetDestination(goalPosition);
            }
            if (!agent.hasPath)
            {

            }
        }

        public override bool CanChange()
        {
            if(flashGoalPositonTime > allowNoGoalTime)
            {
                changeString = "Idle";
                return true;
            }
            return false;
        }
    }
    public class Atk : FSMState
    {
        public Atk(GameObject gameObject)
            : base(gameObject)
        {
            stateName = "Atk";
        }

    }
    public class Find : FSMState
    {
        public Find(GameObject gameObject)
            : base(gameObject)
        {
            stateName = "Find";
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
