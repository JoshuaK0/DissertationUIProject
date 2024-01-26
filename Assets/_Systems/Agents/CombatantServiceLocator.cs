using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class CombatantServiceLocator : MonoBehaviour
{
	[SerializeField] CombatantFSM combatantFSM;
	[SerializeField] CombatantID combatantID;
	[SerializeField] SquadTargetManager squadTargetManager;
	[SerializeField] SquadManager squadManager;
	[SerializeField] HealthManager healthManager;
	[SerializeField] AwarenessManager awarenessManager;
	[SerializeField] NavMeshAgent navMeshAgent;
	[SerializeField] SuspicionTargetManager suspicionTargetSensor;
	[SerializeField] CombatantEnemyVisualSensor visualSensor;
	[SerializeField] CombatantHitpoints hitpoints;
	[SerializeField] Collider visualCollider;

	public Collider GetVisualCollider()
	{
		return visualCollider;
	}


	public CombatantHitpoints GetHitpoints()
	{
		return hitpoints;
	}

	public CombatantEnemyVisualSensor GetVisualSensor()
	{
		return visualSensor;
	}

	public SuspicionTargetManager GetSuspicionManager()
	{
		return suspicionTargetSensor;
	}

	public CombatantID GetCombatantID()
	{
		return combatantID;
	}

	public HealthManager GetHealthManager()
	{
		return healthManager;
	}

	public AwarenessManager GetAwarenessManager()
	{
		return awarenessManager;
	}
	public SquadTargetManager GetSquadTargetManager()
	{
		return squadTargetManager;
	}
	public CombatantFSM CombatantFSM()
	{
		return combatantFSM;
	}

	public NavMeshAgent GetNavMeshAgent()
	{
		return navMeshAgent;
	}
	public SquadManager GetSquadManager()
	{
		return squadManager;
	}

	public CombatantFSM GetCombatantFSM()
	{
		return combatantFSM;
	}
}
