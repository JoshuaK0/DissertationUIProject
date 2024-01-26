using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UIElements;
using static UnityEngine.GraphicsBuffer;

public class SuspicionTargetManager : MonoBehaviour, IKillable
{
	[SerializeField] CombatantServiceLocator combatantServiceLocator;
	AwarenessManager awarenessManager;

	SquadTargetManager squadTargetManager;
	CombatantEnemyVisualSensor visualSensor;

	bool isDisabled = false;

	void Start()
	{
		awarenessManager = combatantServiceLocator.GetAwarenessManager();
		squadTargetManager = combatantServiceLocator.GetSquadTargetManager();
		visualSensor = combatantServiceLocator.GetVisualSensor();
	}
	[SerializeField] List<SuspicionTarget> suspicionTargets = new List<SuspicionTarget>();

	public void Kill()
	{
		isDisabled = true;
	}

	public void AddSuspicionTarget(SuspicionTarget target)
    {
		if(isDisabled)
		{
			return;
		}
		if(target.IsInstantaneous())
		{
			
			float distance = Mathf.Infinity;
			if (target.UseNavMeshDistance())
			{
				NavMeshPath path = new NavMeshPath();
				if (NavMesh.CalculatePath(target.transform.position, transform.position, NavMesh.AllAreas, path))
				{
					distance = 0.0f;
					for (int i = 0; i < path.corners.Length - 1; i++)
					{
						distance += Vector3.Distance(path.corners[i], path.corners[i + 1]);
					}
				}
				else
				{
					distance = Mathf.Infinity;
				}
			}
			else
			{
				distance = Vector3.Distance(combatantServiceLocator.GetCombatantID().transform.position, target.transform.position);
			}
			float awarenessAmount = target.GetDistanceMultiplier(distance);
			if (target.GetMaxSuspicion() != 0)
			{
				if (awarenessManager.GetCurrentAwareness() + target.GetSuspicionValue(awarenessAmount) < target.GetMaxSuspicion())
				{
					awarenessManager.GainInstantaneousAwareness(target.GetSuspicionValue(awarenessAmount));
				}
				else if (awarenessManager.GetCurrentAwareness() < target.GetMaxSuspicion())
				{
					awarenessManager.SetCurrentAwareness(target.GetMaxSuspicion());
				}
			}
			else
			{
				awarenessManager.GainInstantaneousAwareness(target.GetSuspicionValue(awarenessAmount));
			}
		}
		if (!suspicionTargets.Contains(target) || target.IsInstantaneous())
		{
			if (!suspicionTargets.Contains(target))
			{
				suspicionTargets.Add(target);
			}
			
			if(target.HasFuzzyLocation())
			{
				Vector3 exactPos = target.GetCombatantID().transform.position;
				float distance = GetDistance(target, target.UseNavMeshDistance());
				Vector3 fuzzyPos = FindRandomNavMeshLocation(exactPos, target.GetFuzzyRadius(distance));
				target.SetCurrentLocation(fuzzyPos + (1.7f * Vector3.up));
			}
			else
			{
				target.SetCurrentLocation(target.GetCombatantID().transform.position);
			}
		}
	}

	Vector3 FindRandomNavMeshLocation(Vector3 center, float radius)
	{
		for (int i = 0; i < 30; i++)
		{
			Vector3 randomDirection = Random.insideUnitSphere * radius;
			randomDirection += center;

			NavMeshHit hit;
			if (NavMesh.SamplePosition(randomDirection, out hit, 2f, NavMesh.AllAreas))
			{
				NavMeshPath path = new NavMeshPath();
				NavMesh.CalculatePath(center, hit.position, NavMesh.AllAreas, path);
				if(path.status == NavMeshPathStatus.PathComplete)
				{
					return hit.position;
				}
			}
		}
		return center;
	}

	public void RemoveSuspicionTarget(SuspicionTarget target)
	{
		suspicionTargets.Remove(target);
	}

