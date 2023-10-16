using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallInFrontDecision : FSMDecision
{
    [Header("References")]
    public Transform orientation;
    
    [Header("Detection")]
    public float detectionLength;
    public LayerMask whatIsWall;
    
    public override bool DecisionEvaluate()
    {
        RaycastHit hit;
        return Physics.Raycast(orientation.position, orientation.forward, out hit, detectionLength, whatIsWall);
    }
}
