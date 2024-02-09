using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthDollUI : MonoBehaviour
{
	[SerializeField] HealthManager healthManager;
	[SerializeField] GameObject dollObject;

	[SerializeField] Image dollBG;
	[SerializeField] float speed;

	float targetFill = 1;

	void Update()
	{
		if(healthManager.GetCurrentHealthPercentage() == 1)
		{
			dollObject.SetActive(false);
			return;
		}
		else
		{
			dollObject.SetActive(true);
			targetFill = Mathf.Lerp(targetFill, healthManager.GetCurrentHealthPercentage(), speed * Time.deltaTime);
			dollBG.fillAmount = targetFill;
		}
	}
}
