using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoJumpBehaviour : FSMAbility
{
    [SerializeField] float jumpForce;
    [SerializeField] Rigidbody rb;

    PlayerMovementFSM pm;

    [SerializeField] FSMState endJumpLowSpeedState;
    [SerializeField] FSMState endJumpSprintState;
    [SerializeField] float endJumpSpeedForSprint;
    [SerializeField] float jumpClearanceTime;

    bool exitingJump = false;
    public override void FixedUpdateBehaviour()
    {
		if (exitingJump)
		{
			Vector3 flatVel = new Vector3(rb.velocity.x, 0, rb.velocity.z);
			if (flatVel.magnitude >= endJumpSpeedForSprint)
			{
				fsm.ChangeState(endJumpSprintState);
			}
			else
			{
				fsm.ChangeState(endJumpLowSpeedState);
			}
            exitingJump = false;
            return;
		}
            
		DoAbility();

		pm = fsm.GetComponent<PlayerMovementFSM>();
		pm.SetIsJumping(true, jumpClearanceTime);
		rb.AddForce(Vector3.up * (jumpForce), ForceMode.VelocityChange);
        exitingJump = true;
    }
}
