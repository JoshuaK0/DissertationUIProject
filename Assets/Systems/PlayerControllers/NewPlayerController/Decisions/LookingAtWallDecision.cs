using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookingAtWallDecision : FSMDecision
{
    [Header("References")]
    public Transform orientation;

    [Header("Detection")]
    public float detectionLength;
    public float sphereCastRadius;
    public LayerMask whatIsWall;

    public float maxWallLookAngle;
    private float wallLookAngle;
    public override bool DecisionEvaluate()
    {
        RaycastHit hit;
        Physics.SphereCast(orientation.position, sphereCastRadius, orientation.forward, out hit, detectionLength, whatIsWall);
        wallLookAngle = Vector3.Angle(orientation.forward, -hit.normal);
        return wallLookAngle < maxWallLookAngle;
    }
}
