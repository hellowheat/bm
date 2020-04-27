using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace moster_slime
{
    public class Idle :FSMState
    {
        Animator animator;
        public Idle(GameObject gameObject)
            :base(gameObject)
        {
            animator = gameObject.GetComponent<Animator>();
        }
        override public void OnEnter()
        {
            animator.SetTrigger("Idle");
        }
        override public void OnHold() {  }
        override public void OnExit() { }
    }
    public class Patrol : FSMState
    {
        Animator animator;
        public Patrol(GameObject gameObject)
            : base(gameObject)
        {
            animator = gameObject.GetComponent<Animator>();
        }
        public override void OnEnter()
        {
            Debug.Log("part");
            animator.SetTrigger("Patrol");
        }
    }
    public class Accidents : FSMState
    {
        public Accidents(GameObject gameObject)
            : base(gameObject) { }

    }
    public class Follow : FSMState
    {
        public Follow(GameObject gameObject)
            : base(gameObject) { }

    }
    public class Atk : FSMState
    {
        public Atk(GameObject gameObject)
            : base(gameObject) { }

    }
    public class Find : FSMState
    {
        public Find(GameObject gameObject)
            : base(gameObject) { }

    }
    public class Dead : FSMState
    {
        public Dead(GameObject gameObject)
            : base(gameObject) { }

    }

}