	void Update()
	{
		if(isDisabled)
		{
			return;
		}
		List<SuspicionTarget> targetsToRemove = new List<SuspicionTarget>();
		foreach (SuspicionTarget target in suspicionTargets)
		{
			if(!target.IsInstantaneous())
			{
				if (target.AffectedByAngle())
				{
					float distance = GetDistance(target, target.UseNavMeshDistance());
					float angle = GetAngle(target.transform.position);
					float distanceMultiplier = target.GetDistanceMultiplier(distance);
					float angleMultiplier = target.GetAngleMultiplier(angle);
					float multiplier = distanceMultiplier * angleMultiplier;
					float awarenessAmount = target.GetSuspicionValue(multiplier);
					if (target.GetMaxSuspicion() != 0)
					{
						if (awarenessManager.GetCurrentAwareness() + (awarenessAmount * Time.deltaTime) < target.GetMaxSuspicion())
						{
							awarenessManager.GainAwarenessOverTime(awarenessAmount);
						}
						else if (awarenessManager.GetCurrentAwareness() < target.GetMaxSuspicion())
						{
							awarenessManager.SetCurrentAwareness(target.GetMaxSuspicion());
						}
					}
					else
					{
						awarenessManager.GainAwarenessOverTime(awarenessAmount);
					}
				}
				else
				{
					float distance = GetDistance(target, target.UseNavMeshDistance());
					float distanceMultiplier = target.GetDistanceMultiplier(distance);
					float awarenessAmount = target.GetSuspicionValue(distanceMultiplier);
					if (target.GetMaxSuspicion() != 0)
					{
						if (awarenessManager.GetCurrentAwareness() + (awarenessAmount * Time.deltaTime) < target.GetMaxSuspicion())
						{
							awarenessManager.GainAwarenessOverTime(awarenessAmount);
						}
						else if (awarenessManager.GetCurrentAwareness() < target.GetMaxSuspicion())
						{
							awarenessManager.SetCurrentAwareness(target.GetMaxSuspicion());
						}
					}
					else
					{
						awarenessManager.GainAwarenessOverTime(awarenessAmount);
					}
				}
			}
			

			if (awarenessManager.GetCurrentAwareness() >= awarenessManager.GetMaxAwareness())
			{
				if (!squadTargetManager.HasTarget(target.GetCombatantID()))
				{
					if(visualSensor.GetLocallyVisibleTargets().Contains(target.GetCombatantID()) && !target.IsFuzzy())
					{
						squadTargetManager.AddTarget(target.GetCombatantID());
					}
					else
					{
						squadTargetManager.AddUnseenTarget(target.GetCombatantID());
					}
					
					targetsToRemove.Add(target);

					foreach (SquadTarget squadTarget in squadTargetManager.GetSquadTargets())
					{
						if (squadTarget.combatantID == target.GetCombatantID())
						{
							if (target.HasFuzzyLocation())
							{
								Vector3 exactPos = target.GetCombatantID().transform.position;
								float distance = GetDistance(target, target.UseNavMeshDistance());
								Vector3 fuzzyPos = FindRandomNavMeshLocation(exactPos, target.GetFuzzyRadius(distance));
								squadTarget.SetLKP(fuzzyPos + (1.7f * Vector3.up));
							}
						}
					}
							
				}

				foreach (SquadTarget squadTarget in squadTargetManager.GetSquadTargets())
				{
					if (squadTarget.combatantID == target.GetCombatantID())
					{
						if (!target.HasFuzzyLocation() && squadTarget.isVisible())
						{
							squadTarget.UpdateLKP();
						}
					}
				}
			}
		}

	}

	public List<SuspicionTarget> GetSuspicionTargets()
	{
		return suspicionTargets;
	}

	public float GetAngle(Vector3 targetPos)
	{
		Vector3 toPosition = (targetPos - transform.position);
		return Vector3.Angle(transform.forward, toPosition.normalized);
	}
	public float GetDistance(SuspicionTarget target, bool useNavMeshDistance)
	{
		float distance = Mathf.Infinity;
		if (useNavMeshDistance)
		{
			NavMeshPath path = new NavMeshPath();
			if (NavMesh.CalculatePath(target.transform.position, transform.position, NavMesh.AllAreas, path))
			{
				distance = 0.0f;
				for (int i = 0; i < path.corners.Length - 1; i++)
				{
					distance += Vector3.Distance(path.corners[i], path.corners[i + 1]);
				}
			}
			else
			{
				distance = Mathf.Infinity;
			}
		}
		else
		{
			distance = Vector3.Distance(combatantServiceLocator.GetCombatantID().transform.position, target.transform.position);
		}
		return distance;
	}
}
