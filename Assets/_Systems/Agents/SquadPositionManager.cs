using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SquadPositionManager : MonoBehaviour
{
	[SerializeField] SquadTargetManager targetManager;
	[SerializeField] float updateRate;
	[SerializeField] Vector2 minMaxRange;
	[SerializeField] float density;
	[SerializeField] float recalculateDistance;
	[SerializeField] LayerMask LOSLayers;
	[SerializeField] LayerMask combatantLayers;
	[SerializeField] float LOSRadius;

	List<TargetPositionMap> positionMaps = new List<TargetPositionMap>();

	[SerializeField] List<SquadTarget> squadTargets = new List<SquadTarget>();

	int currentPositionMapIndex = 0;

	void Start()
	{
		InvokeRepeating("UpdateSquadPositions", Random.Range(0, updateRate), updateRate);
	}

	void UpdateSquadPositions()
	{
		if(positionMaps.Count > 0)
		{
			positionMaps[currentPositionMapIndex].CalculatePositions();
			if (currentPositionMapIndex < positionMaps.Count-1)
			{
				currentPositionMapIndex++;
			}
			else
			{
				currentPositionMapIndex = 0;
			}
		}
	}

	public void AddTarget(SquadTarget newTarget)
	{
		squadTargets.Add(newTarget);
		
		GameObject newPositionMap = new GameObject();
		newPositionMap.name = newTarget.combatantID.transform.name;
		newPositionMap.transform.SetParent(newTarget.transform);
		newPositionMap.transform.localPosition = Vector3.zero;
		TargetPositionMap newPositionMapComponent = newPositionMap.AddComponent<TargetPositionMap>();
		newPositionMapComponent.squadTarget = newTarget;
		newPositionMapComponent.minMaxRange = minMaxRange;
		newPositionMapComponent.density = density;
		newPositionMapComponent.recalculateDistance = recalculateDistance;
		newPositionMapComponent.LOSLayers = LOSLayers;
		newPositionMapComponent.LOSRadius = LOSRadius;
		newPositionMapComponent.combatantLayer = combatantLayers;

		positionMaps.Add(newPositionMapComponent);
	}

	public void RemoveTarget(SquadTarget newTarget)
	{
		squadTargets.Remove(newTarget);
	}

	public List<TargetPositionMap> GetTargetPositionMap()
	{
		return positionMaps;
	}
}
