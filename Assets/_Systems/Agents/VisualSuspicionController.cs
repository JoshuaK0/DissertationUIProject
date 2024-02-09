using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VisualSuspicionController : MonoBehaviour
{
	[SerializeField] SuspicionTarget suspicionTarget;
	[SerializeField] Collider visualCollider;
	[SerializeField] List<ColliderVisibility> visibilityColliders = new List<ColliderVisibility>();

	public SuspicionTarget GetSuspicionTarget()
	{
		return suspicionTarget;
	}

	public List<ColliderVisibility> GetVisibilityColliders()
	{
		return visibilityColliders;
	}
}

[System.Serializable]
public class ColliderVisibility
{
	public Collider collider;
	public float visibilityMultiplier; // 0 to 1, where 1 is full visibility
}
