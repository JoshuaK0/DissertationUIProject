using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class HealhUI : MonoBehaviour
{
	[SerializeField] HealthManager healthManager;
	[SerializeField] TMP_Text healthText;

	void Start()
	{
		healthManager.OnHealthChange += UpdateUI;
	}

	void UpdateUI()
	{
		healthText.text = healthManager.GetCurrentHealth().ToString();
	}
}
