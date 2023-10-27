using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class ManualTurningBehaviour : FSMBehaviour
{
    CombatantFSM combatantFSM;
    public override void EnterBehaviour()
    {
		combatantFSM = fsm.GetComponent<CombatantFSM>();
		combatantFSM.AgentUpdateRotation(false);
    }
    public override void ExitBehaviour()
    {
        combatantFSM.AgentUpdateRotation(true);
    }
}
