using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SquadManager : MonoBehaviour
{
	[SerializeField] TeamManager teamManager;
	[SerializeField] List<CombatantID> squadMembers;
	[SerializeField] SquadPositionManager positionManager;

	public SquadPositionManager GetSquadPositionManager()
	{
		return positionManager;
	}

	float currentSquadMinAwareness = 0;

	public void SquadMemberKilled(CombatantID deadSquadMember)
	{
		if(squadMembers.Contains(deadSquadMember))
		{
			squadMembers.Remove(deadSquadMember);
			if(squadMembers.Count <= 0)
			{
				teamManager.SquadKilled(this);
			}
		}
	}


	public void Start()
	{
		for(int i = 0; i < squadMembers.Count; i++)
		{
			if(squadMembers[i].GetCombatantServices() != null)
			{
				if (squadMembers[i].GetCombatantServices().GetNavMeshAgent() != null)
				{
					squadMembers[i].GetCombatantServices().GetNavMeshAgent().avoidancePriority = i;
				}
			}
		}
	}
	public void SetTeamMinAwareness(float newMinAwareness)
	{
		teamManager.SetTeamMinAwareness(newMinAwareness);
	}

	public void SetSquadMinAwareness(float newMinAwarenesss)
	{
		if (newMinAwarenesss < currentSquadMinAwareness)
		{
			return;
		}
		else
		{
			currentSquadMinAwareness = newMinAwarenesss;
			foreach (CombatantID combatantID in squadMembers)
			{
				combatantID.GetCombatantServices().GetAwarenessManager().SetMinAwareness(newMinAwarenesss);
			}
		}
	}

	public TeamManager GetTeamManager()
	{
		return teamManager;
	}

	public void SetTeamCurentAwareness(float newAwareness)
	{
		teamManager.SetTeamCurrentAwareness(newAwareness);
	}

	public void SetSquadCurentAwareness(float newAwareness)
	{
		foreach (CombatantID combatantID in squadMembers)
		{
			combatantID.GetCombatantServices().GetAwarenessManager().SetMinAwareness(newAwareness);
		}
	}
}
