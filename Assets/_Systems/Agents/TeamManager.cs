using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeamManager : MonoBehaviour
{
	[SerializeField] UnitManager unitManager;
	[SerializeField] List<SquadManager> squads = new List<SquadManager>();
	[SerializeField] int teamIndex;

	float currentTeamMinAwareness = 0;
	public void SquadKilled(SquadManager deadSquad)
	{
		if (squads.Contains(deadSquad))
		{
			squads.Remove(deadSquad);
			if (squads.Count <= 0)
			{
				unitManager.TeamKilled(this);
			}
		}
	}

	public int GetTeamIndex()
	{
		return teamIndex;
	}

	public void SetTeamMinAwareness(float newTeamMinAwareness)
	{
		if(newTeamMinAwareness < currentTeamMinAwareness)
		{
			return;
		}
		else
		{
			currentTeamMinAwareness = newTeamMinAwareness;
		}
		foreach(SquadManager squad in squads)
		{
			squad.SetSquadMinAwareness(newTeamMinAwareness);
		}
	}

	public void SetTeamCurrentAwareness(float newTeamCurrentAwareness)
	{
		foreach (SquadManager squad in squads)
		{
			squad.SetSquadCurentAwareness(newTeamCurrentAwareness);
		}
	}

	public float GetCurrentMinAwareness()
	{
		return currentTeamMinAwareness;
	}
}
