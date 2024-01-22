using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationController : MonoBehaviour
{
    [SerializeField] Animator animator;
    [SerializeField] Transform velocityTransform;

    [SerializeField] float maxVel;
	
    Vector3 localVel;

	Vector3 previousPosition;

	Vector3 velocity;

	void Update()
    {
		velocity = (transform.position - previousPosition) / Time.deltaTime;
		previousPosition = transform.position;

		if(velocityTransform == null)
		{
			return;
		}

		localVel = velocityTransform.transform.InverseTransformDirection(velocity);
		animator.SetFloat("VelocityX", (Mathf.InverseLerp(-maxVel, maxVel, Mathf.Clamp(-maxVel, localVel.x, maxVel)) * 2) - 1);
		animator.SetFloat("VelocityY", (Mathf.InverseLerp(-maxVel, maxVel, Mathf.Clamp(-maxVel, localVel.z, maxVel)) * 2) - 1);
	}
}
