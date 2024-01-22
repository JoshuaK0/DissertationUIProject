using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrackPosRot : MonoBehaviour
{
	[SerializeField] Transform track;

	void Update()
	{
		transform.position = track.position;
		transform.rotation = track.rotation;
	}
}
