using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClimbingBehaviour : FSMBehaviour
{
    [Header("References")]
    public Transform orientation;
    public Rigidbody rb;
    public PlayerMovementFSM pm;
    public LayerMask whatIsWall;

    [SerializeField] GroundDetector groundDetector;
    [SerializeField] FSMState exitState;

    [Header("Climbing")]
    public float climbSpeed;
    public float maxClimbTime;
    private float climbTimer;

    private bool climbing;

    [Header("ClimbJumping")]
    public float climbJumpUpForce;
    public float climbJumpBackForce;
    public float climbJumpForwardForce;

    public KeyCode jumpKey = KeyCode.Space;
    public int climbJumps;
    private int climbJumpsLeft;

    [Header("Detection")]
    public float detectionLength;
    public float maxWallLookAngle;
    [SerializeField] float detectionRayHeightOffset;
    [SerializeField] float detectionRadius;
    private float wallLookAngle;

    private RaycastHit frontWallHit;
    private bool wallFront;

    private Transform lastWall;
    private Vector3 lastWallNormal;
    public float minWallNormalAngleChange;

    [Header("Exiting")]
    public bool exitingWall;
    public float exitWallTime;
    private float exitWallTimer;

    Vector3 wallNormal;

    bool doFixedUpdate = false;

    public override void EnterBehaviour()
    {
        doFixedUpdate = true;
        wallFront = Physics.SphereCast(orientation.position + (Vector3.up * detectionRayHeightOffset), detectionRadius, orientation.forward, out frontWallHit, detectionLength, whatIsWall);
        wallNormal = frontWallHit.normal;
    }

    public override void ExitBehaviour()
    {
        doFixedUpdate = false;
    }

    private void Update()
    {
        if (groundDetector.IsGrounded())
        {
            climbTimer = maxClimbTime;
        }

        
    }

    private void FixedUpdate()
    {
        
        if(!doFixedUpdate)
        {
            return;
        }
		WallCheck();
		if (climbing && !exitingWall) ClimbingMovement();
		
	}

    public override void UpdateBehaviour()
    {
        
        
        StateMachine();
    }

    private void StateMachine()
    {
        // State 1 - Climbing
        if (wallFront && Input.GetKey(KeyCode.W) && !exitingWall)
        {
            if (!climbing && climbTimer > 0) StartClimbing();

            // timer
            if (climbTimer > 0) climbTimer -= Time.deltaTime;

            if (climbTimer < 0)
            {
                Debug.Log("stoppage");
                StopClimbing();
				pm.ChangeState(exitState);
			}
        }

        // State 2 - Exiting
        else if (exitingWall)
        {
            
            if (climbing)
            {
                
                StopClimbing();
            }

            if (exitWallTimer > 0) exitWallTimer -= Time.deltaTime;
            if (exitWallTimer < 0)
            {
                exitingWall = false;
                pm.ChangeState(exitState);
                
            }
        }

        // State 3 - None
        else
        {
			StopClimbing();
            pm.ChangeState(exitState);
        }

        if (wallFront && Input.GetKeyDown(jumpKey) && climbJumpsLeft > 0) ClimbJump();
    }

    private void WallCheck()
    {
        wallFront = Physics.SphereCast(orientation.position + (Vector3.up * detectionRayHeightOffset), detectionRadius,orientation.forward, out frontWallHit, detectionLength, whatIsWall);
        wallLookAngle = Vector3.Angle(orientation.forward, -frontWallHit.normal);

        bool newWall = frontWallHit.transform != lastWall || Mathf.Abs(Vector3.Angle(lastWallNormal, frontWallHit.normal)) > minWallNormalAngleChange;

        if ((wallFront && newWall) || groundDetector.IsGrounded())
        {
            climbTimer = maxClimbTime;
            climbJumpsLeft = climbJumps;
        }
    }

    private void StartClimbing()
    {
        climbing = true;
        lastWall = frontWallHit.transform;
        lastWallNormal = frontWallHit.normal;

        /// idea - camera fov change
    }

    private void ClimbingMovement()
    {
        rb.velocity = new Vector3(rb.velocity.x, climbSpeed, rb.velocity.z);

        /// idea - sound effect
    }

    private void StopClimbing()
    {
        climbing = false;

        /// idea - particle effect
        /// idea - sound effect
    }

    public Vector3 GetWallMoveDirection(Vector3 direction, Vector3 wallNormal)
    {
        return Vector3.ProjectOnPlane(direction, wallNormal).normalized;
    }

    private void ClimbJump()
    {
        exitingWall = true;
        exitWallTimer = exitWallTime;

        //Vector3.up * climbJumpUpForce + frontWallHit.normal * climbJumpBackForce + 
        Vector3 forceToApply = (Vector3.up * climbJumpUpForce) + (frontWallHit.normal * climbJumpBackForce) + (orientation.forward * climbJumpForwardForce);

        //rb.velocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z);
        rb.AddForce(forceToApply, ForceMode.Impulse);
        Debug.Log("Jumping");

        climbJumpsLeft--;
    }

    public float GetClimbTimer()
    {
        return climbTimer;
    }

    public bool IsExitingClimbing()
    {
        return exitingWall;
    }
}
