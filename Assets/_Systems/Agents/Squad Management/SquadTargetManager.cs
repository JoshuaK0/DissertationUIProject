using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class SquadTargetManager : MonoBehaviour
{
	[SerializeField] List<SquadTarget> squadTargets = new List<SquadTarget>();
	[SerializeField] float predictionTime;
	[SerializeField] SquadPositionManager positionManager;
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

		positionManager.AddTarget(newSquadTarget);

	}

	public void AddUnseenTarget(CombatantID newCombatant)
	{
		foreach (SquadTarget target in squadTargets)
		{
			if (target.combatantID == newCombatant)
			{
				return;
			}
		}
		GameObject newTarget = new GameObject();
		newTarget.name = newCombatant.transform.name;
		newTarget.transform.SetParent(transform);
		SquadTarget newSquadTarget = newTarget.AddComponent<SquadTarget>();
		newSquadTarget.spottedCount = 0;
		newSquadTarget.combatantID = newCombatant;
		squadTargets.Add(newSquadTarget);

		positionManager.AddTarget(newSquadTarget);
	}

	public bool HasTarget(CombatantID combatant)
	{
		foreach (SquadTarget target in squadTargets)
		{
			if (target.combatantID == combatant)
			{
				return true;
			}
		}
		return false;
	}


	public void RemoveTarget(CombatantID newCombatant)
	{
		foreach (SquadTarget target in squadTargets)
		{
			if (target.combatantID == newCombatant)
			{
				target.spottedCount--;
				if(target.spottedCount <= 0 )
				{
					//positionManager.RemoveTarget(target);
					target.spottedCount = 0;
					target.leftVisibility = true;
					target.Invoke("EndUpdateLKP", predictionTime);
				}
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

	public List<SquadTarget> GetSquadTargets()
	{
		return squadTargets;
	}

	void Update()
	{
		foreach (SquadTarget target in  squadTargets)
		{
			if (target.spottedCount > 0)
			{
				target.UpdateLKP();
			}
		}
	}
}
