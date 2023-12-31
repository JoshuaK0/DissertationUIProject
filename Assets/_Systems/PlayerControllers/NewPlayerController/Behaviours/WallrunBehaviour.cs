using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallrunBehaviour : FSMBehaviour
{
    [Header("Wallrunning")]
    public LayerMask whatIsWall;
    public LayerMask whatIsGround;
    public float wallRunForce;
    public float wallJumpUpForce;
    public float wallJumpSideForce;
    public float wallJumpForwardForce;
    public float wallClimbSpeed;
    public float maxWallRunTime;
    private float wallRunTimer;

    [SerializeField] GroundDetector groundDetector;

    [Header("Input")]
    public KeyCode jumpKey = KeyCode.Space;
    public KeyCode upwardsRunKey = KeyCode.LeftShift;
    public KeyCode downwardsRunKey = KeyCode.LeftControl;
    private bool upwardsRunning;
    private bool downwardsRunning;
    private float horizontalInput;
    private float verticalInput;

    [Header("Detection")]
    public float wallCheckDistance;
    public float minJumpHeight;
    private RaycastHit leftWallhit;
    private RaycastHit rightWallhit;
    private bool wallLeft;
    private bool wallRight;

    [Header("Exiting")]
    private bool exitingWall;
    public float exitWallTime;
    private float exitWallTimer;

    [Header("Gravity")]
    public bool useGravity;
    public float gravityForce;

    [Header("References")]
    public Transform orientation;
    public PlayerCam cam;
    private PlayerMovementFSM pm;
    [SerializeField] Rigidbody rb;

    [SerializeField] FSMState exitState;

    bool doFixedUpdate = false;

    bool wallrunning = false;

    Vector3 wallNormal;

    public override void EnterBehaviour()
    {
        doFixedUpdate = true;
        pm = fsm.GetComponent<PlayerMovementFSM>();
        
        wallRight = Physics.Raycast(orientation.position, orientation.right, out rightWallhit, wallCheckDistance, whatIsWall);
        wallLeft = Physics.Raycast(orientation.position, -orientation.right, out leftWallhit, wallCheckDistance, whatIsWall);

        if(wallRight)
        {
            wallNormal = rightWallhit.normal;
        }
        else if (wallLeft)
        {
            wallNormal = leftWallhit.normal;
        }
    }
    
    public override void ExitBehaviour()
    {
        doFixedUpdate = false;
    }

    public override void UpdateBehaviour()
    {
        CheckForWall();
        StateMachine();
    }

    void Update()
    {
        if(groundDetector.IsGrounded())
        {
            wallRunTimer = maxWallRunTime;
        }
    }

    private void FixedUpdate()
    {
        if(!doFixedUpdate)
        {
            return;
        }
        
        if (wallrunning)
            WallRunningMovement();
    }

    private void CheckForWall()
    {
        if(wallRight)
        {
            wallRight = Physics.Raycast(orientation.position, -wallNormal, out rightWallhit, wallCheckDistance, whatIsWall);
        }
        else if (wallLeft)
        {
            wallLeft = Physics.Raycast(orientation.position, -wallNormal, out leftWallhit, wallCheckDistance, whatIsWall);
        }
    }

    private bool AboveGround()
    {
        return !Physics.Raycast(transform.position, Vector3.down, minJumpHeight, whatIsGround);
    }

    private void StateMachine()
    {
        // Getting Inputs
        horizontalInput = Input.GetAxisRaw("Horizontal");
        verticalInput = Input.GetAxisRaw("Vertical");

        upwardsRunning = Input.GetKey(upwardsRunKey);
        downwardsRunning = Input.GetKey(downwardsRunKey);

        // State 1 - Wallrunning
        if ((wallLeft || wallRight) && verticalInput > 0 && AboveGround() && !exitingWall)
        {
            if (!wallrunning)
                StartWallRun();

            // wallrun timer
            if (wallRunTimer > 0)
                wallRunTimer -= Time.deltaTime;

            if (wallRunTimer <= 0 && wallrunning)
            {
                exitingWall = true;
                exitWallTimer = exitWallTime;
            }

            // wall jump
            if (Input.GetKeyDown(jumpKey)) WallJump();
        }

        // State 2 - Exiting
        else if (exitingWall)
        {
            if (wallrunning)
                StopWallRun();

            if (exitWallTimer > 0)
                exitWallTimer -= Time.deltaTime;

            if (exitWallTimer <= 0)
            {
                exitingWall = false;
                pm.ChangeState(exitState);
            }
                
            
        }

        // State 3 - None
        else
        {
            StopWallRun();
            pm.ChangeState(exitState);
        }
    }

    public float GetWallrunTimer()
    {
        return wallRunTimer;
    }

    private void StartWallRun()
    {
        wallrunning = true;

        rb.velocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z);

        // apply camera effects
        cam.DoFov(90f);
        if (wallLeft) cam.DoTilt(-5f);
        if (wallRight) cam.DoTilt(5f);
    }

    private void WallRunningMovement()
    {
        if (useGravity)
        {
            rb.AddForce(-Vector3.up * gravityForce, ForceMode.Force);
        }

        

        Vector3 wallForward = Vector3.Cross(wallNormal, transform.up);

        if ((orientation.forward - wallForward).magnitude > (orientation.forward - -wallForward).magnitude)
            wallForward = -wallForward;

        // forward force
        rb.AddForce(GetWallMoveDirection(cam.transform.forward, wallNormal) * wallRunForce, ForceMode.Force);

        // upwards/downwards force
        if (upwardsRunning)
            rb.velocity = new Vector3(rb.velocity.x, wallClimbSpeed, rb.velocity.z);
        if (downwardsRunning)
            rb.velocity = new Vector3(rb.velocity.x, -wallClimbSpeed, rb.velocity.z);

        // push to wall force
        if (!(wallLeft && horizontalInput > 0) && !(wallRight && horizontalInput < 0))
            rb.AddForce(-wallNormal * 100, ForceMode.Force);
    }

    public Vector3 GetWallMoveDirection(Vector3 direction, Vector3 wallNormal)
    {
        return Vector3.ProjectOnPlane(direction, wallNormal).normalized;
    }

    private void StopWallRun()
    {
        wallrunning = false;

        // reset camera effects
        cam.DoFov(80f);
        cam.DoTilt(0f);
    }

    private void WallJump()
    {
        // enter exiting wall state
        exitingWall = true;
        exitWallTimer = exitWallTime;

        Vector3 wallNormal = wallRight ? rightWallhit.normal : leftWallhit.normal;

        Vector3 forceToApply = Vector3.up * wallJumpUpForce + wallNormal * wallJumpSideForce + orientation.forward * wallJumpForwardForce;

        // reset y velocity and add force
        rb.velocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z);
        rb.AddForce(forceToApply, ForceMode.Impulse);
    }
}
