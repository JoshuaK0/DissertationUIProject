using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HarmonicSpringVector3 : MonoBehaviour
{
	[SerializeField] float stiffness;
	[SerializeField] float damping;
	
	[SerializeField] Vector3 targetValue;
	[SerializeField] Vector3 currentValue;

	[SerializeField] bool isEulerAngle;
	

	SpringUtils.tDampedSpringMotionParams xSpringParams;
	SpringUtils.tDampedSpringMotionParams ySpringParams;
	SpringUtils.tDampedSpringMotionParams zSpringParams;


	Vector3 currentVel;

	public void SetParams(float stiffness, float damping)
	{
		this.stiffness = stiffness;
		this.damping = damping;
	}

	void Start()
	{
		xSpringParams = new SpringUtils.tDampedSpringMotionParams();
		ySpringParams = new SpringUtils.tDampedSpringMotionParams();
		zSpringParams = new SpringUtils.tDampedSpringMotionParams();
		
		SpringUtils.CalcDampedSpringMotionParams(ref xSpringParams, Time.deltaTime, stiffness, damping);
		SpringUtils.CalcDampedSpringMotionParams(ref ySpringParams, Time.deltaTime, stiffness, damping);
		SpringUtils.CalcDampedSpringMotionParams(ref zSpringParams, Time.deltaTime, stiffness, damping);
	}

	public void Update()
	{
		SpringUtils.CalcDampedSpringMotionParams(ref xSpringParams, Time.deltaTime, stiffness, damping);
		SpringUtils.CalcDampedSpringMotionParams(ref ySpringParams, Time.deltaTime, stiffness, damping);
		SpringUtils.CalcDampedSpringMotionParams(ref zSpringParams, Time.deltaTime, stiffness, damping);
		
		SpringUtils.UpdateDampedSpringMotion(ref currentValue.x, ref currentVel.x, targetValue.x, xSpringParams);
		SpringUtils.UpdateDampedSpringMotion(ref currentValue.y, ref currentVel.y, targetValue.y, ySpringParams);
		SpringUtils.UpdateDampedSpringMotion(ref currentValue.z, ref currentVel.z, targetValue.z, zSpringParams);

/*		if(Vector3.Magnitude(currentValue - targetValue) < 0.0001)
		{
			currentValue = targetValue;
		}
*/
	}

	public void SetValue(Vector3 newPos)
	{
		if (isEulerAngle)
		{
			currentValue.x = Mathf.LerpAngle(currentValue.x, newPos.x, 1);
			currentValue.y = Mathf.LerpAngle(currentValue.y, newPos.y, 1);
			currentValue.z = Mathf.LerpAngle(currentValue.z, newPos.z, 1);

		}
		else
		{
			currentValue = newPos;
		}
	}
	public Vector3 GetValue()
	{
		return currentValue;
	}

	public void SetTarget(Vector3 newTarget)
	{
		if(isEulerAngle)
		{
			targetValue.x = Mathf.LerpAngle(targetValue.x, newTarget.x, 1);
			targetValue.y = Mathf.LerpAngle(targetValue.y, newTarget.y, 1);
			targetValue.z = Mathf.LerpAngle(targetValue.z, newTarget.z, 1);

		}
		else
		{
			targetValue = newTarget;
		}
		
	}

	public Vector3 GetTarget()
	{
		return targetValue;
	}
}

