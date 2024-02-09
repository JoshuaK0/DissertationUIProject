using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnableComponentBehaviour : FSMBehaviour
{
	[SerializeField] Behaviour components;
	[SerializeField] bool truity;

	public override void EnterBehaviour()
	{
		components.enabled = truity;
	}
}
