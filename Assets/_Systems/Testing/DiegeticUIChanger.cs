using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DiegeticUIChanger : MonoBehaviour
{
	[SerializeField] GameObject[] ammo1;
	[SerializeField] GameObject[] ammo2;
	[SerializeField] GameObject[] ammo3;
	[SerializeField] GameObject[] ammo4;
	
	[SerializeField] GameObject[] health1;
	[SerializeField] GameObject[] health2;
	[SerializeField] GameObject[] health3;
	[SerializeField] GameObject[] health4;

	[SerializeField] int ammoUIType = 1;
	[SerializeField] int healthUIType = 1;

	[SerializeField] Material healthFlash;
	[SerializeField] Material ammoFlash;

	void Awake()
	{
		ChangeAmmoUI(DiegeticDatabase.Instance.GetAmmoUI());
		ChangeHealthUI(DiegeticDatabase.Instance.GetHealthUI());
	}

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
		ChangeAmmoUI(DiegeticDatabase.Instance.GetAmmoUI());
		ChangeHealthUI(DiegeticDatabase.Instance.GetHealthUI());
	}

	public void ChangeAmmoUI(int newUI)
	{
		DiegeticDatabase.Instance.SetAmmoUI(newUI);
		ammoUIType = DiegeticDatabase.Instance.GetAmmoUI();
		DisableAllAmmo();
		if (ammoUIType == 1)
		{
			foreach (GameObject b in ammo1)
			{
				b.SetActive(true);
			}
		}
		else if (ammoUIType == 2)
		{
			foreach (GameObject b in ammo2)
			{
				b.SetActive(true);
			}
		}
		else if (ammoUIType == 3)
		{
			foreach (GameObject b in ammo3)
			{
				b.SetActive(true);
			}
		}
		else if (ammoUIType == 4)
		{
			foreach (GameObject b in ammo4)
			{
				b.SetActive(true);
			}
		}
	}

	public void ChangeHealthUI(int newUI)
	{
		DisableAllHealth();
		DiegeticDatabase.Instance.SetHealthUI(newUI);
		healthUIType = DiegeticDatabase.Instance.GetHealthUI();
		if (healthUIType == 1)
		{
			foreach (GameObject b in health1)
			{
				b.SetActive(true);
			}
		}
		else if (healthUIType == 2)
		{
			foreach (GameObject b in health2)
			{
				b.SetActive(true);
			}
		}
		else if (healthUIType == 3)
		{
			foreach (GameObject b in health3)
			{
				b.SetActive(true);
			}
		}
		else if (healthUIType == 4)
		{
			foreach (GameObject b in health4)
			{
				b.SetActive(true);
			}
		}
	}

	void DisableAllAmmo()
	{
		
		
		//Disable every behaviour in each array

		foreach (GameObject b in ammo1)
		{
			b.SetActive(false);
		}
		foreach (GameObject b in ammo2)
		{
			b.SetActive(false);
		}
		foreach (GameObject b in ammo3)
		{
			b.SetActive(false);
		}
		foreach (GameObject b in ammo4)
		{
			b.SetActive(false);
		}
		
		if (ammoFlash.HasProperty("_EmissionColor"))
		{
			// Set the HDR color
			ammoFlash.SetColor("_EmissionColor", UnityEngine.Color.black * 0);
			// Update the material's global illumination
		}
	}

	void DisableAllHealth()
	{

		foreach (GameObject b in health1)
		{
			b.SetActive(false); ;
		}
		foreach (GameObject b in health2)
		{
			b.SetActive(false);
		}
		foreach (GameObject b in health3)
		{
			b.SetActive(false);
		}
		foreach (GameObject b in health4)
		{
			b.SetActive(false);
		}

		if (healthFlash.HasProperty("_EmissionColor"))
		{
			// Set the HDR color
			healthFlash.SetColor("_EmissionColor", UnityEngine.Color.black * 0);
			// Update the material's global illumination
		}
	}
}
