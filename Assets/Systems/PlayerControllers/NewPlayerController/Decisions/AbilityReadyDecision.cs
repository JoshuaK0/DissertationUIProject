using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbilityReadyDecision : FSMDecision
{
    [SerializeField] FSMAbility ability;

    public override bool DecisionEvaluate()
    {
        return ability.GetIsAbilityReady();
    }
}
