using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class DamageEffect : MonoBehaviour
{
	[SerializeField] HealthManager healthManager;
	[SerializeField] Volume normalVolume;   // Volume for normal state
	[SerializeField] Volume damageVolume;   // Volume for damage state
	[SerializeField] float cooldownSpeed;
	[SerializeField] AnimationCurve intensityCurve;
	[SerializeField] AnimationCurve cooldownCurve;

	void Start()
	{
		healthManager.OnDamage += DoDamageEffect;
		normalVolume.weight = 1; // Assuming normal state initially
		damageVolume.weight = 0;
	}

	void DoDamageEffect()
	{
		// Increase the weight of the damage volume based on the current health
		float damageIntensity = intensityCurve.Evaluate(1 - healthManager.GetCurrentHealthPercentage());
		damageVolume.weight = damageIntensity;
		normalVolume.weight = 1 - damageIntensity; // Reduce the normal volume's weight correspondingly
	}

	void Update()
	{
		if (damageVolume.weight > 0)
		{
			// Cooldown effect to smoothly transition back to normal state
			float cooldownAmount = cooldownCurve.Evaluate(healthManager.GetCurrentHealthPercentage()) * cooldownSpeed;
			damageVolume.weight = Mathf.MoveTowards(damageVolume.weight, 0, Time.deltaTime * cooldownAmount);
			normalVolume.weight = 1 - damageVolume.weight; // Adjust the normal volume's weight inversely
		}
	}
}
