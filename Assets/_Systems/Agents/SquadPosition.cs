using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SquadPosition : MonoBehaviour
{
	public Vector3 position;
	public CombatantID occupant;
	public bool reachable;
	public bool hasLOS;
	public bool isCover;

	public SquadPosition(Vector3 position)
	{
		this.position = position;
	}

	void OnDrawGizmos()
	{
		Gizmos.color = Color.red;
		if(reachable)
		{
			if (!hasLOS && !isCover)
			{
				Gizmos.color = Color.yellow;
				Gizmos.DrawSphere(position, 0.25f);
			}
			else if (isCover)
			{
				Gizmos.color = Color.red;
				Gizmos.DrawSphere(position, 0.25f);
			}
			else
			{
				Gizmos.color = Color.green;
				Gizmos.DrawSphere(position, 0.25f);
			}
			if(occupant != null)
			{
				Gizmos.color = Color.green;
				Gizmos.DrawLine(position, occupant.transform.position);
			}
		}
	}
}
