using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class SpringAimAtBehaviour : FSMBehaviour
{
    CombatantFSM combatantFSM;
    [SerializeField] HarmonicSpringVector3 aimSpring;

    public override void EnterBehaviour()
    {
		combatantFSM = fsm.GetComponent<CombatantFSM>();
        aimSpring.SetValue(combatantFSM.transform.eulerAngles);
	}

    public override void UpdateBehaviour()
    {
        if (combatantFSM.GetTargetLKP() != null)
        {
            
            var lookPos = (combatantFSM.GetTargetLKP() - combatantFSM.transform.position);
            lookPos.y = 0;
            lookPos = lookPos.normalized;
			var rotation = Quaternion.LookRotation(lookPos);
			aimSpring.SetTarget(rotation.eulerAngles);
			combatantFSM.transform.eulerAngles = aimSpring.GetValue();
		}
    }
}
