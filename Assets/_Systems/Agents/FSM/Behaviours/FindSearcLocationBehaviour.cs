using UnityEngine;
using System.Collections.Generic;
using UnityEngine.AI;
using UnityEngine.UIElements;
using UnityEngine.Rendering.Universal;
using static UnityEngine.GraphicsBuffer;

public class FindSearchLocationBehaviour : FSMBehaviour
{
	public float predictedTargetSpeed = 5; // The distance away from the AI to sample points
	public int numSamplePoints = 20; // Number of points to sample within the 210-degree range
	[SerializeField] float navMeshSampleRadius;
	[SerializeField] float maxPathLength;
	[SerializeField] float preferredSearchAngle;
	[SerializeField] float maxSearchAngle;
	[SerializeField] float searchStopDist;
	NavMeshAgent navMeshAgent;
	CombatantFSM combatantFSM;
	[SerializeField] FSMState exitBehaviour;
	[SerializeField] ClearAreaBehaviour clearAreaBehaviour;
	[SerializeField] float heightOffset;
	[SerializeField] float LOSRadius;
	[SerializeField] float defaultSamplingRadius;
	[SerializeField] LayerMask LOSLayers;

	int loopCount;

	[SerializeField] int searchAmount;

	float samplingRadius;

	float startSearchTime;

	bool clearing;

	Vector3 currentTargetPos;

	Quaternion rotToLookAtLastvel;

	private List<Vector3> GetSampledPoints()
	{
		if (combatantFSM.GetTarget() == null)
		{
			rotToLookAtLastvel = Quaternion.LookRotation(transform.forward);
		}
		else
		{
			rotToLookAtLastvel = Quaternion.LookRotation(combatantFSM.GetTarget().lastMovedDir);
		}
		
		
		List<Vector3> sampledPoints = new List<Vector3>();
		float angleIncrement = preferredSearchAngle / (numSamplePoints - 1);
		for (int i = 0; i < numSamplePoints; i++)
		{
			float angle = -(preferredSearchAngle * 0.5f) + (angleIncrement * i);
			float radian = angle * Mathf.Deg2Rad;

			Vector3 samplePoint = new Vector3(Mathf.Sin(radian), 0, Mathf.Cos(radian)) * samplingRadius;
			
			if(loopCount == 0)
			{
				samplePoint = transform.position + rotToLookAtLastvel * samplePoint;
			}
			else
			{
				samplePoint = transform.position + transform.rotation * samplePoint;
			}
			

			NavMeshHit hit;
			if (NavMesh.SamplePosition(samplePoint, out hit, navMeshSampleRadius, NavMesh.AllAreas))
			{
				Vector3 startPos = fsm.transform.position + (heightOffset * Vector3.up);
				Vector3 targetPos = samplePoint + (heightOffset * Vector3.up);
				Vector3 dir = startPos - targetPos;
				float dist = Vector3.Distance(startPos, targetPos);
				RaycastHit sphereCastHit;
				if (Physics.SphereCast(startPos, LOSRadius, dir, out sphereCastHit, dist, LOSLayers))
				{
					NavMeshPath path = new NavMeshPath();
					if (NavMesh.CalculatePath(navMeshAgent.transform.position, hit.position, NavMesh.AllAreas, path))
					{
						float length = 0.0f;
						for (int j = 1; j < path.corners.Length; ++j)
						{
							length += Vector3.Distance(path.corners[j - 1], path.corners[j]);
							if (length <= maxPathLength)
							{
								sampledPoints.Add(samplePoint);
							}
						}
					}
				}
			}
		}
		if(sampledPoints.Count == 0)
		{
			angleIncrement = maxSearchAngle / (numSamplePoints - 1);
			for (int i = 0; i < numSamplePoints; i++)
			{
				float angle = -(maxSearchAngle * 0.5f) + (angleIncrement * i);
				float radian = angle * Mathf.Deg2Rad;

				Vector3 samplePoint = new Vector3(Mathf.Sin(radian), 0, Mathf.Cos(radian)) * samplingRadius;
				if (loopCount == 0)
				{
					samplePoint = transform.position + rotToLookAtLastvel * samplePoint;
				}
				else
				{
					samplePoint = transform.position + transform.rotation * samplePoint;
				}

				NavMeshHit hit;
				if (NavMesh.SamplePosition(samplePoint, out hit, navMeshSampleRadius, NavMesh.AllAreas))
				{
					Vector3 startPos = fsm.transform.position + (heightOffset * Vector3.up);
					Vector3 targetPos = samplePoint + (heightOffset * Vector3.up);
					Vector3 dir = startPos - targetPos;
					float dist = Vector3.Distance(startPos, targetPos);
					RaycastHit sphereCastHit;
					if (Physics.SphereCast(startPos, LOSRadius, dir, out sphereCastHit, dist, LOSLayers))
					{
						NavMeshPath path = new NavMeshPath();
						if (NavMesh.CalculatePath(navMeshAgent.transform.position, hit.position, NavMesh.AllAreas, path))
						{
							float length = 0.0f;
							for (int j = 1; j < path.corners.Length; ++j)
							{
								length += Vector3.Distance(path.corners[j - 1], path.corners[j]);
								if (length <= maxPathLength)
								{
									sampledPoints.Add(samplePoint);
								}
							}
						}
					}
				}
			}
		}

		return sampledPoints;
	}

