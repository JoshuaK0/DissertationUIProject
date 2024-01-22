using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyObjectBehaviour : FSMBehaviour
{
	[SerializeField] GameObject gameObjectToDestroy;
	[SerializeField] float delay;

	public override void EnterBehaviour()
	{
		Destroy(gameObjectToDestroy, delay);
	}
}
