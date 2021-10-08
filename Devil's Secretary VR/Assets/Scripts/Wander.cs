using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Wander : States
{
   

    public Wander(GameObject _npc, UnityEngine.AI.NavMeshAgent _agent, Animator _anim, Transform _player) : base(_npc, _agent, _anim, _player)
    {
        name = STATE.WANDER;
        agent.speed = 5;
    }
    public override void Enter()
    {
        //anim.SetTrigger("isWandering");
      
        base.Enter();
       


    }
    public override void Update()
    {
        
        timer += Time.deltaTime;
        Debug.Log("I am wandering!");
        if(timer >= wanderTimer)
        {
            Vector3 newPos = WanderAround(npc.transform.position, wanderRadius, -1);
            agent.SetDestination(newPos);
            Debug.Log("Time to wander elsewhere!");
            timer = 0;

            
        }
        else if( timer < wanderTimer)
        {
            Debug.Log("Still Wandering!");
            //Debug.Log(timer);
            base.Update();
        }
        if (CanSeePlayer())
        {

            nextState = new Flee(npc, agent, anim, player);
            stage = EVENT.EXIT;
        }
        if (IsPlayerBehind())
        {
            nextState = new Flee(npc, agent, anim, player);
            stage = EVENT.EXIT;
        }




    }

    public override void Exit()
    {
        //anim.ResetTrigger("isWandering");
        base.Exit();
    }
    






}
