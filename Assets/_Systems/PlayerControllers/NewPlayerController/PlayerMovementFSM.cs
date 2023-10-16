using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class PlayerMovementFSM : FiniteStateMachine
{
    [Header("References")]
    [SerializeField] CapsuleCollider playerCollider;
    [SerializeField] GroundDetector groundDetector;

    [Header("Player Base Stats")]
    [SerializeField] float currentPlayerHeight;

    [Header("Status flags")]
    [SerializeField] bool exitingSlope;
    [SerializeField] bool isJumping;

    float currentMaxSpeed;
    
    public float GetCurrentMaxSpeed()
    {
        return currentMaxSpeed;
    }

    public override void Update()
    {
        base.Update();
        if (groundDetector.IsGrounded())
        {
            isJumping = false;
        }
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
        Invoke("ResetIsJumping", cooldown);
    }

    void ResetIsJumping()
    {
        exitingSlope = false;
    }
}
