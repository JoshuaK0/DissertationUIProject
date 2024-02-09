using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ChangeDiegeticUI : MonoBehaviour
{
	[SerializeField] DiegeticUIChanger changer;
	[SerializeField] bool isAmmo;
	[SerializeField] bool isHealth;
	[SerializeField] int type;

	[SerializeField] GameObject[] otherImages;
	[SerializeField] GameObject ownImages;

	private void OnEnable()
	{
		SceneManager.sceneLoaded += OnLevelLoaded;
	}

	private void OnDisable()
	{
		SceneManager.sceneLoaded -= OnLevelLoaded;
	}

	private void OnLevelLoaded(Scene scene, LoadSceneMode loadSceneMode)
	{
		
		UpdateBackground();
	}

	void Start()
	{
		UpdateBackground();
	}

	void UpdateBackground()
	{
		if (isAmmo)
		{
			if (DiegeticDatabase.Instance.GetAmmoUI() != type)
			{
				ownImages.SetActive(false);
			}
			else
			{
				ownImages.SetActive(true);
			}
		}
		else if (isHealth)
		{
			if (DiegeticDatabase.Instance.GetHealthUI() != type)
			{
				ownImages.SetActive(false);
			}
			else
			{
				ownImages.SetActive(true);
			}
		}
	}

	public void OnChange()
	{
		if (isAmmo)
		{
			changer.ChangeAmmoUI(type);
		}
		else if (isHealth)
		{
			changer.ChangeHealthUI(type);
		}

		foreach(GameObject image in otherImages)
		{
			image.SetActive(false);
		}
		ownImages.SetActive(true);
	}
}
