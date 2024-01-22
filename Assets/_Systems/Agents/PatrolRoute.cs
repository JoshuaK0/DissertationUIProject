using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class PatrolRoute : MonoBehaviour
{
	[SerializeField] List<Transform> waypoints = new List<Transform>();
	bool isOccupied = false;

	public bool IsOccupied()
	{
		return isOccupied;
	}
	public void SetOccupied(bool truity)
	{
		isOccupied = truity;
	}
	public Vector3 GetRouteLocation()
	{
		if (waypoints == null || waypoints.Count == 0)
		{
			Debug.LogError("Transform list is null or empty");
			return Vector3.zero;
		}

		Vector3 sum = Vector3.zero;
		foreach (Transform t in waypoints)
		{
			sum += t.position;
		}

		return sum / waypoints.Count;
	}

	public List<Transform> GetWaypoints()
	{
		return waypoints;
	}
}
