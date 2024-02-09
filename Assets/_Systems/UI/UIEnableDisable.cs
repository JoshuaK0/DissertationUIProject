using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIEnableDisable : MonoBehaviour
{
	[SerializeField] GameObject[] objectsToEnable;
	[SerializeField] GameObject[] objectsToDisable;
	public void OnClick()
	{
		foreach(GameObject obj in objectsToEnable)
		{
			obj.SetActive(true);
		}
		foreach (GameObject obj in objectsToDisable)
		{
			obj.SetActive(false);
		}
	}
}
