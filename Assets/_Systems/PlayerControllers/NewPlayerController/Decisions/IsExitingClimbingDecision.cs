using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IsExitingClimbingDecision : FSMDecision
{
    [SerializeField] ClimbingBehaviour climbingBehaviour;

    public override bool DecisionEvaluate()
    {
        return climbingBehaviour.IsExitingClimbing();
    }
}
