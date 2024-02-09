using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyObjectBehaviour : FSMBehaviour
{
	[SerializeField] Object objectToDestroy;
	[SerializeField] float delay;

	public override void EnterBehaviour()
	{
		Destroy(objectToDestroy, delay);
	}
}
