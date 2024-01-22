using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ApplyGravityBehaviour : FSMBehaviour
{
    [SerializeField] float gravityForce;
    [SerializeField] bool disableOnSlope;
    [SerializeField] SlopeDetector slopeDetector;
    [SerializeField] GroundDetector groundDetector;
    [SerializeField] Rigidbody rb;

    bool doGravity = false;

    public override void EnterBehaviour()
    {
        doGravity = true;
    }

    void FixedUpdate()
    {
        if(doGravity)
        {
            if (disableOnSlope && slopeDetector.OnSlope())
            {
                return;
            }
			if (groundDetector.IsGrounded())
            {
				return;
			}

			else
            {
                rb.AddForce(Vector3.up * -gravityForce, ForceMode.Acceleration);
            }
        }
    }

    public override void ExitBehaviour()
    {
        doGravity = false;
    }
}
