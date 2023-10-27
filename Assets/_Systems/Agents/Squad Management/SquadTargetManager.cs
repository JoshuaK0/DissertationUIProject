using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SquadTargetManager : MonoBehaviour
{
	[SerializeField] List<SquadTarget> squadTargets = new List<SquadTarget>();
	public void AddTarget(CombatantID newCombatant)
	{
		foreach (SquadTarget target in squadTargets)
		{
			if (target.combatantID == newCombatant)
			{
				target.spottedCount++;
				return;
			}
		}
		GameObject newTarget = new GameObject();
		newTarget.name = newCombatant.transform.name;
		newTarget.transform.SetParent(transform);
		SquadTarget newSquadTarget = newTarget.AddComponent<SquadTarget>();
		newSquadTarget.spottedCount = 1;
		newSquadTarget.combatantID = newCombatant;
		squadTargets.Add(newSquadTarget);
	}

	public void RemoveTarget(CombatantID newCombatant)
	{
		foreach (SquadTarget target in squadTargets)
		{
			if (target.combatantID == newCombatant)
			{
				target.spottedCount--;
				return;
			}
		}
	}

	public List<SquadTarget> GetSeenTargets()
	{
		List<SquadTarget> seenTargets = new List<SquadTarget>();
		foreach (SquadTarget target in squadTargets)
		{
			if (target.spottedCount > 0)
			{
				seenTargets.Add(target);
			}
		}
		return seenTargets;
	}

	public List<SquadTarget> GetLastKnownPositionTargets()
	{
		List<SquadTarget> unseenTargets = new List<SquadTarget>();
		foreach (SquadTarget target in squadTargets)
		{
			if (target.spottedCount <= 0)
			{
				unseenTargets.Add(target);
			}
		}
		return unseenTargets;
	}
}
