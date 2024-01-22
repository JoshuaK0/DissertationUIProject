using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnableRagdollBehaviour : FSMBehaviour
{
	[SerializeField] RagdollManager ragdollManager;
	[SerializeField] Transform ragdollDetatchParent;

	public override void EnterBehaviour()
	{
		ragdollDetatchParent.parent = null;
		ragdollManager.EnableRagdoll();
	}
}
