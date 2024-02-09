using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IsClearingDecision : FSMDecision
{
	[SerializeField] MoveToSuspicionBehaviour moveToSuspicionBehaviour;
	public override bool DecisionEvaluate()
	{
		return moveToSuspicionBehaviour.IsClearing();
	}
}
