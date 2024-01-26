using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DestroyOnCollide : MonoBehaviour
{
	[SerializeField] LayerMask lm;
	bool firstFrame = true;

	Vector3 lastPos;
	[SerializeField] float distance;
	Vector3 startPos;

	[SerializeField] Rigidbody rb;

	void Update()
	{
		float distanceFromStart = Vector3.Distance(startPos, transform.position);
		if (distanceFromStart > distance)
		{
			if (Physics.Raycast(lastPos, transform.position - lastPos, out RaycastHit hit, Vector3.Distance(lastPos, transform.position), lm))
			{
				if (lm == (lm | (1 << hit.collider.gameObject.layer)))
				{
					transform.position = hit.point;
					Stop();
				}
			}
			
		}
		lastPos = transform.position;
	}

	void Start()
	{
		firstFrame = false;
		startPos = transform.position;
	}

	public void Stop()
	{
		rb.isKinematic = true;
		rb.velocity = Vector3.zero;
	}
}
