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
    [SerializeField] float slopeDownforce;

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
		Vector3 flatVel = new Vector3(rb.velocity.x, 0f, rb.velocity.z);

/*		if (flatVel.magnitude > pm.GetCurrentMaxSpeed())
		{
			float speed = Vector3.Magnitude(rb.velocity);
			float brakeSpeed = Mathf.Max(0, speed - pm.GetCurrentMaxSpeed());  // calculate the speed decrease
			Vector3 normalisedVelocity = rb.velocity.normalized;
			if (moveDirection == Vector3.zero)
			{
				normalisedVelocity = moveDirection.normalized;
			}
			Vector3 brakeVelocity = normalisedVelocity * brakeSpeed;  // make the brake Vector3 value  
			rb.AddForce(-brakeVelocity);  // apply opposing brake force
		}*/
	}

    void FixedUpdate()
    {
        if(!doFixedUpdate)
        {
            return;
        }
        
        // calculate movement direction
        moveDirection = (orientation.forward * verticalInput) + (orientation.right * horizontalInput);

        // on slope
        if (slopeDetector.OnSlope() && !pm.IsExitingSlope())
        {
            rb.AddForce(slopeDetector.GetSlopeMoveDirection(moveDirection).normalized * acceleration * 10, ForceMode.Acceleration);
            Debug.DrawRay(transform.position, slopeDetector.GetSlopeMoveDirection(moveDirection) * 2);

            if (rb.velocity.y > 0)
            {
                rb.AddForce(Vector3.down * slopeDownforce, ForceMode.Acceleration);
            }

        }

        // on ground
        else if (groundDetector.IsGrounded())
        {
			rb.AddForce(moveDirection.normalized * acceleration * 10, ForceMode.Acceleration);            //rb.AddForce(moveDirection.normalized * ((acceleration * 10) - rb.velocity.magnitude), ForceMode.Acceleration);
		}

        // in air
        else if (!groundDetector.IsGrounded())
        {
			rb.AddForce(moveDirection.normalized * acceleration * 10 * airMultiplier, ForceMode.Acceleration);
		}
    }

    public override void ExitBehaviour()
    {
        doFixedUpdate = false;
    }

    void HandleInput()
    {
        horizontalInput = Input.GetAxisRaw("Horizontal");
        verticalInput = Input.GetAxisRaw("Vertical");

        if(Input.GetButtonDown("Jump"))
        {
            pm.DoJump();
        }
    }
}
