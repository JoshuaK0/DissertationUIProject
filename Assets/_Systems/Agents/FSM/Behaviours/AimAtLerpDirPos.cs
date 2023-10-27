using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class AimAtLerpDirPos : FSMBehaviour
{
	CombatantFSM combatantFSM;
	[SerializeField] float aimSpeed;

	SquadTarget squadTarget;
	Vector3 lastSeendDir;
	Vector3 lastSeendPos;
	Vector3 targetRot;
	float startDist;
	
	public override void EnterBehaviour()
	{
		combatantFSM = fsm.GetComponent<CombatantFSM>();
		squadTarget = combatantFSM.GetTarget();

		lastSeendPos = new Vector3(squadTarget.lastSpottedPosition.x, 0, squadTarget.lastSpottedPosition.z);
		lastSeendDir = new Vector3(squadTarget.lastMovedDir.x, 0, squadTarget.lastMovedDir.z);

		startDist = Vector3.Distance(squadTarget.lastSpottedPosition, fsm.transform.position);
	}

	public override void UpdateBehaviour()
	{
		Vector3 aimAtRot = (lastSeendPos - new Vector3(fsm.transform.position.x, 0, fsm.transform.position.z)).normalized;
		float currentDist = Vector3.Distance(squadTarget.lastSpottedPosition, fsm.transform.position);

		Debug.DrawRay(fsm.transform.position, lastSeendDir * 10, Color.magenta);


		if (combatantFSM.GetTargetLKP() != null)
		{
			Vector3 targetRot = Vector3.Slerp(lastSeendDir, aimAtRot, currentDist / startDist);
			
			//Debug.DrawRay(fsm.transform.position, aimAtRot * 10);
			//Debug.DrawRay(fsm.transform.position, targetRot * 10);
			
			
			if (targetRot != Vector3.zero)
			{
				var lookAtTargetRot = Quaternion.LookRotation(targetRot);
				combatantFSM.transform.rotation = Quaternion.Slerp(combatantFSM.transform.rotation, lookAtTargetRot, Time.deltaTime * aimSpeed);
			}
		}
	}
}
