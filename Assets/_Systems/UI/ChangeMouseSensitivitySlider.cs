using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChangeMouseSensitivitySlider : MonoBehaviour
{
	[SerializeField] MouseLook mouseLook;
	[SerializeField] Slider slider;
	
	public void ChangeSensitivity()
	{
		mouseLook.SetSensitivity(slider.value);
	}
}
