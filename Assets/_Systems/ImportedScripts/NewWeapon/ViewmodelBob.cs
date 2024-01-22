using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ViewmodelBob : MonoBehaviour
{
	[SerializeField] Transform pivot;
	[SerializeField] Transform target;
	[SerializeField] float speedCurve;
	[SerializeField] float speedMultiplier;

	float curveSin { get => Mathf.Sin(speedCurve); }
	float curveCos { get => Mathf.Cos(speedCurve); }

	[SerializeField] Vector3 travelLimit;
	[SerializeField] Vector3 bobLimit;

	[SerializeField] Vector3 bobRotationMultiplier;

	[SerializeField] float posSmooth;
	[SerializeField] float rotSmooth;
	Vector3 bobRotation;

	Vector3 bobPosition;

	Vector2 walkInput;

	Vector3 startPosition;

	void Start()
	{
		target.parent = pivot;
		startPosition = pivot.localPosition;
	}

	void Update()
	{
		walkInput.x = Input.GetAxisRaw("Horizontal");
		walkInput.y = Input.GetAxisRaw("Vertical");
		walkInput = walkInput.normalized;

		BobOffset();
		BobRotation();

		

		

		pivot.localPosition = Vector3.Lerp(pivot.localPosition, startPosition + bobPosition, posSmooth * Time.deltaTime);
		pivot.localRotation = Quaternion.Lerp(pivot.localRotation, Quaternion.Euler(bobRotation), rotSmooth * Time.deltaTime);
	}

	void BobOffset()
	{
		if(walkInput != Vector2.zero)
		{
			speedCurve += Time.deltaTime * speedMultiplier;
			bobPosition.x = curveCos * bobLimit.x - (walkInput.x * travelLimit.x);
			bobPosition.y = curveSin * bobLimit.y;
			bobPosition.z = -walkInput.y * bobLimit.z;
		}
		else
		{
			bobPosition = Vector3.zero;
		}
		

		
	}
	
	void BobRotation()
	{
		if (walkInput != Vector2.zero)
		{
			bobRotation.x = bobRotationMultiplier.x * (Mathf.Sin(2 * speedCurve));
			bobRotation.y = bobRotationMultiplier.y * curveCos;
			bobRotation.z = bobRotationMultiplier.z * curveCos * walkInput.x;
		}
		else
		{
			bobRotation = Vector3.zero;
		}
	}
}
