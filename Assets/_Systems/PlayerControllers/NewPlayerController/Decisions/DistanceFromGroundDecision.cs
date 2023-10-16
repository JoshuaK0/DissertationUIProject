using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DistanceFromGroundDecision : FSMDecision
{
    [SerializeField] float distanceFromGround;
    [SerializeField] LayerMask whatIsGround;

    public override bool DecisionEvaluate()
    {
        return !Physics.Raycast(transform.position, Vector3.down, distanceFromGround, whatIsGround);
    }
}
