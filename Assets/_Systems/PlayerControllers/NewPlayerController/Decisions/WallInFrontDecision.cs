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
	public float detectionRayHeightOffset;
    public float detectionRadius;

	public override bool DecisionEvaluate()
    {
        RaycastHit hit;
        return Physics.SphereCast(orientation.position + (Vector3.up * detectionRayHeightOffset), detectionRadius, orientation.forward, out hit, detectionLength, whatIsWall);
    }
}
