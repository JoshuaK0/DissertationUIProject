using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class CombatantFSM : FiniteStateMachine
{
	
	[Header("References")]
	[SerializeField] SquadTargetManager squadTargetManager;
	[SerializeField] NavMeshAgent agent;

	[Header("Target Finding")]
	[SerializeField] SquadTarget currentTarget;

	[Header("Guard")]
	[SerializeField] Vector3 guardingPos;

	float timeSinceLastSawTarget;
	float lastSawTargetTime;

	void Awake()
	{
		currentTarget = null;
	}
	public void SetTarget(SquadTarget newTarget)
	{
		currentTarget = newTarget;
	}

	public SquadTarget GetTarget()
	{
		return currentTarget;
	}
	public SquadTargetManager GetSquadTargetManager()
	{
		return squadTargetManager;
	}

	public void SetNavDestination(Vector3 destination)
	{
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
}
