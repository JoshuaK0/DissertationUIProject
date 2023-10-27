using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveToLKP : FSMBehaviour
{
	CombatantFSM combatantFSM;

	public override void EnterBehaviour()
	{
		combatantFSM = fsm.GetComponent<CombatantFSM>();
		combatantFSM.SetNavDestination(combatantFSM.GetTargetLKP());
	}
}
