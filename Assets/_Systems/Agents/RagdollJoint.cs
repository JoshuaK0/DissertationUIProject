using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RagdollJoint : MonoBehaviour
{
	Rigidbody rb;
	CharacterJoint characterJoint;
	[SerializeField] float startValue;
	[SerializeField] float speed;

	float lowTwistLimit;
	float highTwistLimit;
	float swing1Limit;
	float swing2Limit;

	SoftJointLimit currentLowTwistLimit;
	SoftJointLimit currentHighTwistLimit;
	SoftJointLimit currentSwing1Limit;
	SoftJointLimit currentSwing2Limit;

	bool doRagdoll;
	void Start()
	{
		rb = GetComponent<Rigidbody>();
		characterJoint = GetComponent<CharacterJoint>();

		if (characterJoint == null)
		{
			return;
		}
		lowTwistLimit = characterJoint.lowTwistLimit.limit;
		highTwistLimit = characterJoint.highTwistLimit.limit;
		swing1Limit = characterJoint.swing1Limit.limit;
		swing2Limit = characterJoint.swing2Limit.limit;

		SoftJointLimit newLimit = characterJoint.lowTwistLimit;
		newLimit.limit = -startValue;
		characterJoint.lowTwistLimit = newLimit;

		newLimit = characterJoint.highTwistLimit;
		newLimit.limit = startValue;
		characterJoint.highTwistLimit = newLimit;

		newLimit = characterJoint.swing1Limit;
		newLimit.limit = startValue;
		characterJoint.swing1Limit = newLimit;

		newLimit = characterJoint.swing2Limit;
		newLimit.limit = startValue;
		characterJoint.swing2Limit = newLimit;

	}

	void Update()
	{
		if (doRagdoll)
		{
			if (characterJoint == null)
			{
				return;
			}

			currentLowTwistLimit = characterJoint.lowTwistLimit;
			currentLowTwistLimit.limit = Mathf.Lerp(currentLowTwistLimit.limit, lowTwistLimit, speed * Time.deltaTime);
			characterJoint.lowTwistLimit = currentLowTwistLimit;

			currentHighTwistLimit = characterJoint.highTwistLimit;
			currentHighTwistLimit.limit = Mathf.Lerp(currentHighTwistLimit.limit, highTwistLimit, speed * Time.deltaTime); ;
			characterJoint.highTwistLimit = currentHighTwistLimit;

			currentSwing1Limit = characterJoint.swing1Limit;
			currentSwing1Limit.limit = Mathf.Lerp(currentSwing1Limit.limit, swing1Limit, speed * Time.deltaTime); ;
			characterJoint.swing1Limit = currentSwing1Limit;

			currentSwing2Limit = characterJoint.swing2Limit;
			currentSwing2Limit.limit = Mathf.Lerp(currentSwing2Limit.limit, swing2Limit, speed * Time.deltaTime); ;
			characterJoint.swing2Limit = currentSwing2Limit;
		}
	}
	public void DoRagdoll()
	{
		doRagdoll = true;
		rb.isKinematic = false;
		rb.useGravity = true;
		rb.velocity = Vector3.zero;
		rb.angularVelocity = Vector3.zero;
	}

	public void DisableRagdoll()
	{
		doRagdoll = false;

		rb.useGravity = false;

		rb.isKinematic = true;

		if (characterJoint == null)
		{
			return;
		}

		SoftJointLimit newLimit = characterJoint.lowTwistLimit;
		newLimit.limit = -startValue;
		characterJoint.lowTwistLimit = newLimit;

		newLimit = characterJoint.highTwistLimit;
		newLimit.limit = startValue;
		characterJoint.highTwistLimit = newLimit;

		newLimit = characterJoint.swing1Limit;
		newLimit.limit = startValue;
		characterJoint.swing1Limit = newLimit;

		newLimit = characterJoint.swing2Limit;
		newLimit.limit = startValue;
		characterJoint.swing2Limit = newLimit;
	}

	public Rigidbody GetRigidbody()
	{
		return rb;
	}
}
