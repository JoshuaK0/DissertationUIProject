using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpringFloat
{
    float currentValue;
    float currentVelocity;
    float targetValue;
    [SerializeField] float stiffness = 1f; // value highly dependent on use case
    [SerializeField] float damping = 0.1f; // 0 is no damping, 1 is a lot, I think
    float valueThreshold = 0.01f;
    float velocityThreshold = 0.01f;

    public void Update()
    {
        float dampingFactor = Mathf.Max(0, 1 - damping * Time.deltaTime);
        float acceleration = (targetValue - currentValue) * stiffness * Time.deltaTime;
        currentVelocity = currentVelocity * dampingFactor + acceleration;
        currentValue += currentVelocity * Time.deltaTime;

        if (Mathf.Abs(currentValue - targetValue) < valueThreshold && Mathf.Abs(currentVelocity) < velocityThreshold)
        {
            currentValue = targetValue;
            currentVelocity = 0f;
        }
    }

    public void SetTarget(float target)
    {
        this.targetValue = target;
    }

    public void NudgeTarget(float targetDelta)
    {
        this.targetValue += targetDelta;
    }

    public float GetValue()
    {
        return currentValue;
    }

    public SpringFloat(float stiffness, float damping)
    {
        this.stiffness = stiffness;
        this.damping = damping;
    }
}
