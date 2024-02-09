using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MoveToSuspicionBehaviour : FSMBehaviour
{
	SuspicionTarget suspicion;

	CombatantFSM combatantFSM;

	SuspicionTarget closestTarget;

	NavMeshAgent agent;
	CombatantEnemyVisualSensor sensor;

	[SerializeField] float range;

	bool isClearing;

	bool gotLOS;

	Vector3 LOSCheck;
	public override void EnterBehaviour()
	{
		gotLOS = false;
		isClearing = false;
		closestTarget = null;
		combatantFSM = fsm.GetComponent<CombatantFSM>();
		agent = combatantFSM.GetCombatantServices().GetNavMeshAgent();
		sensor = combatantFSM.GetCombatantServices().GetVisualSensor();

	}

	public override void UpdateBehaviour()
	{
		List<SuspicionTarget> targets = combatantFSM.GetCombatantServices().GetSuspicionManager().GetSuspicionTargets();

		if (targets.Count > 0)
		{
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
		}
		SuspicionTarget currentSuspicionTarget = combatantFSM.GetSuspicionTarget();
		if(currentSuspicionTarget != null)
		{
			if (Vector3.Distance(currentSuspicionTarget.GetCurrentLocation(), fsm.transform.position) <= range && (sensor.IsCombatantVisible(currentSuspicionTarget.GetCombatantID())))
			{
				Debug.Log(sensor.IsCombatantVisible(currentSuspicionTarget.GetCombatantID()));
				isClearing = true;
				agent.updateRotation = false;
				var lookPos = currentSuspicionTarget.GetCurrentLocation() - combatantFSM.transform.position;
				lookPos.y = 0;
				if (lookPos != Vector3.zero)
				{
					var rotation = Quaternion.LookRotation(lookPos);
					combatantFSM.transform.rotation = Quaternion.Slerp(combatantFSM.transform.rotation, rotation, Time.deltaTime * 25);
				}
				if(!gotLOS)
				{
					combatantFSM.SetNavDestination(combatantFSM.transform.position);
					gotLOS = true;
				}
			}
			else
			{
				combatantFSM.SetNavDestination(currentSuspicionTarget.GetCurrentLocation());
				isClearing = false;
				agent.updateRotation = true;
			}
		}
	}

	public bool IsClearing()
	{
		return isClearing;
	}
	public override void ExitBehaviour()
	{
		agent.updateRotation = true;
	}
}
