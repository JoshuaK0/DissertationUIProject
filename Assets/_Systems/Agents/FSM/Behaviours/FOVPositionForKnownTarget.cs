using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using static UnityEngine.UI.Image;
using UnityEngine.UIElements;
using static UnityEngine.GraphicsBuffer;

public class FOVPositionForKnownTarget : FSMBehaviour
{
	[Header("Update Params")]
	[SerializeField] float updatePositionInterval = 0.1f;
	[SerializeField] float targetRecalculateDistance = 1f;

	[Header("Position Sampling Params")]
	[SerializeField] float minPositionDistance = 3f;
	[SerializeField] float maxPositionDistance = 10f;
	[SerializeField] float LOSRadius;
	[SerializeField] float density = 1.0f; // Increase in radius for each step
	[SerializeField] float navmeshSampleRadius = 1;
	[SerializeField] LayerMask LOSCollisionLayers;
	[SerializeField] float eyeHeight = 1.7f;
	

	CombatantFSM combatantFSM;

	List<Vector3> sampledPositions = new();
	List<Vector3> finalList = new();
	Vector3 targetPos;

	Vector3 prevTargetPos;

	public override void EnterBehaviour()
	{
		combatantFSM = fsm.GetComponent<CombatantFSM>();
		prevTargetPos = combatantFSM.GetTargetLKP();
		FindNewPosition();
		InvokeRepeating("FindNewPosition", 0, updatePositionInterval);
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
		if (combatantFSM.GetNavMeshAgent().hasPath)
		{
			return;
		}
		Vector3 target = combatantFSM.GetTargetLKP();

		bool hasLOS = HasLOS(combatantFSM.transform.position + eyeHeight * Vector3.up, target, LOSCollisionLayers);
		if (hasLOS)
		{
			return;
		}

		if (!TargetMovedEnoughToRelocate() && hasLOS)
		{
			return;
		}

		sampledPositions = FOVPositioning.GetPositionsAroundPoint(target, minPositionDistance, maxPositionDistance, density);
		sampledPositions = FOVPositioning.GetNavMeshPoints(sampledPositions, navmeshSampleRadius);
		finalList = FOVPositioning.PrunePositionsForLOSCheck(sampledPositions, target, LOSRadius, eyeHeight, LOSCollisionLayers);
		

		targetPos = FOVPositioning.GetClosestPos(finalList, combatantFSM.transform.position);
		combatantFSM.SetNavDestination(targetPos);

		prevTargetPos = combatantFSM.GetTargetLKP();
	}
	   
	bool TargetMovedEnoughToRelocate()
	{
		return (Vector3.Distance(prevTargetPos, combatantFSM.GetTargetLKP()) > targetRecalculateDistance); 
	}

	public override void ExitBehaviour()
	{
		sampledPositions = null;
		CancelInvoke();
	}

	public void OnDrawGizmosSelected()
	{
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
		Gizmos.DrawSphere(targetPos, 0.5f); 
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
