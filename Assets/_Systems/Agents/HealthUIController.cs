using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthUIController : MonoBehaviour
{
	[SerializeField] CombatantFSM combatantFSM;
	[SerializeField] Slider healthSlider;

	void Start()
	{
		combatantFSM.GetCombatantServices().GetHealthManager().OnHealthChange += UpdateHealthUI;
		healthSlider.value = combatantFSM.GetCombatantServices().GetHealthManager().GetCurrentHealth() / combatantFSM.GetCombatantServices().GetHealthManager().GetMaxHealth();

	}

	void UpdateHealthUI()
	{
		healthSlider.value = combatantFSM.GetCombatantServices().GetHealthManager().GetCurrentHealth() / combatantFSM.GetCombatantServices().GetHealthManager().GetMaxHealth();
	}

	void OnDisable()
	{
		combatantFSM.GetCombatantServices().GetHealthManager().OnHealthChange -= UpdateHealthUI;
	}

	void OnDestroy()
	{
		combatantFSM.GetCombatantServices().GetHealthManager().OnHealthChange -= UpdateHealthUI;
	}
}
