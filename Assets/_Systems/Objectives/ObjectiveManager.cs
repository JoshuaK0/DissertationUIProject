using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ObjectiveManager : MonoBehaviour
{
	[Header("Level Logic UI Controller")]
	[SerializeField] LevelLogicUIController levelLogicUIController;
	
	[Header("Objective References")]
	[SerializeField] UnitManager unitManager;
	[SerializeField] HealthManager healthManager;

	[Header("Objective Settings")]
	[SerializeField] int playerTeamIndex;

	void Start()
	{
		SceneManager.LoadScene("LevelLogic", LoadSceneMode.Additive);
		healthManager.OnHealthChange += CheckHealth;
		unitManager.OnUnitsUpdated += CheckUnits;
	}

	void Update()
	{
		if(levelLogicUIController == null)
		{
			levelLogicUIController = FindObjectOfType<LevelLogicUIController>();
		}
	}

	void CheckHealth()
	{
		if (healthManager.GetCurrentHealth() <= 0)
		{
			levelLogicUIController.OnLose();
		}
	}

	void CheckUnits()
	{
		if (unitManager.IsOtherTeamsDead(playerTeamIndex))
		{
			levelLogicUIController.OnWin();
		}
	}
}
