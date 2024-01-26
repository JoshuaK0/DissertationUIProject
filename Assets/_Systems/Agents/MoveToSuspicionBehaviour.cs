using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MoveToSuspicionBehaviour : FSMBehaviour
{
	SuspicionTarget suspicion;

	CombatantFSM combatantFSM;

	SuspicionTarget closestTarget;
	SuspicionTarget previousTarget;

	NavMeshAgent agent;
	CombatantEnemyVisualSensor sensor;
	public override void EnterBehaviour()
	{
		closestTarget = null;
		previousTarget = null;
		combatantFSM = fsm.GetComponent<CombatantFSM>();
		agent = combatantFSM.GetCombatantServices().GetNavMeshAgent();
		sensor = combatantFSM.GetCombatantServices().GetVisualSensor();

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

		if(sensor.PointHasLOS(closestTarget.GetCurrentLocation()))
		{
			agent.updateRotation = false;
			var lookPos = closestTarget.GetCurrentLocation() - combatantFSM.transform.position;
			lookPos.y = 0;
			if (lookPos != Vector3.zero)
			{
				var rotation = Quaternion.LookRotation(lookPos);
				combatantFSM.transform.rotation = Quaternion.Slerp(combatantFSM.transform.rotation, rotation, Time.deltaTime * 5);
			}
		}
		else
		{
			agent.updateRotation = true;
		}
	}

	public override void ExitBehaviour()
	{
		agent.updateRotation = true;
	}
}
