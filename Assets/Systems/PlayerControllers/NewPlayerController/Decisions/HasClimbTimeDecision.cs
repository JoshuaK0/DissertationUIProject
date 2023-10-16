using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HasClimbTimeDecision : FSMDecision
{
    [SerializeField] ClimbingBehaviour climbingBehaviour;

    public override bool DecisionEvaluate()
    {
        return climbingBehaviour.GetClimbTimer() > 0;
    }
}
