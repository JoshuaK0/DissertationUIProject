using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class ClearAreaBehaviour : FSMBehaviour
{
    CombatantID combatantFSM;
    [SerializeField] float turnSpeed;

    [SerializeField] int turnDirection;

    [SerializeField] float turnFinishThreshold;

    bool finishedFirstDirection;
    bool finishedSecondDirection;
    bool finishedThirdDirection;

    Quaternion startRotation;

    Quaternion rotation1;
    Quaternion rotation2;

    public override void EnterBehaviour()
    {
        combatantFSM = fsm.GetComponent<CombatantID>();
        int randomInt = Random.Range(0, 1);
        if(randomInt == 1 )
        {
            turnDirection = 1;
        }
        else
        {
            turnDirection = -1;
        }

        startRotation = fsm.transform.rotation;

        rotation1 = Quaternion.LookRotation(fsm.transform.right * turnDirection);
        rotation2 = Quaternion.LookRotation(fsm.transform.right * -turnDirection);

        finishedFirstDirection = false;
        finishedSecondDirection = false;
        finishedThirdDirection = false;
    }

    public override void UpdateBehaviour()
    {
        if (finishedSecondDirection)
        {
            combatantFSM.transform.rotation = Quaternion.Slerp(combatantFSM.transform.rotation, startRotation, Time.deltaTime * turnSpeed);

            if (Mathf.Abs(Quaternion.Angle(combatantFSM.transform.rotation, startRotation)) < turnFinishThreshold)
            {
                finishedThirdDirection = true;
            }
        }
        else if (finishedFirstDirection)
        {

            combatantFSM.transform.rotation = Quaternion.Slerp(combatantFSM.transform.rotation, rotation2, Time.deltaTime * turnSpeed);

            if (Mathf.Abs(Quaternion.Angle(combatantFSM.transform.rotation, rotation2)) < turnFinishThreshold)
            {
                finishedSecondDirection = true;
            }
        }
        else
        {
            combatantFSM.transform.rotation = Quaternion.Slerp(combatantFSM.transform.rotation, rotation1, Time.deltaTime * turnSpeed);

            if (Mathf.Abs(Quaternion.Angle(combatantFSM.transform.rotation, rotation1)) < turnFinishThreshold)
            {
                turnDirection = turnDirection * -1;
                finishedFirstDirection = true;
            }
        }
    }

    public bool IsFinished()
    {
        return finishedThirdDirection;
    }
}
