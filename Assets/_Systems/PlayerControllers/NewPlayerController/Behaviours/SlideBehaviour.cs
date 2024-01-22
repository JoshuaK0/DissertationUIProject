using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlideBehaviour : FSMAbility
{
    [SerializeField] Transform orientation;
    [SerializeField] float slideForce;
	[SerializeField] float downForce;
	[SerializeField] float slopeSlideForce;
    [SerializeField] Rigidbody rb;
    [SerializeField] float minimumVelocity;

    [SerializeField] SlopeDetector slopeDetector;
    [SerializeField] GroundDetector groundDetector;

    [SerializeField] KeyCode slideKey;

    float horizontalInput;
    float verticalInput;

    bool doFixedUpdate;

    [SerializeField] FSMState endSlideLowSpeedState;
    [SerializeField] FSMState endSlideSprintState;
    [SerializeField] float endSlideSpeedForSprint;

    public override void EnterBehaviour()
    {
        DoAbility();
        doFixedUpdate = true;
        if (groundDetector.IsGrounded())
        {
			rb.AddForce(-orientation.up * downForce, ForceMode.VelocityChange);
			if (slopeDetector.OnSlope())
            {
                rb.AddForce(slopeDetector.GetSlopeMoveDirection(orientation.forward) * slideForce, ForceMode.VelocityChange);
            }
            else
            {
                rb.AddForce(orientation.forward * slideForce, ForceMode.VelocityChange);
            }
        }
    }      

    public override void UpdateBehaviour()
    {
        horizontalInput = Input.GetAxisRaw("Horizontal");
        verticalInput = Input.GetAxisRaw("Vertical");

        if (rb.velocity.magnitude < minimumVelocity|| Input.GetKeyUp(slideKey))
        {
            if(rb.velocity.magnitude >= endSlideSpeedForSprint)
            {
                fsm.ChangeState(endSlideSprintState);
            }
            else
            {
                fsm.ChangeState(endSlideLowSpeedState);
            }
        }
    }

    private void FixedUpdate()
    {
        if(!doFixedUpdate)
        {
            return;
        }

        Vector3 inputDirection = orientation.forward * verticalInput + orientation.right * horizontalInput;

        // sliding normal
        if (groundDetector.IsGrounded())
        {
            if (!slopeDetector.OnSlope() || rb.velocity.y > -0.1f)
            {
                //rb.AddForce(inputDirection.normalized * slideForce, ForceMode.Force);
            }
            // sliding down a slope
            else
            {
                float slopeAngle = Vector3.Angle(Vector3.up, slopeDetector.GetSlopeHit().normal);
                float slopeAngleIncrease = 1 + (slopeAngle / 90f);
                rb.AddForce(slopeDetector.GetSlopeMoveDirection(inputDirection) * slopeSlideForce * slopeAngleIncrease, ForceMode.Acceleration);
            }
        }
        

        
    }

    public override void ExitBehaviour()
    {
        doFixedUpdate = false;
    }
}
