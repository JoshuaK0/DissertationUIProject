using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatantID : MonoBehaviour
{
	[SerializeField] int teamIndex;
	[SerializeField] CombatantServiceLocator combatantServices;

	public CombatantServiceLocator GetCombatantServices()
	{
		return combatantServices;
	}
	public int GetTeamIndex()
	{
		return teamIndex;
	}
}
