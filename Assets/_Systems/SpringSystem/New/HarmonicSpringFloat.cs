using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HarmonicSpringFloat : MonoBehaviour
{
	[SerializeField] float stiffness;
	[SerializeField] float damping;
	[SerializeField] float targetValue;
	[SerializeField] float currentValue;
	
	SpringUtils.tDampedSpringMotionParams springParams;

	
	float currentVel;

	void Awake()
    {
		springParams = new SpringUtils.tDampedSpringMotionParams();
		SpringUtils.CalcDampedSpringMotionParams(ref springParams, Time.deltaTime, stiffness, damping);
	}

	public void Update()
	{
		SpringUtils.UpdateDampedSpringMotion(ref currentValue, ref currentVel, targetValue, springParams);
		if(Mathf.Abs(targetValue - currentValue) < 0.01)
		{
			currentValue = targetValue;
		}
	}

	public void SetValue(float newValue)
	{
		currentValue = newValue;
	}
	
	public float GetValue()
	{
		return currentValue;
	}
}
