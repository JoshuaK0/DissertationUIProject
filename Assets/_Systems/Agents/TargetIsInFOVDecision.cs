using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetIsInFOVDecision : FSMDecision
{
	CombatantEnemyVisualSensor visualSensor;

	CombatantFSM combatantFSM;

	public override void InitDecision()
	{
		combatantFSM = fsm.GetComponent<CombatantFSM>();
		visualSensor = combatantFSM.GetCombatantServices().GetVisualSensor();
	}

	public override bool DecisionEvaluate()
	{
		List<CombatantID> list = new List<CombatantID>();
		list = visualSensor.GetAllTargetsInFOV();
		if (list == null || list.Count == 0)
		{
			return false;
		}
		SquadTarget target = combatantFSM.GetTarget();
		if (target == null)
		{
			return false;
		}
		return list.Contains(target.combatantID);
	}
}
