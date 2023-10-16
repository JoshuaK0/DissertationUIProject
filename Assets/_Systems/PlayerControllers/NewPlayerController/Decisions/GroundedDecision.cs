using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundedDecision : FSMDecision
{
    [SerializeField] GroundDetector groundDetector;
    [SerializeField] bool useCyoteTime = true;

    public override bool DecisionEvaluate()
    {
        if (useCyoteTime)
        {
            return groundDetector.IsGroundedCoyoteTime();
        }
        else
        {
            return groundDetector.IsGrounded();
        }
    }
}
