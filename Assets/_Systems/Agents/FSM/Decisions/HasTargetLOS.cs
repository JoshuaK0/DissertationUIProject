using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HasTargetLOS : FSMDecision
{
	CombatantFSM combatantFSM;
	[SerializeField] float range;
	[SerializeField] LayerMask losLayers;
	public override void InitDecision()
	{
		combatantFSM = fsm.GetComponent<CombatantFSM>();
	}

	public override bool DecisionEvaluate()
	{
		Vector3 origin = combatantFSM.transform.position;
		Vector3 target = combatantFSM.GetTargetLKP();
		Vector3 dir = origin - target;
		return Physics.Raycast(origin, dir, range, losLayers);
	}
}
