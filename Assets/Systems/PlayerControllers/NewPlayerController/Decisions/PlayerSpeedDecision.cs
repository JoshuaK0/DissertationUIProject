using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

public class PlayerSpeedDecision : FSMDecision
{
    enum SpeedState
    {
        LessThan,
        LessThanOrEqualTo,
        EqualTo,
        GreaterThan,
        GreaterThanOrEqualTo
    }

    [SerializeField] Rigidbody rb;
    [SerializeField] SpeedState speedState;
    [SerializeField] float speed;

    [SerializeField] bool useFlatVel = false;
    

    public override bool FixedUpdateDecisionEvaluate()
    {
        if(!useFlatVel)
        {
            switch (speedState)
            {
                case SpeedState.LessThan:
                    return rb.velocity.magnitude < speed;
                case SpeedState.LessThanOrEqualTo:
                    return rb.velocity.magnitude <= speed;
                case SpeedState.EqualTo:
                    return rb.velocity.magnitude == speed;
                case SpeedState.GreaterThan:
                    return rb.velocity.magnitude > speed;
                case SpeedState.GreaterThanOrEqualTo:
                    return rb.velocity.magnitude >= speed;
                default:
                    return false;
            }
        }
        else
        {
            float flatVel = new Vector3(rb.velocity.x, 0f, rb.velocity.z).magnitude;
            switch (speedState)
            {
                case SpeedState.LessThan:
                    return flatVel < speed;
                case SpeedState.LessThanOrEqualTo:
                    return flatVel <= speed;
                case SpeedState.EqualTo:
                    return flatVel == speed;
                case SpeedState.GreaterThan:
                    return flatVel > speed;
                case SpeedState.GreaterThanOrEqualTo:
                    return flatVel >= speed;
                default:
                    return false;
            }
        }
        
    }
}
