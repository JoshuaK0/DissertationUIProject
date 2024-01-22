using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealthUIController : MonoBehaviour
{
	[SerializeField] HealthManager healthManager;
	[SerializeField] RectTransform healthUI;
	[SerializeField] TMP_Text healthText;
	[SerializeField] Slider slider;
	[SerializeField] float updateSpeed;

	float maxHealth;

	float targetSliderValue;
	void Start()
	{
		healthManager.OnHealthChange += UpdateHealth;
		maxHealth = healthManager.GetMaxHealth();
		if (healthManager.GetCurrentHealth() == healthManager.GetMaxHealth())
		{
			healthUI.gameObject.SetActive(false);
		}
	}

	void UpdateHealth()
	{
		float currentHealth = healthManager.GetCurrentHealth();
		if (currentHealth == healthManager.GetMaxHealth())
		{
			healthUI.gameObject.SetActive(false);
		}
		else
		{
			healthUI.gameObject.SetActive(true);
			healthText.text = currentHealth.ToString();
			targetSliderValue = currentHealth / maxHealth;
		}
	}

	void Update()
	{
		if (updateSpeed > 0)
		{
			slider.value = Mathf.Lerp(slider.value, 1 - (healthManager.GetCurrentHealth() / maxHealth), Time.deltaTime * updateSpeed);
		}
		else
		{
			slider.value = targetSliderValue;
		}
	}
}
