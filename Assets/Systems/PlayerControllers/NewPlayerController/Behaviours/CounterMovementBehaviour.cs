using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static PlayerMovementAdvanced;

public class CounterMovementBehaviour : FSMBehaviour
{
    [SerializeField] float drag;
    [SerializeField] Rigidbody rb;
    [SerializeField] bool disableInAir;
    [SerializeField] GroundDetector groundDetector;
    
    public override void UpdateBehaviour()
    {
        if(disableInAir && !groundDetector.IsGrounded())
        {
            rb.drag = 0;
        }
        else
        {
            rb.drag = drag;
        }
    }

    public override void ExitBehaviour()
    {
        rb.drag = 0;
    }
}
