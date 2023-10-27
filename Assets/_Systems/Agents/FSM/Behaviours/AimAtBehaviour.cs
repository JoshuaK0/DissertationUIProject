using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class AimAtBehaviour : FSMBehaviour
{
    CombatantFSM combatantFSM;
    [SerializeField] float aimSpeed;

    public override void EnterBehaviour()
    {
		combatantFSM = fsm.GetComponent<CombatantFSM>();
    }

    public override void UpdateBehaviour()
    {
        if (combatantFSM.GetTargetLKP() != null)
        {
            var lookPos = combatantFSM.GetTargetLKP() - combatantFSM.transform.position;
            lookPos.y = 0;
            if(lookPos != Vector3.zero)
            {
				var rotation = Quaternion.LookRotation(lookPos);
				combatantFSM.transform.rotation = Quaternion.Slerp(combatantFSM.transform.rotation, rotation, Time.deltaTime * aimSpeed);
			}
        }
    }
}
