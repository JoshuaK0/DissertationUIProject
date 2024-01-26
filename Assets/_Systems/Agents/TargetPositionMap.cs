using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class TargetPositionMap : MonoBehaviour
{
	public SquadTarget squadTarget;
	public float recalculateDistance;

	public Vector2 minMaxRange;
	public float density;
	public LayerMask LOSLayers;
	public LayerMask combatantLayer;
	public float LOSRadius;

	List<Vector3> possiblePositions = new List<Vector3>();

	List<SquadPosition> positions = new List<SquadPosition>();

	public List<SquadPosition> GetPositions()
	{
		return positions;
	}

	Vector3 prevPos;

	void Update()
	{
		if (Vector3.Distance(prevPos, squadTarget.lastSpottedPosition) < recalculateDistance)
		{
			return;
		}
		prevPos = squadTarget.lastSpottedPosition;
	}

	void Start()
	{
		List<Vector3> probePositions = GetPositionsAroundPoint(transform.position, minMaxRange.x, minMaxRange.y, density);
		foreach(Vector3 pos in probePositions)
		{
			GameObject positionProbe = new GameObject();
			positionProbe.transform.position = pos;
			positionProbe.transform.SetParent(transform);
			positionProbe.AddComponent<SquadPosition>();
			positions.Add(positionProbe.GetComponent<SquadPosition>());
		}
	}

	public void CalculatePositions()
	{
		foreach (SquadPosition pos in positions)
		{
			if(NavMesh.SamplePosition(pos.transform.position, out NavMeshHit hit, 3f, NavMesh.AllAreas))
			{
				pos.position = hit.position;
				
				if(NavMesh.SamplePosition(squadTarget.lastSpottedPosition, out NavMeshHit reachbilityHit, 3f, NavMesh.AllAreas))
				{
					NavMeshPath path = new NavMeshPath();
					Vector3 combatantIDPos = reachbilityHit.position + 1.7f * Vector3.up;
					NavMesh.CalculatePath(pos.position, combatantIDPos, NavMesh.AllAreas, path);
					if (path.status == NavMeshPathStatus.PathComplete)
					{
						pos.reachable = true;
						Vector3 startPos = pos.position + (1.7f * Vector3.up);
						Vector3 dir = squadTarget.lastSpottedPosition - startPos;
						float dist = Vector3.Distance(startPos, squadTarget.lastSpottedPosition);

						Debug.DrawLine(startPos, squadTarget.lastSpottedPosition);

						RaycastHit LOShit;
						if (!Physics.SphereCast(startPos, LOSRadius, dir, out LOShit, dist, LOSLayers))
						{
							pos.hasLOS = true;
							pos.isCover = false;
						}
						else
						{
							pos.hasLOS = false;
							if(LOShit.rigidbody != null)
							{
								if (combatantLayer == (combatantLayer | (1 << LOShit.rigidbody.gameObject.layer)))
								{
									pos.isCover = false;
								}
								else
								{
									pos.isCover = true;
								}
							}
							else
							{
								pos.isCover = true;
							}
						}
					}
					else
					{
						pos.reachable = false;
						pos.hasLOS = false;
					}
				}
				else
				{
					pos.reachable = false;
					pos.hasLOS = false;
				}

			}
			else
			{
				pos.reachable = false;
				pos.hasLOS = false;
			}
		}
	}

	public List<Vector3> GetPositionsAroundPoint(Vector3 target, float minRange, float maxRange, float density)
	{
		List<Vector3> sampledPoints = new List<Vector3>();
		float radialStep = maxRange / density;

		// Iterate over concentric circles around the agent
		for (float r = radialStep + minRange; r <= maxRange; r += radialStep)
		{
			float circumference = 2 * Mathf.PI * r;
			int numberOfPoints = Mathf.RoundToInt(circumference / radialStep);
			float angleStep = 360f / numberOfPoints;

			// Sample points on the current circle
			for (float theta = 0; theta < 360; theta += angleStep)
			{
				Vector3 point = new Vector3(
					r * Mathf.Cos(Mathf.Deg2Rad * theta),
					0,
					r * Mathf.Sin(Mathf.Deg2Rad * theta)
				);

				point += target;
				sampledPoints.Add(point);
			}
		}
		return sampledPoints;
	}

	public static List<Vector3> GetNavMeshPoints(List<Vector3> positions, Vector3 startPos, float sampleRadius)
	{
		List<Vector3> points = new List<Vector3>();
		foreach (Vector3 position in positions)
		{
			NavMeshHit hit;
			if (NavMesh.SamplePosition(position, out hit, sampleRadius, NavMesh.AllAreas))
			{
				points.Add(hit.position);
			}
		}
		return points;
	}

	public void AssignToPosition(CombatantID combatant, Vector3 pos)
	{
		foreach (SquadPosition squadPosition in positions)
		{
			if(squadPosition.position == pos)
			{
				squadPosition.occupant = combatant;
			}
			else if (squadPosition.occupant == combatant)
			{
				squadPosition.occupant = null;
			}
		}
	}
	
	public void UnassignCombatant(CombatantID combatant)
	{
		foreach (SquadPosition pos in positions)
		{
			if (pos.occupant == combatant)
			{
				pos.occupant = null;
			}
		}
	}
}
