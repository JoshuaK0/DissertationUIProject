using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetIsVisibleDecision : FSMDecision
{
	CombatantFSM combatantFSM;
	public override void InitDecision()
	{
		combatantFSM = fsm.GetComponent<CombatantFSM>();
	}

	public override bool DecisionEvaluate()
	{
		if(combatantFSM.GetTarget() == null)
		{
			return false;
		}
		return combatantFSM.GetTarget().isVisible();
	}
}
