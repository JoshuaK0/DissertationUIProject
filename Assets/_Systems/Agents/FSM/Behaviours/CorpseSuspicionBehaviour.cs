using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CorpseSuspicionBehaviour : FSMBehaviour
{
	[SerializeField] GameObject corpseSuspicionGameObject;

	public override void EnterBehaviour()
	{
		corpseSuspicionGameObject.SetActive(true);
		corpseSuspicionGameObject.transform.parent = null;
	}
}
