using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AwarenessUIController : MonoBehaviour
{
	[SerializeField] CombatantEnemyVisualSensor sensor;
	[SerializeField] AwarenessManager awarenessManager;
	[SerializeField] Slider awarenessSlider;
	
	void Start()
	{
		sensor.onAwarenessChange += UpdateAwarenessUI;
	}
	
	void UpdateAwarenessUI()
	{
		awarenessSlider.value = awarenessManager.GetCurrentAwareness() / awarenessManager.GetMaxAwareness();
	}
}
