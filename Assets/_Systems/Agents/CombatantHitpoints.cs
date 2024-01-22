using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatantHitpoints : MonoBehaviour
{
	[SerializeField] Transform primaryHitpoint;

	public Transform GetHitpoint()
	{
		return primaryHitpoint;
	}
}
