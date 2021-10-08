using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Flee: States
{
    public Flee(GameObject _npc, NavMeshAgent _agent, Animator _anim, Transform _player) : base(_npc, _agent, _anim, _player)
    {
        name = STATE.FLEE;
        agent.speed = 12;
    }
    public override void Enter()
    {
        //anim.SetTrigger("isIdle");
        base.Enter();

    }
    public override void Update()
    {
        RunAway();
        if(IsPlayerBehind())
        {
            Debug.Log("Player is still behind!");
            base.Update();
        }
        else if (!IsPlayerBehind())
        {
            nextState = new Idle(npc, agent, anim, player);
            stage = EVENT.EXIT;
        }
       




    }

    public override void Exit()
    {
        //anim.ResetTrigger("isIdle");
        base.Exit();
    }
}
