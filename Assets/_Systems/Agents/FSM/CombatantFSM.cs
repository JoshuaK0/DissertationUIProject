using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class CombatantFSM : FiniteStateMachine, IKillable
{
	
	[Header("References")]
	[SerializeField] CombatantServiceLocator combatantServices;

	[Header("Target Finding")]
	[SerializeField] SquadTarget currentTarget;
	[SerializeField] float awarenessThreshold;
	[SerializeField] float minAwarenessAfterDetection;
	[SerializeField] float awarenessAfterDetection;

	[Header("Guard")]
	[SerializeField] Vector3 guardingPos;

	[Header("Death")]
	[SerializeField] FSMState deathState;
	[SerializeField] bool showDebugLogs;

	SquadTargetManager squadTargetManager;
	SquadManager squadManager;
	NavMeshAgent agent;
	HealthManager health;
	CombatantID combatantID;
	AwarenessManager awarenessManager;

	float timeSinceLastSawTarget;
	float lastSawTargetTime;

	bool isAlive = true;

	void Awake()
	{
		currentTarget = null;
	}

	public override void Start()
	{
		
		health = combatantServices.GetHealthManager();
		agent = combatantServices.GetNavMeshAgent();
		squadManager = combatantServices.GetSquadManager();
		combatantID = combatantServices.GetCombatantID();
		awarenessManager = combatantServices.GetAwarenessManager();
		awarenessManager.OnAwarenessGainedLocally += OnAwarenessChange;

		base.Start();
	}

	public void OnAwarenessChange()
	{
		if (awarenessManager.GetCurrentAwareness() >= awarenessThreshold)
		{
			squadManager.SetTeamMinAwareness(minAwarenessAfterDetection);
			squadManager.SetSquadCurentAwareness(awarenessAfterDetection);
		}
	}

	public CombatantServiceLocator GetCombatantServices()
	{
		return combatantServices;
	}
	public void SetTarget(SquadTarget newTarget)
	{
		currentTarget = newTarget;
	}

	public SquadTarget GetTarget()
	{
		return currentTarget;
	}

	public void SetNavDestination(Vector3 destination)
	{
		if(showDebugLogs)
		{
			Debug.Log(currentState.name + " setting destination to " + destination);
		}
		agent.SetDestination(destination);
	}

	public override void Update()
	{
		base.Update();
		if(currentTarget != null && currentTarget.spottedCount > 0)
		{
			lastSawTargetTime = Time.time;
		}
	}

	public NavMeshAgent GetNavMeshAgent()
	{
		return agent;
	}

	public float GetAwarenessThreshold()
	{
		return awarenessThreshold;
	}

	public void AgentUpdateRotation(bool state)
	{
		agent.updateRotation = state;
	}

	public Vector3 GetTargetLKP()
	{
		if(currentTarget != null)
		{
			return currentTarget.lastSpottedPosition;
		}
		else
		{
			return transform.position;
		}
	}

	public float GetTimeSinceLastSawTarget()
	{
		return Time.time - lastSawTargetTime;
	}
	public void Kill()
	{
		isAlive = false;
		squadManager.SquadMemberKilled(combatantID);
		foreach (CombatantID target in combatantServices.GetVisualSensor().GetLocallyVisibleTargets())
		{
			combatantServices.GetSquadTargetManager().RemoveTarget(target);
		}
		ChangeState(deathState);
	}

	public bool IsAlive()
	{
		return isAlive;
	}

	public AwarenessManager GetAwarenessManager()
	{
		return awarenessManager;
	}
}
