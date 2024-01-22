using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrackPositionRotation : MonoBehaviour
{
	[SerializeField] Transform positionTrack;
	[SerializeField] Transform rotationTrack;
	[SerializeField] bool localPosition;
	[SerializeField] bool localRotation;
	void Update()
	{
		if (localPosition)
		{
			transform.localPosition = positionTrack.localPosition;
		}
		else
		{
			transform.position = positionTrack.position;
		}
		if (localRotation)
		{
			transform.localEulerAngles = rotationTrack.localEulerAngles;
		}
		else
		{
			transform.eulerAngles = rotationTrack.eulerAngles;
		}
	}
}
