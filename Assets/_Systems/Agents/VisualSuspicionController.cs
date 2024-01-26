using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VisualSuspicionController : MonoBehaviour
{
	[SerializeField] SuspicionTarget suspicionTarget;
	[SerializeField] Collider visualCollider;
	[SerializeField] CombatantHitpoints hitpoints;

	public SuspicionTarget GetSuspicionTarget()
	{
		return suspicionTarget;
	}

	public Collider GetVisualCollider()
	{
		return visualCollider;
	}

	public CombatantHitpoints GetHitpoints()
	{
		return hitpoints;
	}
}
