using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class SlimeStateManager : AvatarStateManager
{
    NavMeshAgent agent;
    Animator animator;
    SlimeState currentState;
    enum SlimeState
    {
        Idle,Patrol,Accidents,Follow,Atk,Find,Dead
        //站立，巡逻，警惕，追击，攻击，寻找，死亡
    }
    
    protected override void init()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        currentState = SlimeState.Idle;
        stateMachine.Add("Idle", new moster_slime.Idle(gameObject));
        stateMachine.Add("Patrol", new moster_slime.Patrol(gameObject));
        stateMachine.Add("Sccidents", new moster_slime.Accidents(gameObject));
        stateMachine.Add("Follow", new moster_slime.Follow(gameObject));
        stateMachine.Add("Atk", new moster_slime.Atk(gameObject));
        stateMachine.Add("Find", new moster_slime.Find(gameObject));
        stateMachine.Add("Dead", new moster_slime.Dead(gameObject));
    }
    
    override protected void updateState() {
        Debug.Log(agent.hasPath);
        if(currentState==SlimeState.Patrol && !agent.hasPath)
        {
            stateMachine.TranslateState("Idle");
            currentState = SlimeState.Idle;
        }

        if (Input.GetMouseButtonDown(0))
        {
            Debug.Log(111111);
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            Physics.Raycast(ray, out hit);
            if (hit.collider != null)
            {
                agent.SetDestination(hit.point);
                currentState = SlimeState.Patrol;
                stateMachine.TranslateState("Patrol");
            }
        }
    }

}
