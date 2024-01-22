using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AuditorySensor : MonoBehaviour
{
    [SerializeField] SuspicionTargetManager targetManager;
	public void AddSuspicionTarget(SuspicionTarget suspicionTarget)
	{
		targetManager.AddSuspicionTarget(suspicionTarget);
	}

	public void RemoveSuspicionTarget(SuspicionTarget suspicionTarget)
	{
		targetManager.RemoveSuspicionTarget(suspicionTarget);
	}
}
