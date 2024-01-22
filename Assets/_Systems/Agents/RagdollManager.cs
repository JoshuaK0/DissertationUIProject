using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RagdollManager : MonoBehaviour
{
	[SerializeField] List<RagdollJoint> joints = new List<RagdollJoint>();
	[SerializeField] Animator animator;

	[SerializeField] Transform ragdollPickUp;

	[SerializeField] float extraGravity;
	[SerializeField] float ragdollDrag;

	bool ragdollEnabled = false;
	public void EnableRagdoll()
	{
		foreach (RagdollJoint joint in joints)
		{
			joint.DoRagdoll();
			joint.GetRigidbody().drag = ragdollDrag;
		}
		animator.enabled = false;
		ragdollEnabled = true;
	}

	public void DisableRagdoll()
	{
		foreach (RagdollJoint joint in joints)
		{
			joint.DisableRagdoll();
		}
		ragdollEnabled=false;
	}

	public Transform GetPickUpTransform()
	{
		return ragdollPickUp;
	}

	void FixedUpdate()
	{
		if (ragdollEnabled)
		{
			foreach (RagdollJoint joint in joints)
			{
				joint.GetRigidbody().AddForce(Vector3.down * extraGravity, ForceMode.Acceleration);
			}
		}
	}
}
