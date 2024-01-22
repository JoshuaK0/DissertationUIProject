using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class PlayerMovementFSM : FiniteStateMachine
{
    [Header("References")]
    [SerializeField] CapsuleCollider playerCollider;
    [SerializeField] GroundDetector groundDetector;
    [SerializeField] Rigidbody rb;

    [Header("Player Base Stats")]
    [SerializeField] float currentPlayerHeight;

    [Header("Status flags")]
    [SerializeField] bool exitingSlope;
    [SerializeField] bool isJumping;

	[Header("Stance States")]
	[SerializeField] FSMState crouchingState;
	[SerializeField] FSMState sprintingState;

	float currentMaxSpeed;

    Vector2 drag;

	[SerializeField] float jumpForce;

	[SerializeField] FSMState endJumpLowSpeedState;
	[SerializeField] FSMState endJumpSprintState;
	[SerializeField] float endJumpSpeedForSprint;
	[SerializeField] float jumpClearanceTime;

	bool exitingJump = false;
    bool doJump;
    bool readyToJump = true;

    [SerializeField] float jumpCooldown;

    public bool IsJumpReady()
    {
		return readyToJump;
	}

	public float GetCurrentMaxSpeed()
    {
        return currentMaxSpeed;
    }

    public override void Update()
    {
        base.Update();
/*        if (groundDetector.IsGrounded())
        {
            isJumping = false;
        }*/
    }

    public void SetDrag(Vector2 newDrag)
    {
        drag = newDrag;
    }

    public bool IsJumping()
    {
        return isJumping;
    }

    public void SetCurrentMaxSpeed(float newSpeed)
    {
        currentMaxSpeed = newSpeed;
    }

    public void SetCurrentPlayerHeight(float newHeight)
    {
        if (newHeight != currentPlayerHeight)
        {
            currentPlayerHeight = newHeight;
            playerCollider.height = currentPlayerHeight;
        }
    }

    public float GetCurrentPlayerHeight()
    {
        return currentPlayerHeight;
    }

    public bool IsExitingSlope()
    {
        return exitingSlope;
    }

    public void SetIsJumping(bool value, float cooldown)
    {
        exitingSlope = value;
        isJumping = true;
        Invoke("ResetIsJumping", cooldown);
    }

    void ResetIsJumping()
    {
        isJumping = false;
        exitingSlope = false;
    }

    public override void FixedUpdate()
    {
        base.FixedUpdate();
		Vector3 dragVel = new Vector3(-rb.velocity.x * drag.x, -rb.velocity.y * drag.y, -rb.velocity.z * drag.x);
		rb.AddForce(dragVel, ForceMode.Acceleration);

        if(doJump)
        {
			if (exitingJump)
			{
				Vector3 flatVel = new Vector3(rb.velocity.x, 0, rb.velocity.z);
				if (flatVel.magnitude >= endJumpSpeedForSprint)
				{
					ChangeState(endJumpSprintState);
				}
				else
				{
					ChangeState(endJumpLowSpeedState);
				}
				exitingJump = false;
                doJump = false;

				return;
			}
			else if(groundDetector.IsGrounded())
			{
				SetIsJumping(true, jumpClearanceTime);
				rb.AddForce(Vector3.up * (jumpForce) + (Vector3.up * -rb.velocity.y), ForceMode.VelocityChange);
				exitingJump = true;
			}
		}
        
	}
    public void DoJump()
    {
        doJump = true;
        readyToJump = false;
		Invoke("ResetJump", jumpCooldown);
	}

    void ResetJump()
    {
		readyToJump = true;
	}

    public StanceType GetCurrentStance()
    {
		if (currentState == sprintingState)
		{
            return StanceType.Sprinting;
		}
		else if (currentState == crouchingState)
		{
			return StanceType.Crouching;
		}
        else
        {
			return StanceType.Standing;
		}
	}
}
