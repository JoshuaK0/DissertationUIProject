using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitManager : MonoBehaviour
{
	[SerializeField] ObjectiveManager objectiveManager;

	[SerializeField] List<TeamManager> teams = new List<TeamManager>();

	public delegate void UnitsUpdated();
	public event UnitsUpdated OnUnitsUpdated;
	
	public void TeamKilled(TeamManager deadSquad)
	{
		if(teams.Contains(deadSquad))
		{
			teams.Remove(deadSquad);
			OnUnitsUpdated?.Invoke();
		}
	}

	public bool IsOtherTeamsDead(int ownTeam)
	{
		foreach (TeamManager squad in teams)
		{
			if (squad.GetTeamIndex() != ownTeam)
			{
				return false;
			}
		}
		return true;
	}
}
