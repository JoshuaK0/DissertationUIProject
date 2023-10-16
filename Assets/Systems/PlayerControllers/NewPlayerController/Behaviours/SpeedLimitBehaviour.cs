using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpeedLimitBehaviour : FSMBehaviour
{
    
    [SerializeField] float maxSpeed;
    [SerializeField] Rigidbody rb;
    [SerializeField] bool keepMomentum;
    [SerializeField] SlopeDetector slopeDetector;
    [SerializeField] GroundDetector groundDetector;
    [SerializeField] float speedChangeThreshold = 9;
    [SerializeField] float speedChangeFactor;
    [SerializeField] bool disableInAir = false;

    float currentMaxSpeed;

    float prevMaxSpeed;

    float flatVel = 0;

    PlayerMovementFSM pm;
    // Update is called once per frame

    public override void EnterBehaviour()
    {
        pm = fsm.GetComponent<PlayerMovementFSM>();
        prevMaxSpeed = pm.GetCurrentMaxSpeed();
        currentMaxSpeed = Mathf.Max(prevMaxSpeed, maxSpeed);
        
    }

    void UpdateMaxSpeed()
    {
        if (groundDetector.IsGrounded())
        {
            

            if (keepMomentum && prevMaxSpeed - maxSpeed > speedChangeThreshold && currentMaxSpeed != 0)
            {
                StopAllCoroutines();
                StartCoroutine(SmoothlyLerpMoveSpeed());
            }
            else
            {
                currentMaxSpeed = maxSpeed;
            }
        }
    }
    public override void UpdateBehaviour()
    {
        flatVel = new Vector3(rb.velocity.x, 0f, rb.velocity.z).magnitude;
        pm.SetCurrentMaxSpeed(Mathf.Min(flatVel, maxSpeed));

        if (maxSpeed != prevMaxSpeed)
        {
            UpdateMaxSpeed();
        }

        if (disableInAir && !groundDetector.IsGrounded())
        {
            return;
        }

        // limiting speed on slope
        // add && !exitingSlope
        if (slopeDetector.OnSlope() && !pm.IsExitingSlope() && groundDetector.IsGrounded())
        {
            if (rb.velocity.magnitude > currentMaxSpeed)
                rb.velocity = rb.velocity.normalized * currentMaxSpeed;
        }

        // limiting speed on ground or in air
        else
        {
            
            // limit velocity if needed
            if (flatVel > currentMaxSpeed)
            {
                
                Vector3 limitedVel = new Vector3(rb.velocity.x, 0f, rb.velocity.z).normalized * currentMaxSpeed;
                rb.velocity = new Vector3(limitedVel.x, rb.velocity.y, limitedVel.z);
            }
        }
/*
        if (maxYSpeed != 0 && rb.velocity.y > maxYSpeed)
            rb.velocity = new Vector3(rb.velocity.x, maxYSpeed, rb.velocity.z);*/
    }

    private IEnumerator SmoothlyLerpMoveSpeed()
    {
        while(currentMaxSpeed != maxSpeed)
        {
            currentMaxSpeed = Mathf.Lerp(currentMaxSpeed, maxSpeed, Time.deltaTime * speedChangeFactor);
            yield return null;
        }
    }

    public override void ExitBehaviour()
    {
        if (pm != null)
        {
            pm.SetCurrentMaxSpeed(flatVel);
        }
        //pm.OnNewMaxSpeed -= NewMaxSpeed;
    }
}
