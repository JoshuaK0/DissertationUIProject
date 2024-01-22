using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FootstepSuspicionController : MonoBehaviour
{
	[SerializeField] float speedThreshold;
	[SerializeField] Rigidbody rb;
	[SerializeField] PlayerMovementFSM movementController;
	[SerializeField] SuspicionTarget SuspicionTarget;

	[SerializeField] Vector2 walkingSuspicionValueMinMax;
	[SerializeField] float walkingRange;

	[SerializeField] Vector2 crouchingSuspiocionValueMinMax;
	[SerializeField] float crouchingRange;

	[SerializeField] Vector2 sprintSuspicionValueMinMax;
	[SerializeField] float sprintRange;
	[SerializeField] SphereCollider sphereCollider;
	void Update()
	{
		if(rb.velocity.magnitude > speedThreshold)
		{
			switch (movementController.GetCurrentStance())
			{
				case StanceType.Standing:
					SuspicionTarget.SetRange(walkingRange);
					sphereCollider.radius=walkingRange;
					SuspicionTarget.SetSuspicionValueMinMax(walkingSuspicionValueMinMax);
					break;
				case StanceType.Crouching:
					SuspicionTarget.SetRange(crouchingRange);
					sphereCollider.radius = crouchingRange;
					SuspicionTarget.SetSuspicionValueMinMax(crouchingSuspiocionValueMinMax);
					break;
				case StanceType.Sprinting:
					SuspicionTarget.SetRange(sprintRange);
					sphereCollider.radius = sprintRange;
					SuspicionTarget.SetSuspicionValueMinMax(sprintSuspicionValueMinMax);
					break;
			}
		}
		else
		{
			SuspicionTarget.SetRange(0);
			sphereCollider.radius = 0;
		}
	}

	void OnTriggerEnter(Collider other)
	{
		AuditorySensor sensor = other.GetComponent<AuditorySensor>();
		if (sensor != null)
		{
			sensor.AddSuspicionTarget(SuspicionTarget);
		}
	}

	void OnTriggerExit(Collider other)
	{
		AuditorySensor sensor = other.GetComponent<AuditorySensor>();
		if (sensor != null)
		{
			sensor.RemoveSuspicionTarget(SuspicionTarget);
		}
	}
}
