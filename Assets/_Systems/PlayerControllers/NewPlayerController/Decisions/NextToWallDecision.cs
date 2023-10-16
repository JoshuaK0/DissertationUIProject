using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NextToWallDecision : FSMDecision
{
    [SerializeField] float wallCheckDistance;
    [SerializeField] Transform orientation;
    [SerializeField] LayerMask whatIsWall;
    public override bool DecisionEvaluate()
    {
        bool wallRight = Physics.Raycast(transform.position, orientation.right, wallCheckDistance, whatIsWall);
        bool wallLeft = Physics.Raycast(transform.position, -orientation.right, wallCheckDistance, whatIsWall);
        return wallRight || wallLeft;
    }
}
