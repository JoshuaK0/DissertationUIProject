using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class GroundDetector : MonoBehaviour
{
    [SerializeField] PlayerMovementFSM pm;
    [SerializeField] LayerMask whatIsGround;
    [SerializeField] Transform playerTransform;
    [SerializeField] float checkDistance = 0.2f;
    [SerializeField] float checkRadius = 0.25f;

    [SerializeField]  bool isGrounded;
    [SerializeField] bool isGroundedCoyoteTime;

    [SerializeField] float groundAngle;

    public float coyoteTime = 0.1f;

    float lastGroundedTime;

    private void Update()
    {
        RaycastHit hit;
        if (Physics.SphereCast(playerTransform.position, checkRadius, Vector3.down, out hit, pm.GetCurrentPlayerHeight() * 0.5f + checkDistance, whatIsGround))
        {
            isGrounded = Vector3.Angle(Vector3.up, hit.normal) <= groundAngle;
        }
        else
        {
            isGrounded = false;
        }
        if(isGrounded)
        {
            lastGroundedTime = Time.time;
        }
        isGroundedCoyoteTime = Time.time - lastGroundedTime <= coyoteTime;
    }
    public bool IsGrounded()
    {
        return isGrounded;
    }

    public bool IsGroundedCoyoteTime()
    {
        return isGroundedCoyoteTime;
    }
}
