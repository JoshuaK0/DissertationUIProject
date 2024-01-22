using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class FindTargetBehaviour : FSMBehaviour
{
	[SerializeField] float searchInterval = 0.5f;
	CombatantFSM combatantFSM;
	SquadTargetManager squadTargetManager;
	AwarenessManager awarenessManager;
	public override void EnterBehaviour()
	{
		combatantFSM = fsm.GetComponent<CombatantFSM>();
		squadTargetManager = combatantFSM.GetCombatantServices().GetSquadTargetManager();
		awarenessManager = combatantFSM.GetCombatantServices().GetAwarenessManager();
		FindTargets();
	}
	public override void UpdateBehaviour()
	{
		InvokeRepeating("FindTargets", 0, searchInterval);
	}

	void FindTargets()
	{
		List<SquadTarget> seenTargets = squadTargetManager.GetSeenTargets();
		List<SquadTarget> lastKnownLocations = squadTargetManager.GetLastKnownPositionTargets();
		if (seenTargets.Count > 0 )
		{
			float closestDist = Mathf.Infinity;
			foreach(SquadTarget target in seenTargets)
			{
				float currentDist = Vector3.Distance(fsm.transform.position, target.combatantID.transform.position);
				if (currentDist < closestDist)
				{
					closestDist = currentDist;
					combatantFSM.SetTarget(target);
				}
			}
		}
		else if (lastKnownLocations.Count > 0)
		{
			
			foreach(SquadTarget target in lastKnownLocations )
			{
				float closestDist = Mathf.Infinity;
				float currentDist = Vector3.Distance(fsm.transform.position, target.combatantID.transform.position);
				if (currentDist < closestDist)
				{
					closestDist = currentDist;
					combatantFSM.SetTarget(target);
				}
			}
		}
		else
		{
			combatantFSM.SetTarget(null);
		}
	}

	public override void ExitBehaviour()
	{
		CancelInvoke();
	}
}