	public override void EnterBehaviour()
	{
		loopCount = 0;
		combatantFSM = fsm.GetComponent<CombatantFSM>();
		samplingRadius = defaultSamplingRadius;
		navMeshAgent = combatantFSM.GetNavMeshAgent();
		clearAreaBehaviour.GetComponent<FSMBehaviour>().SetFSM(fsm);
		clearAreaBehaviour.EnterBehaviour();
		clearing = true;
	}

	void SetNewSearchLocation()
	{
		combatantFSM.AgentUpdateRotation(true);
		List<Vector3> sampledPoints = GetSampledPoints();
		if(sampledPoints.Count > 0)
		{
			float bestDifference = Mathf.Infinity;
			Vector3 bestPoint = sampledPoints[0];

			foreach (Vector3 point in sampledPoints)
			{
				float currentDist = Vector3.Distance(point, fsm.transform.position);
				float currentDifference = Mathf.Abs(currentDist - samplingRadius);
				if (currentDifference < bestDifference)
				{
					bestDifference = currentDifference;
					bestPoint = point;
				}
				currentTargetPos = bestPoint;
			}
			
			combatantFSM.SetNavDestination(currentTargetPos);
		}
		
		startSearchTime = Time.time;
		loopCount += 1;
	}

	public override void UpdateBehaviour()
	{
		if(clearing)
		{
			clearAreaBehaviour.UpdateBehaviour();
		}
		if (loopCount > 0)
		{
			samplingRadius = (Time.time - startSearchTime) * predictedTargetSpeed;
		}
		if(clearAreaBehaviour.IsFinished() && clearing)
		{
			clearing = false;
			if (navMeshAgent.remainingDistance <= searchStopDist)
			{
				if (loopCount < searchAmount)
				{
					clearAreaBehaviour.ExitBehaviour();
					SetNewSearchLocation();
				}
			}
		}
		else if (!clearing && navMeshAgent.remainingDistance <= searchStopDist)
		{
			if (loopCount < searchAmount)
			{
				clearing = true;
				clearAreaBehaviour.EnterBehaviour();
			}
			else
			{
				clearAreaBehaviour.ExitBehaviour();
				fsm.ChangeState(exitBehaviour);
			}
		}
	}
	void OnDrawGizmosSelected()
	{
		if(combatantFSM == null)
		{
			return;
		}

		// Draw the sampled points in the editor for debugging purposes
		foreach (var point in GetSampledPoints())
		{
			Gizmos.DrawSphere(point, 0.25f);
		}

		Gizmos.color = Color.green;
		Gizmos.DrawSphere(currentTargetPos, 0.5f);
	}
}