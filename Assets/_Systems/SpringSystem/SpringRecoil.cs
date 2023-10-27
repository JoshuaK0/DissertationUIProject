using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpringRecoil : MonoBehaviour
{
	[SerializeField] float stiffness;
	[SerializeField] float damping;
	[SerializeField] float recoilSpeed;

	[SerializeField] Vector3 targetValue;
	[SerializeField] Vector3 currentValue;

	[SerializeField] bool isEulerAngle;


	SpringUtils.tDampedSpringMotionParams xSpringParams;
	SpringUtils.tDampedSpringMotionParams ySpringParams;
	SpringUtils.tDampedSpringMotionParams zSpringParams;


	Vector3 currentVel;

	bool isIncreasing = false;
	Vector3 nonSpringTarget;

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
		if(isIncreasing)
		{
			NonSpringMovement();
		}
		else
		{
			SpringUtils.CalcDampedSpringMotionParams(ref xSpringParams, Time.deltaTime, stiffness, damping);
			SpringUtils.CalcDampedSpringMotionParams(ref ySpringParams, Time.deltaTime, stiffness, damping);
			SpringUtils.CalcDampedSpringMotionParams(ref zSpringParams, Time.deltaTime, stiffness, damping);

			SpringUtils.UpdateDampedSpringMotion(ref currentValue.x, ref currentVel.x, targetValue.x, xSpringParams);
			SpringUtils.UpdateDampedSpringMotion(ref currentValue.y, ref currentVel.y, targetValue.y, ySpringParams);
			SpringUtils.UpdateDampedSpringMotion(ref currentValue.z, ref currentVel.z, targetValue.z, zSpringParams);
		}
		

		/*		if(Vector3.Magnitude(currentValue - targetValue) < 0.0001)
				{
					currentValue = targetValue;
				}
		*/
	}

	void NonSpringMovement()
	{
		currentValue = Vector3.MoveTowards(currentValue, nonSpringTarget, recoilSpeed * Time.deltaTime);
		if (Vector3.Magnitude(currentValue - nonSpringTarget) < 0.0001)
		{
			currentValue = nonSpringTarget;
			isIncreasing = false;
		}
	}

	public void SetValue(Vector3 newPos)
	{
		isIncreasing = true;
		nonSpringTarget = newPos;
	}
	public Vector3 GetValue()
	{
		return currentValue;
	}

	public void SetTarget(Vector3 newTarget)
	{
		targetValue.x = Mathf.LerpAngle(targetValue.x, newTarget.x, 1);
		targetValue.y = Mathf.LerpAngle(targetValue.y, newTarget.y, 1);
		targetValue.z = Mathf.LerpAngle(targetValue.z, newTarget.z, 1);
	}
}
