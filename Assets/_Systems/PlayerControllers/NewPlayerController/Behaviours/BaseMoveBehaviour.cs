using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseMoveBehaviour : FSMBehaviour
{
    [Header("References")]
    [SerializeField] Rigidbody rb;
    [SerializeField] Transform orientation;
    [SerializeField] SlopeDetector slopeDetector;
    [SerializeField] GroundDetector groundDetector;

    [Header("Movement Stats")]
    [SerializeField] float acceleration;
    [SerializeField] float airMultiplier;

    PlayerMovementFSM pm;

    float horizontalInput;
    float verticalInput;

    Vector3 moveDirection;

    bool doFixedUpdate = false;
    public override void EnterBehaviour()
    {
        doFixedUpdate = true;
        pm = fsm.GetComponent<PlayerMovementFSM>();
    }
    public override void UpdateBehaviour()
    {
        HandleInput();
    }

    void FixedUpdate()
    {
        if(!doFixedUpdate)
        {
            return;
        }
        
        // calculate movement direction
        moveDirection = orientation.forward * verticalInput + orientation.right * horizontalInput;

        // on slope
        if (slopeDetector.OnSlope() && !pm.IsExitingSlope())
        {
            rb.AddForce(slopeDetector.GetSlopeMoveDirection(moveDirection) * acceleration * 20, ForceMode.Force);

            if (rb.velocity.y > 0)
            {
                rb.AddForce(Vector3.down * 80f, ForceMode.Force);
            }

        }

        // on ground
        else if (groundDetector.IsGrounded())
            rb.AddForce(moveDirection.normalized * acceleration * 10, ForceMode.Force);

        // in air
        else if (!groundDetector.IsGrounded())
            rb.AddForce(moveDirection.normalized * acceleration * 10 * airMultiplier, ForceMode.Force);
    }

    public override void ExitBehaviour()
    {
        doFixedUpdate = false;
    }

    void HandleInput()
    {
        horizontalInput = Input.GetAxisRaw("Horizontal");
        verticalInput = Input.GetAxisRaw("Vertical");
    }
}
