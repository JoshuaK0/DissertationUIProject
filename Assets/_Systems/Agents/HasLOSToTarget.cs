using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HasLOSToTarget : FSMDecision
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
		return visualSensor.IsCombatantVisible(combatantFSM.GetTarget().combatantID);
	}
}
