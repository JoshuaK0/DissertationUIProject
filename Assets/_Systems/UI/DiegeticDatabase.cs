using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DiegeticDatabase : MonoBehaviour
{
	private static DiegeticDatabase _instance;

	public static DiegeticDatabase Instance { get { return _instance; } }

	int ammoUI = 1;
	int healthUI = 1;


	void Awake()
	{
		if (_instance != null && _instance != this)
		{
			Destroy(this.gameObject);
		}
		else
		{
			_instance = this;
			DontDestroyOnLoad(this.gameObject);
		}
	}

	public void SetAmmoUI(int newUI)
	{
		ammoUI = newUI;
	}

	public int GetAmmoUI()
	{
		return ammoUI;
	}

	public int GetHealthUI()
	{
		return healthUI;
	}

	public void SetHealthUI(int newHealthUI)
	{
		healthUI = newHealthUI;
	}
}
