using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DelayDecision : FSMDecision
{
    [SerializeField] float delayTime;
    [SerializeField] bool useRandomDelay;
    [SerializeField] Vector2 randomDelay;
    float startTime;

    public override void InitDecision()
    {
        if(useRandomDelay)
        {
            startTime = Random.Range(randomDelay.x, randomDelay.y);
        }
        else
        {
            startTime = delayTime;
        }
    }


    public override bool DecisionEvaluate()
    {
        if (startTime > 0)
        { 
            startTime -= Time.deltaTime;
            return false;
        }
        else
        {
            return true;
        }
    }
}
