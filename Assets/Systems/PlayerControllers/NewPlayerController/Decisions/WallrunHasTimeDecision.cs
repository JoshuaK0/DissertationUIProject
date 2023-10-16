using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallrunHasTimeDecision : FSMDecision
{
    [SerializeField] WallrunBehaviour wallrun;

    public override bool DecisionEvaluate()
    {
        return wallrun.GetWallrunTimer() > 0;
    }
}
