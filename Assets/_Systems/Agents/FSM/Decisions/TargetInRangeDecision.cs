using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetInRangeDecision : FSMDecision
{
    CombatantFSM combatantFSM;
    [SerializeField] float range;
    [SerializeField] EvaluationType evalType;

    enum EvaluationType
    {
        LessThan,
        GreaterThan,
        EqualTo
    }

    public override bool DecisionEvaluate()
    {
        if(combatantFSM.GetTarget() == null)
        {
            return false;
        }
        else if (evalType == EvaluationType.LessThan)
        {
            return Vector3.Distance(combatantFSM.transform.position, combatantFSM.GetTargetLKP()) < range;
        }
        else if (evalType == EvaluationType.GreaterThan)
        {
            return Vector3.Distance(combatantFSM.transform.position, combatantFSM.GetTargetLKP()) > range;
        }
        else if (evalType == EvaluationType.EqualTo)
        {
            return Vector3.Distance(combatantFSM.transform.position, combatantFSM.GetTargetLKP()) == range;
        }
            return false;
    }

    public override void InitDecision()
    {
		combatantFSM = fsm.GetComponent<CombatantFSM>();
    }
}
