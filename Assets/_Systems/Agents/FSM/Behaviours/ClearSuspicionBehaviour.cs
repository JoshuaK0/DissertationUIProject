using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClearSuspicionBehaviour : FSMBehaviour
{
	
	public override void EnterBehaviour()
	{
		CombatantFSM combatantFSM = fsm.GetComponent<CombatantFSM>();
		SuspicionTargetManager targetManager = combatantFSM.GetCombatantServices().GetSuspicionManager();
		if (combatantFSM.GetTarget() != null && combatantFSM.GetSuspicionTarget() != null)
		{
			if(combatantFSM.GetTarget().combatantID == combatantFSM.GetSuspicionTarget().GetCombatantID())
			{
				targetManager.RemoveSuspicionTarget(combatantFSM.GetSuspicionTarget());
				Debug.Log("Cleared suspicion");
			}
		}
	}
}
