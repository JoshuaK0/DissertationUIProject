using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthFlashUI : MonoBehaviour
{
    [SerializeField] HealthManager healthManager;
    [SerializeField] FlashWarning healthFlashUI;

    // Update is called once per frame
    void Update()
    {
        float percentage = healthManager.GetCurrentHealthPercentage();

        if (percentage < (2f / 3f))
        {
            healthFlashUI.SetWarningLevel(1 - (healthManager.GetCurrentHealth() / healthManager.GetMaxHealth() * (2f / 3f)));

		}
        else
		{
			healthFlashUI.SetWarningLevel(0);
		}
	}
}
