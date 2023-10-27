using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpringPositionTest : MonoBehaviour
{
	[SerializeField] HarmonicSpringVector3 spring;

	void Start()
	{
		spring.SetValue(transform.position);
	}

	void Update()
	{
		transform.position = spring.GetValue();
	}
}
