using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyComponentBehaviour : FSMBehaviour
{
    [SerializeField] float delay;
	[SerializeField] Component component;

	public override void EnterBehaviour()
	{
		Destroy(component, delay);
	}
}
