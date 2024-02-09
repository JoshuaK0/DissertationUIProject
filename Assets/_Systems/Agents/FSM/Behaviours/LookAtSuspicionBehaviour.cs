using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.InputSystem;

public class LookAtSuspicionBehaviour : FSMBehaviour
{
	SuspicionTarget closestTarget;
	CombatantFSM combatantFSM;
	NavMeshAgent agent;
	public override void EnterBehaviour()
	{
		closestTarget = null;
		combatantFSM = fsm.GetComponent<CombatantFSM>();
		agent = combatantFSM.GetCombatantServices().GetNavMeshAgent();
	}

	// Update is called once per frame
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
		combatantFSM.SetSuspicionTarget(closestTarget);

		SuspicionTarget currentSuspicionTarget = combatantFSM.GetSuspicionTarget();
		if (currentSuspicionTarget != null)
		{
			agent.updateRotation = false;
			var lookPos = currentSuspicionTarget.GetCurrentLocation() - combatantFSM.transform.position;
			lookPos.y = 0;
			if (lookPos != Vector3.zero)
			{
				var rotation = Quaternion.LookRotation(lookPos);
				combatantFSM.transform.rotation = Quaternion.Slerp(combatantFSM.transform.rotation, rotation, Time.deltaTime * 5);
			}
		}
	}
}
