using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AwarenessCheckDecision : FSMDecision
{
	CombatantFSM combatantFSM;
	[SerializeField] float awarenessValue;
	[SerializeField] EvaluationType evalType;

	enum EvaluationType
	{
		LessThan,
		GreaterThan,
		EqualTo
	}

	public override bool DecisionEvaluate()
	{
		if (evalType == EvaluationType.LessThan)
		{
			return combatantFSM.GetCombatantServices().GetAwarenessManager().GetCurrentAwareness() < awarenessValue;
		}
		else if (evalType == EvaluationType.GreaterThan)
		{
			return combatantFSM.GetCombatantServices().GetAwarenessManager().GetCurrentAwareness() > awarenessValue;
		}
		else if (evalType == EvaluationType.EqualTo)
		{
			return combatantFSM.GetCombatantServices().GetAwarenessManager().GetCurrentAwareness() == awarenessValue;
		}
		return false;
	}

	public override void InitDecision()
	{
		combatantFSM = fsm.GetComponent<CombatantFSM>();
	}
}
