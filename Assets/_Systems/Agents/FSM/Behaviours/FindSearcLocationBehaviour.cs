using UnityEngine;
using System.Collections.Generic;
using UnityEngine.AI;
using UnityEngine.UIElements;

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

	int loopCount;

	[SerializeField] int searchAmount;

	float samplingRadius;

	float startSearchTime;

	bool clearing;

	Vector3 currentTargetPos;

	private List<Vector3> GetSampledPoints()
	{
		List<Vector3> sampledPoints = new List<Vector3>();
		float angleIncrement = preferredSearchAngle / (numSamplePoints - 1);

		for (int i = 0; i < numSamplePoints; i++)
		{
			float angle = -(preferredSearchAngle * 0.5f) + (angleIncrement * i);
			float radian = angle * Mathf.Deg2Rad;

			Vector3 samplePoint = new Vector3(Mathf.Sin(radian), 0, Mathf.Cos(radian)) * samplingRadius; 
			samplePoint = transform.position + transform.rotation * samplePoint;

			NavMeshHit hit;
			if (NavMesh.SamplePosition(samplePoint, out hit, navMeshSampleRadius, NavMesh.AllAreas))
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
		if(sampledPoints.Count == 0)
		{
			for (int i = 0; i < numSamplePoints; i++)
			{
				float angle = -(maxSearchAngle * 0.5f) + (angleIncrement * i);
				float radian = angle * Mathf.Deg2Rad;

				Vector3 samplePoint = new Vector3(Mathf.Sin(radian), 0, Mathf.Cos(radian)) * samplingRadius;
				samplePoint = transform.position + transform.rotation * samplePoint;

				NavMeshHit hit;
				if (NavMesh.SamplePosition(samplePoint, out hit, navMeshSampleRadius, NavMesh.AllAreas))
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

		return sampledPoints;
	}

	public override void EnterBehaviour()
	{
		loopCount = 0;
		combatantFSM = fsm.GetComponent<CombatantFSM>();
		samplingRadius = combatantFSM.GetTimeSinceLastSawTarget();
		navMeshAgent = combatantFSM.GetNavMeshAgent();
		clearAreaBehaviour.GetComponent<FSMBehaviour>().SetFSM(fsm);
		clearAreaBehaviour.EnterBehaviour();
		clearing = true;
	}

	void SetNewSearchLocation()
	{
		List<Vector3> sampledPoints = GetSampledPoints();
		if(sampledPoints.Count > 0)
		{
			currentTargetPos = sampledPoints[Random.Range(0, sampledPoints.Count - 1)];
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
					SetNewSearchLocation();
				}
				else
				{
					clearAreaBehaviour.ExitBehaviour();
					fsm.ChangeState(exitBehaviour);
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