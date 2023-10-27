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

	void Update()
	{
		if(spottedCount > 0)
		{
			transform.position = combatantID.transform.position;
			lastSpottedPosition = combatantID.transform.position;
			if((combatantID.transform.position - lastPos).normalized != Vector3.zero)
			{
				lastMovedDir = (combatantID.transform.position - lastPos).normalized;
			}
			
			lastPos = combatantID.transform.position;
		}
	}

	public bool isVisible()
	{
		return spottedCount > 0;
	}
}
