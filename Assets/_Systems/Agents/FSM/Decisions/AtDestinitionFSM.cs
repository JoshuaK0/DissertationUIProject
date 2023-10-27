using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AtDestinitionFSM : FSMDecision
{
	[SerializeField] float arrivalDistance = 0.5f;
	CombatantFSM combatantFSM;

	public override void InitDecision()
	{
		combatantFSM = fsm.GetComponent<CombatantFSM>();
	}

	public override bool DecisionEvaluate()
	{
		NavMeshAgent agent = combatantFSM.GetNavMeshAgent();
		return (agent.remainingDistance < arrivalDistance);
	}
}
