using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SquadTarget: MonoBehaviour
{
	public CombatantID combatantID;
	public int spottedCount;
	public Vector3 lastSpottedPosition;
	public Vector3 lastMovedDir;

	Vector3 lastPos;

	public bool leftVisibility;
	
	CombatantHitpoints hitpoints;

	void Start()
	{
		if(combatantID != null)
		{
			hitpoints = combatantID.GetCombatantServices().GetHitpoints();
			if(hitpoints.GetHitpoint() == null )
			{
				Debug.Log("Cant find hitpoints");
			}
			
		}
		else
		{
			Debug.Log("No Combatant ID");
		}
	}

	void Update()
	{
		if(spottedCount > 0)
		{
			leftVisibility = true;
			transform.position = hitpoints.GetHitpoint().position;
			lastSpottedPosition = hitpoints.GetHitpoint().position;
			if((hitpoints.GetHitpoint().position - lastPos).normalized != Vector3.zero)
			{
				lastMovedDir = (hitpoints.GetHitpoint().position - lastPos).normalized;
			}
			
			lastPos = hitpoints.GetHitpoint().position;
		}

		if(leftVisibility && spottedCount <= 0)
		{
			lastSpottedPosition = hitpoints.GetHitpoint().position;
			if ((hitpoints.GetHitpoint().position - lastPos).normalized != Vector3.zero)
			{
				lastMovedDir = (hitpoints.GetHitpoint().position - lastPos).normalized;
			}
		}
	}

	public void EndUpdateLKP()
	{
		leftVisibility = false;
	}

	public bool isVisible()
	{
		return spottedCount > 0;
	}

	void OnDrawGizmosSelected()
	{
		Gizmos.color = Color.cyan;
		Gizmos.DrawWireSphere(lastSpottedPosition, 0.5f);
		Gizmos.DrawLine(transform.position, transform.position + lastMovedDir);
	}

	public void UpdateLKP()
	{
		if (hitpoints == null)
		{
			hitpoints = combatantID.GetCombatantServices().GetHitpoints();
		}
		transform.position = hitpoints.GetHitpoint().position;
		lastSpottedPosition = hitpoints.GetHitpoint().position;
		if ((hitpoints.GetHitpoint().position - lastPos).normalized != Vector3.zero)
		{
			lastMovedDir = (hitpoints.GetHitpoint().position - lastPos).normalized;
		}

		lastPos = hitpoints.GetHitpoint().position;
	}

	public void SetLKP(Vector3 position)
	{
		transform.position = position;
		lastSpottedPosition = position;
	}
}
