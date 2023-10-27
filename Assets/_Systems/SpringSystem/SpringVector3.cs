using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpringVector3
{
    SpringFloat springX;
    SpringFloat springY;
    SpringFloat springZ;
    
    public void SetTarget(Vector3 target)
    {
        springX.SetTarget(target.x);
        springY.SetTarget(target.y);
        springZ.SetTarget(target.z);
    }

    public void NudgeTarget(Vector3 targetDelta)
    {
        springX.NudgeTarget(targetDelta.x);
        springY.NudgeTarget(targetDelta.y);
        springZ.NudgeTarget(targetDelta.z);
    }

    public Vector3 GetValue()
    {
        return new Vector3(springX.GetValue(), springY.GetValue(), springZ.GetValue());    
    }

    public SpringVector3(float stiffness, float damping)
    {
        springX = new SpringFloat(stiffness, damping);
        springY = new SpringFloat(stiffness, damping);
        springZ = new SpringFloat(stiffness, damping);
    }
}
