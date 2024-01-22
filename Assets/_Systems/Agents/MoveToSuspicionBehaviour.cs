using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveToSuspicionBehaviour : FSMBehaviour
{
	SuspicionTarget suspicion;

	CombatantFSM combatantFSM;

	SuspicionTarget closestTarget;
	SuspicionTarget previousTarget;
	public override void EnterBehaviour()
	{
		closestTarget = null;
		previousTarget = null;
		combatantFSM = fsm.GetComponent<CombatantFSM>();
		
	}

	public override void UpdateBehaviour()
	{
		List<SuspicionTarget> targets = combatantFSM.GetCombatantServices().GetSuspicionManager().GetSuspicionTargets();

		if (targets.Count <= 0)
		{
			return;
		}

		float closestDist = Mathf.Infinity;
		closestTarget = targets[0];
		foreach (SuspicionTarget target in targets)
		{
			float currentDist = Vector3.Distance(transform.position, target.GetCurrentLocation());
			if (currentDist < closestDist)
			{
				closestDist = currentDist;
				closestTarget = target;
			}
		}
		
		if (closestTarget != previousTarget)
		{
			previousTarget = closestTarget;
			combatantFSM.SetNavDestination(closestTarget.GetCurrentLocation());
		}
	}
}
