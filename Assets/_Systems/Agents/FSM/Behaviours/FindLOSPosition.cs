using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using static UnityEngine.UI.Image;
using UnityEngine.UIElements;
using static UnityEngine.GraphicsBuffer;

public class FindLOSPosition : FSMBehaviour
{
	[Header("References")]
	[SerializeField] FSMState finishState;

	[Header("Position Sampling Params")]
	[SerializeField] bool hasLOS;
	[SerializeField] bool isCover;
	[SerializeField] float minPositionDistance = 3f;
	[SerializeField] float maxPositionDistance = 10f;
	[SerializeField] float LOSRadius;
	[SerializeField] LayerMask LOSCollisionLayers;
	[SerializeField] bool usePredictedPos;
	[SerializeField] float predictionRangeMultiplier;
	[SerializeField] Vector2 predictedRangeMinMax;

	bool foundPosition = false;


	CombatantFSM combatantFSM;

	List<Vector3> sampledPositions = new();
	List<Vector3> finalList = new();
	bool showGizmos;


	SquadPosition squadPos;

	Vector3 prevMapPos;

	SquadPositionManager positionManager;

	Vector3 finalPos;

	public override void EnterBehaviour()
	{
		showGizmos = true;
		combatantFSM = fsm.GetComponent<CombatantFSM>();
		positionManager = combatantFSM.GetCombatantServices().GetSquadManager().GetSquadPositionManager();
		foundPosition = false;
		
		
	}
	public override void UpdateBehaviour()
	{
		if(!foundPosition)
		{
			FindNewPosition();
		}
		else
		{
			combatantFSM.SetNavDestination(finalPos);
			finalPos = transform.position;
			fsm.ChangeState(finishState);
			
		}
	}

	bool HasLOS(Vector3 origin, Vector3 target, LayerMask lm)
	{
		float dist = Vector3.Distance(origin, target);
		if (dist > maxPositionDistance)
		{
			return false;
		}
		if(dist < minPositionDistance)
		{
			return false;
		}

		Vector3 dir = target - origin;
		RaycastHit hit;
		Debug.DrawRay(origin, dir, Color.blue);
		if (!Physics.SphereCast(origin, LOSRadius, dir, out hit, dist, lm))
		{
			return true;
		}
		return false;
	}

	void FindNewPosition()
	{


		/*		if (combatantFSM.GetNavMeshAgent().hasPath)
				{
					return;
				}
				if (usePredictedPos)
				{
					float dist = Vector3.Distance(combatantFSM.GetTargetLKP(), combatantFSM.transform.position);
					float multiplier = Mathf.Clamp(predictionRangeMultiplier * dist, predictedRangeMinMax.x, predictedRangeMinMax.y);
					target = combatantFSM.GetTargetLKP() + (combatantFSM.GetTarget().lastMovedDir.normalized * multiplier);
				}
				else
				{
					target = combatantFSM.GetTargetLKP();
				}

				tooClose = TooClose();
				hasLOS = HasLOS(combatantFSM.transform.position + eyeHeight * Vector3.up, target, LOSCollisionLayers);
				if (hasLOS)
				{
					if(!TooClose())
					{
						return;
					}
				}
		*//*		if (!TargetMovedEnoughToRelocate())
				{
					if (usePredictedPos)
					{
						Debug.Log("returned2");
					}
					return;
				}*//*

				sampledPositions = FOVPositioning.GetPositionsAroundPoint(target, minPositionDistance, maxPositionDistance, density);
				sampledPositions = FOVPositioning.GetNavMeshPoints(sampledPositions, combatantFSM.transform.position, navmeshSampleRadius);

				List<Vector3> positionsToAvoid = new List<Vector3>();
				CombatantID[] combatants = FindObjectsOfType<CombatantID>();
				foreach (CombatantID combatant in combatants)
				{
					positionsToAvoid.Add(combatant.transform.position);
				}

				sampledPositions = FOVPositioning.PrunePositionsForOverlaps(sampledPositions, positionsToAvoid, otherAgentAvoidRadius);
				finalList = FOVPositioning.PrunePositionsForLOSCheck(sampledPositions, target, LOSRadius, eyeHeight, LOSCollisionLayers);


				targetPos = FOVPositioning.GetClosestPos(finalList, combatantFSM.transform.position);
				combatantFSM.SetNavDestination(targetPos);

				prevTargetPos = combatantFSM.GetTargetLKP();*/
		List<TargetPositionMap> positionMaps = positionManager.GetTargetPositionMap();
		TargetPositionMap myMap = null;
		foreach (TargetPositionMap map in positionMaps)
		{
			if (map.squadTarget == combatantFSM.GetTarget())
			{
				myMap = map;
			}
		}
		if (myMap == null)
		{
			return;
		}
		prevMapPos = myMap.squadTarget.transform.position;

		float closestDist = Mathf.Infinity;
		Vector3 closestPos = transform.position;
		foreach (SquadPosition potentialPos in myMap.GetPositions())
		{
			if (potentialPos.reachable == false || potentialPos.hasLOS != hasLOS || potentialPos.isCover != isCover)
			{
				continue;
			}
			if(potentialPos.occupant != null)
			{
				continue;
			}
			NavMeshPath path = new NavMeshPath();
			NavMesh.CalculatePath(combatantFSM.GetNavMeshAgent().transform.position, potentialPos.position, NavMesh.AllAreas, path);
			if (path.status == NavMeshPathStatus.PathComplete)
			{
				float length = 0.0f;
				for (int j = 1; j < path.corners.Length; ++j)
				{
					length += Vector3.Distance(path.corners[j - 1], path.corners[j]);
				}

				if (length < closestDist)
				{
					closestDist = length;
					closestPos = potentialPos.position;
					squadPos = potentialPos;
					foundPosition = true;
				}
			}
		}
		myMap.AssignToPosition(combatantFSM.GetCombatantServices().GetCombatantID(), closestPos);
		finalPos = closestPos;
	}

	bool TooClose()
	{
		CombatantID[] combatants = FindObjectsOfType<CombatantID>();

		bool tooClose = false;
		foreach(CombatantID combatant in combatants)
		{
			if (combatant != fsm.GetComponent<CombatantID>())
			{
				float dist = Vector3.Distance(combatantFSM.transform.position, combatant.transform.position);
				if (dist < minPositionDistance)
				{
					tooClose = true;
				}
			}
		}
		return tooClose;
	}

	public override void ExitBehaviour()
	{
		showGizmos = false;
		sampledPositions = null;
		CancelInvoke();
	}

	public void OnDrawGizmosSelected()
	{
		if(!showGizmos)
		{
			return;
		}
		
		if (combatantFSM == null)
		{
			return;
		}
		if(finalList.Count == 0)
		{
			return;
		}
		Gizmos.color = Color.cyan;
		foreach (Vector3 pos in finalList)
		{
			Gizmos.DrawSphere(pos, 0.25f);
		}
		Gizmos.color = Color.green;
		if(combatantFSM == null)
		{
			return;
		}
		NavMeshAgent agent = combatantFSM.GetNavMeshAgent();
		if(agent == null)
		{
			return;
		}
		if (agent.hasPath)
		{
			for (int i = 0; i < agent.path.corners.Length - 1; i++)
			{
				Gizmos.DrawLine(agent.path.corners[i], agent.path.corners[i + 1]);
			}
		}
	}
}
