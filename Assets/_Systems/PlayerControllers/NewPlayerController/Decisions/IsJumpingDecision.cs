using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IsJumpingDecision : FSMDecision
{
    PlayerMovementFSM pm;
    
    void Start()
    {
        pm = fsm.GetComponent<PlayerMovementFSM>();
    }
    public override bool DecisionEvaluate()
    {
        return pm.IsJumping();
    }
}
