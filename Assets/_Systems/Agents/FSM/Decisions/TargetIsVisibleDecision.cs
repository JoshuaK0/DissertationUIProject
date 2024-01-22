using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetIsVisibleDecision : FSMDecision
{
	CombatantFSM combatantFSM;

	[SerializeField] bool hasWaitTime;
	[SerializeField] float waitTime;

	float currentTime;
	public override void InitDecision()
	{
		combatantFSM = fsm.GetComponent<CombatantFSM>();
	}

	public override bool DecisionEvaluate()
	{
		if(hasWaitTime)
		{
			if (invert)
			{
				if(!combatantFSM.GetTarget().isVisible())
				{
					currentTime += Time.deltaTime;
					if(currentTime >= waitTime)
					{
						return false;
					}
				}
				currentTime = 0;
				return true;
			}
			else
			{
				if (combatantFSM.GetTarget().isVisible())
				{
					currentTime += Time.deltaTime;
					if (currentTime >= waitTime)
					{
						return true;
					}
				}
				currentTime = 0;
				return false;
			}
		}
		else
		{
			if (combatantFSM.GetTarget() == null)
			{
				return false;
			}
			return combatantFSM.GetTarget().isVisible();
		}
	}
}
