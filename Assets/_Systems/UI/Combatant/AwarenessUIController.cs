using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AwarenessUIController : MonoBehaviour
{
	[SerializeField] CombatantEnemyVisualSensor sensor;
	[SerializeField] AwarenessManager awarenessManager;
	[SerializeField] CombatantFSM fsm;
	[SerializeField] Slider awarenessSlider;
	[SerializeField] Image eye;
	[SerializeField] Image glass;
	[SerializeField] Image mark;

	[SerializeField] Color eyeColor;
	[SerializeField] Color glassColor;
	[SerializeField] Color markColor;
	[SerializeField] Image fill;

	[SerializeField] float investigationValue;
	[SerializeField] float firstValue;
	
	void Start()
	{
		sensor.onAwarenessChange += UpdateAwarenessUI;
		UpdateAwarenessUI();
	}
	
	void UpdateAwarenessUI()
	{
		if(eye == null || glass == null || mark == null)
		{
			return;
		}
		if(fsm.GetCurrentState() == "")
		{
			eye.gameObject.SetActive(false);
			glass.gameObject.SetActive(false);
			mark.gameObject.SetActive(false);
			awarenessSlider.transform.SetParent(eye.transform);
			awarenessSlider.value = 0;
			fill.color = Color.white;
		}
		if(awarenessManager.GetCurrentAwareness() == 0)
		{
			eye.gameObject.SetActive(false);
			glass.gameObject.SetActive(false);
			mark.gameObject.SetActive(false);
			awarenessSlider.transform.SetParent(eye.transform);
		}
		else if (awarenessManager.GetCurrentAwareness() < investigationValue && awarenessManager.GetCurrentAwareness() > firstValue)
		{
			eye.gameObject.SetActive(true);
			glass.gameObject.SetActive(false);
			mark.gameObject.SetActive(false);
			awarenessSlider.transform.SetParent(eye.transform);
			awarenessSlider.value = (awarenessManager.GetCurrentAwareness() - firstValue) / (investigationValue - firstValue);
			fill.color = eyeColor;
		}
		else if (awarenessManager.GetCurrentAwareness() > firstValue && awarenessManager.GetCurrentAwareness() == awarenessManager.GetMaxAwareness() || fsm.GetTarget() != null)
		{
			eye.gameObject.SetActive(false);
			glass.gameObject.SetActive(false);
			mark.gameObject.SetActive(true);
			awarenessSlider.transform.SetParent(mark.transform);
			awarenessSlider.value = 1;
			fill.color = markColor;
		}
		else if (awarenessManager.GetCurrentAwareness() > firstValue && awarenessManager.GetCurrentAwareness() < awarenessManager.GetMaxAwareness())
		{
			eye.gameObject.SetActive(false);
			glass.gameObject.SetActive(true);
			mark.gameObject.SetActive(false);
			awarenessSlider.transform.SetParent(glass.transform);
			awarenessSlider.value = (awarenessManager.GetCurrentAwareness() - investigationValue) / (awarenessManager.GetMaxAwareness() - investigationValue);
			fill.color = glassColor;
		}
	}
}
