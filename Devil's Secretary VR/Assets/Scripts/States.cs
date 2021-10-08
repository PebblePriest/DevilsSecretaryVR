using System.Collections;
using System.Collections.Generic;
using UnityEngine.AI;
using UnityEngine;

public class States  
{
   public enum STATE
    {
        IDLE, WANDER, FLEE, HIDE
    }
    public enum EVENT
    {
        ENTER, UPDATE, EXIT
    }

    public STATE name;
    protected EVENT stage;
    protected GameObject npc;
    protected Animator anim;
    protected Transform player;
    protected States nextState;
    protected NavMeshAgent agent;
    float visDist = 10.0f;
    float visAngle = 30.0f;
    float EnemyDistRun = 10.0f;
    public float wanderTimer = 4;
    public float wanderRadius = 50;
    public float timer;
    public float waitTillNextWander = 20;
    public bool CanSeePlayer()
    {
        Vector3 direction = player.position - npc.transform.position;
        float angle = Vector3.Angle(direction, npc.transform.forward);

        if (direction.magnitude < visDist && angle < visAngle)
        {
            Debug.Log("He is over there!");
            return true;
        }
        Debug.Log("I can't see him");
        return false;
    }
    public bool IsPlayerBehind()
    {
        Vector3 direction = npc.transform.position - player.position;
        float angle = Vector3.Angle(direction, npc.transform.forward);
        if (direction.magnitude < 10 && angle < 30)
        {
            Debug.Log("There is someone behind me!");
            return true;
        }
        Debug.Log("Nobody is behind me");
        return false;
    }
    public bool RunAway()
    {
        float distance = Vector3.Distance(npc.transform.position, player.transform.position);
        if(distance < EnemyDistRun)
        {
            Debug.Log("Run Away!");
            Vector3 dirToPlayer = npc.transform.position - player.transform.position;
            Vector3 newPOs = 2 *(npc.transform.position + dirToPlayer);
            agent.SetDestination(newPOs);
            return true;
        }
        Debug.Log("Nobody There");
        return false;
        
    }
    public static Vector3 WanderAround(Vector3 origin, float dist, int layermask)
    {
        Vector3 randDirection = Random.insideUnitSphere * dist;

        randDirection += origin;

        NavMeshHit navHit;

        NavMesh.SamplePosition(randDirection, out navHit, dist, layermask);

        return navHit.position;
    }
    public void TimerStart()
    {
        timer = wanderTimer;

    }

    public States(GameObject _npc, NavMeshAgent _agent, Animator _anim, Transform _player)
    {
        npc = _npc;
        agent = _agent;
        anim = _anim;
        stage = EVENT.ENTER;
        player = _player;
    }
    public virtual void Enter() { stage = EVENT.UPDATE; }
    public virtual void Update() { stage = EVENT.UPDATE; }
    public virtual void Exit() { stage = EVENT.EXIT; }
    public States Process()
    {
        if (stage == EVENT.ENTER) Enter();
        if (stage == EVENT.UPDATE) Update();
        if (stage == EVENT.EXIT)
        {
            Exit();
            return nextState;
        }
        return this;
    }
  
  
}

