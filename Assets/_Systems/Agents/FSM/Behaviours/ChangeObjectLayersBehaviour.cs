using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeObjectLayersBehaviour : FSMBehaviour
{
	[SerializeField] GameObject[] objects;
	[SerializeField] string layerName;

	public override void EnterBehaviour()
	{
		foreach (GameObject obj in objects)
		{
			obj.layer = LayerMask.NameToLayer(layerName);
		}
	}
}
