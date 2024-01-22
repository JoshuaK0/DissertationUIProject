using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FootstepController : MonoBehaviour
{
	[SerializeField] AudioClip footstepSound;
	[SerializeField] AudioSource footstepSource;
	[SerializeField] Transform velocityTransform;

	[SerializeField] float velocityThreshold;

	Vector3 previousPosition;

	Vector3 velocity;
	void Start()
	{
		footstepSource.clip = footstepSound;
	}

	void Update()
	{
		velocity = (transform.position - previousPosition) / Time.deltaTime;
		previousPosition = transform.position;

		if (velocity.magnitude > velocityThreshold && !footstepSource.isPlaying)
		{
			footstepSource.Play();
		}
		else if(velocity.magnitude < velocityThreshold && footstepSource.isPlaying)
		{
			footstepSource.Stop();
		}
	}
}
