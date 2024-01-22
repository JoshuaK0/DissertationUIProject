using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ChangeNavmeshPriorityBehaviour : FSMBehaviour
{
    [SerializeField] int priorityChange;

    NavMeshAgent agent;

    public override void ExitBehaviour()
    {
		agent.avoidancePriority -= priorityChange;
	}

    public override void EnterBehaviour()
    {
        agent = fsm.GetComponent<NavMeshAgent>();
        agent.avoidancePriority += priorityChange;
    }
}
