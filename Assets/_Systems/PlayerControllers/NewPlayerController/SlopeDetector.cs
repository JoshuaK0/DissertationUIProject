using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlopeDetector : MonoBehaviour
{
    RaycastHit slopeHit;
    [SerializeField] float checkDistance = 0.3f;
    [SerializeField] float minSlopeAngle = 5;
    [SerializeField] float maxSlopeAngle = 45;
    [SerializeField] Transform orientation;
    [SerializeField] PlayerMovementFSM pm;

    [SerializeField] bool isOnSlope = false;

    void Update()
    {
        if (Physics.Raycast(orientation.position, Vector3.down, out slopeHit, pm.GetCurrentPlayerHeight() * 0.5f + checkDistance))
        {
            
            float angle = Vector3.Angle(Vector3.up, slopeHit.normal);
            isOnSlope = (minSlopeAngle <= angle  && angle <= maxSlopeAngle);
        }
        else
        {
            isOnSlope = false;
        }

        
    }
    public bool OnSlope()
    {
        return isOnSlope;
        
    }

    public Vector3 GetSlopeMoveDirection(Vector3 direction)
    {
        return Vector3.ProjectOnPlane(direction, slopeHit.normal).normalized;
    }

    public RaycastHit GetSlopeHit()
    {
        return slopeHit;
    }
}
