using System.Collections;
using System.Collections.Generic;
using Unity.Burst.Intrinsics;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEngine.Rendering.DebugUI.Table;
using UnityEngine.AI;
using UnityEngine.UIElements;
using static UnityEngine.GraphicsBuffer;
using Unity.Burst.CompilerServices;
using static UnityEngine.UI.Image;
using UnityEngine.Rendering.Universal;
using System.Drawing;
using System.IO;

public static class FOVPositioning
{
	public static List<Vector3> GetPositionsAroundPoint(Vector3 target, float minRange, float maxRange, float density)
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
		foreach(Vector3 position in positions)
		{
			NavMeshHit hit;
			if (NavMesh.SamplePosition(position, out hit, sampleRadius, NavMesh.AllAreas))
			{
				NavMeshPath path = new NavMeshPath();
				NavMesh.CalculatePath(startPos, position, NavMesh.AllAreas, path);
				if(path.status == NavMeshPathStatus.PathComplete)
				{
					points.Add(hit.position);
				}
			}
		}
		return points;
	}

	public static List<Vector3> PrunePositionsForLOSCheck(List<Vector3> positions, Vector3 target, float LOSRadius, float heightOffset, LayerMask lm)
	{
		List<Vector3> results = new List<Vector3>();
		foreach(Vector3 position in positions)
		{
			Vector3 startPos = position + (heightOffset * Vector3.up);
			Vector3 dir = target - startPos;
			float dist = Vector3.Distance(startPos, target);
			RaycastHit hit;
			if (!Physics.SphereCast(startPos, LOSRadius, dir, out hit, dist, lm))
			{
				results.Add(position);
			}
		}
		return results;
	}

	public static List<Vector3> PrunePositionsForOverlaps(List<Vector3> positions, List<Vector3> positionsToAvoid, float minDistance)
	{
		List<Vector3> results = new List<Vector3>();
		foreach (Vector3 position in positions)
		{
			bool valid = true;
			foreach(Vector3 pos in positionsToAvoid)
			{
				float dist = Vector3.Distance(pos, position);
				if(dist < minDistance)
				{
					valid = false;
				}
				
			}
			if(valid)
			{
				results.Add(position);
			}
		}
		return results;
	}

	public static Vector3 GetClosestPos(List<Vector3> positions, Vector3 startPos)
	{
		if(positions.Count == 0)
		{
			return startPos;
		}
		float closestDist = Mathf.Infinity;
		Vector3 closestPos = positions[0];
		foreach(Vector3 position in positions)
		{
			NavMeshPath path = new NavMeshPath();
			NavMesh.CalculatePath(startPos, position, NavMesh.AllAreas, path);
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
					closestPos = position;
				}
			}
		}
		return closestPos;
	}
}
