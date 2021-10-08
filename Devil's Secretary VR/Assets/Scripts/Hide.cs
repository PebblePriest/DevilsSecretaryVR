using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hide : States
{
    public Hide(GameObject _npc, UnityEngine.AI.NavMeshAgent _agent, Animator _anim, Transform _player) : base(_npc, _agent, _anim, _player)
    {
        name = STATE.HIDE;
    }
}
