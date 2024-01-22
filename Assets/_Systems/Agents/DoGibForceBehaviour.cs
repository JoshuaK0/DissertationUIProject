using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoGibForceBehaviour : FSMBehaviour
{
	[SerializeField] RagdollGibForceManager ragdollGibForceManager;
	[SerializeField] float delay;

	public override void EnterBehaviour()
	{
		ragdollGibForceManager.Invoke("DoGibForce", delay);
	}
}
