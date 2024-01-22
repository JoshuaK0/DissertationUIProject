using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DestroyOnCollide : MonoBehaviour
{
	[SerializeField] LayerMask lm;
	bool firstFrame = true;

	void Start()
	{
		firstFrame = false;
	}
	void OnTriggerEnter(Collider other)
	{
		if (!firstFrame)
		{
			if (lm == (lm | (1 << other.gameObject.layer)))
			{
				Destroy(gameObject);
			}
			
		}
		
	}
}
