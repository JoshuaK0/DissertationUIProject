using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SuspicionTarget : MonoBehaviour
{
	[SerializeField] CombatantID CombatantID;
	[SerializeField] Vector2 suspicionValueMinMax;
	[SerializeField] float range;
	[SerializeField] AnimationCurve rangeFalloffCurve;
	[SerializeField] bool fuzzyLocation;
	[SerializeField] Vector2 fuzzyDistanceMinMax;
	[SerializeField] float fuzzyDistanceRange;
	[SerializeField] AnimationCurve fuzzyLocationFalloffCurve;
	[SerializeField] bool instantaneous;
	[SerializeField] bool useNavMeshDistance;
	[SerializeField] bool affectedByAngle;
	[SerializeField] AnimationCurve angleDetectionCurve;
	[SerializeField] Vector2 angleMinMax;
	[SerializeField] float maxSuspicion = 0;

	[SerializeField] float multiplier = 1;
	[SerializeField] int priority;

	[SerializeField] bool rediscoverable;

	bool isInvestigated;

	float currentFuzzyRadius = 0;

	Vector3 currentLocation;

	public void SetInvestigated(bool truity)
	{
		isInvestigated = truity;
	}

	public bool IsInvestigated()
	{
		if(rediscoverable)
		{
			return false;
		}
		return isInvestigated;
	}

	public int GetPriority()
	{
		return priority;
	}

	public void SetMultiplier(float newMultiplier)
	{
		multiplier = newMultiplier;
	}

	public float GetMaxSuspicion()
	{
		return maxSuspicion;
	}

	public bool AffectedByAngle()
	{
		return affectedByAngle;
	}

	public bool UseNavMeshDistance()
	{
		return useNavMeshDistance;
	}

	public bool HasFuzzyLocation()
	{
		return fuzzyLocation;
	}

	public float GetFuzzyRadius(float distance)
	{
		if (distance <= fuzzyDistanceRange)
		{
			float lerpValue = fuzzyLocationFalloffCurve.Evaluate(distance / fuzzyDistanceRange);
			currentFuzzyRadius = Mathf.Lerp(fuzzyDistanceMinMax.x, fuzzyDistanceMinMax.y, lerpValue);
			return Mathf.Lerp(fuzzyDistanceMinMax.x, fuzzyDistanceMinMax.y, lerpValue);
		}

		currentFuzzyRadius = fuzzyDistanceMinMax.y;
		return fuzzyDistanceMinMax.y;
	}

	public bool IsInstantaneous()
	{
		return instantaneous;
	}
	public float GetAngleMultiplier(float angle)
	{
		if(angle > angleMinMax.y)
		{
			return 0;
		}
		else if (angle > angleMinMax.x)
		{
			float lerpValue = Mathf.InverseLerp(angleMinMax.x, angleMinMax.y, angle);
			return angleDetectionCurve.Evaluate(1 - lerpValue);
		}
		else
		{
			return 1;
		}
	}
	public float GetDistanceMultiplier(float distance)
	{
		if(distance <= range)
		{
			float lerpValue = rangeFalloffCurve.Evaluate(1 - Mathf.InverseLerp(0, range, distance));
			return lerpValue;
		}
		return 0;
	}

	public float GetSuspicionValue(float inputMultiplier)
	{
		return Mathf.Lerp(suspicionValueMinMax.x, suspicionValueMinMax.y, inputMultiplier) * multiplier;
	}

	public CombatantID GetCombatantID()
	{
		return CombatantID;
	}

	public void SetRange(float newRange)
	{
		range = newRange;
	}

	public void SetSuspicionValueMinMax(Vector2 newSuspicionValueMinMax)
	{
		suspicionValueMinMax = newSuspicionValueMinMax;
	}

	void OnDrawGizmosSelected()
	{
		Gizmos.color = Color.cyan;
		Gizmos.DrawSphere(currentLocation, 0.5f);
		Gizmos.color = Color.red;
		Gizmos.DrawLine(currentLocation, transform.position);
	}

	public Vector3 GetCurrentLocation()
	{
		return currentLocation;
	}

	public void SetCurrentLocation(Vector3 newLocation)
	{
		currentLocation = newLocation;
	}

	public bool IsFuzzy()
	{
		return fuzzyLocation;
	}
}
