using System.Collections.Generic;
using UnityEngine;

public class CombatantVisualSensor : MonoBehaviour
{
	// Serialized fields
	[SerializeField] private SquadTargetManager targetManager;
	[SerializeField] private LayerMask raycastLayerMask;
	[SerializeField] private Transform agentEye;
	[SerializeField] List<CombatantID> localSpottedTargets = new List<CombatantID>();
	[SerializeField] float LOSRadius;

	List<CombatantID> targetsInFOVCollider = new List<CombatantID>();

	private void OnTriggerEnter(Collider other)
	{
		CombatantID combatant = GetCombatantFromCollider(other);
		if (combatant != null && !targetsInFOVCollider.Contains(combatant))
		{
			targetsInFOVCollider.Add(combatant);
		}
	}

	private void Update()
	{
		ProcessVisibleTargets();
	}

	private void OnTriggerExit(Collider other)
	{
		CombatantID combatant = GetCombatantFromCollider(other);
		if (combatant != null)
		{
			targetsInFOVCollider.Remove(combatant);
			if (localSpottedTargets.Contains(combatant))
			{
				localSpottedTargets.Remove(combatant);
				targetManager.RemoveTarget(combatant);
			}
		}
	}

	private CombatantID GetCombatantFromCollider(Collider collider)
	{
		Rigidbody rb = collider.attachedRigidbody;
		return rb ? rb.GetComponent<CombatantID>() : null;
	}

	private void ProcessVisibleTargets()
	{
		List<CombatantID> combatantsToRemove = new List<CombatantID>();

		foreach (CombatantID combatant in targetsInFOVCollider)
		{
			if (IsVisible(combatant))
			{
				if (!localSpottedTargets.Contains(combatant))
				{
					localSpottedTargets.Add(combatant);
					targetManager.AddTarget(combatant);
				}
			}
			else if (localSpottedTargets.Contains(combatant))
			{
				combatantsToRemove.Add(combatant);
			}
		}

		foreach (CombatantID combatant in combatantsToRemove)
		{
			localSpottedTargets.Remove(combatant);
			targetManager.RemoveTarget(combatant);
		}
	}

	private bool IsVisible(CombatantID combatant)
	{
		Vector3 direction = combatant.transform.position - agentEye.position;
		if (Physics.SphereCast(agentEye.position, LOSRadius, direction, out RaycastHit hit, Mathf.Infinity, raycastLayerMask))
		{
			return hit.rigidbody && hit.rigidbody.transform == combatant.transform;
		}
		return false;
	}

	public bool IsPointInFOV(Vector3 point)
	{
		Ray ray = new Ray(point, Vector3.up); // cast a ray upwards
		RaycastHit hit;
		int intersectionCount = 0;

		while (Physics.Raycast(ray, out hit, Mathf.Infinity))
		{
			intersectionCount++;
			ray = new Ray(hit.point + ray.direction * 0.001f, ray.direction);
		}

		// odd intersections means inside, even means outside
		return intersectionCount % 2 != 0;
	}
}