using System.Collections.Generic;
using UnityEngine;

public class CombatantEnemyVisualSensor : MonoBehaviour
{
	[SerializeField] float updateRate;
	[SerializeField] int teamIndex;
	[SerializeField] private LayerMask raycastLayerMask;
	[SerializeField] private Transform agentEye;
	[SerializeField] List<VisualSuspicionController> localVisibleTargets = new List<VisualSuspicionController>();
	[SerializeField] float LOSRadius;
	SuspicionTargetManager suspicionManager;
	SquadTargetManager squadTargetManager;

	List<VisualSuspicionController> targetsInFOVCollider = new List<VisualSuspicionController>();

	[SerializeField] CombatantServiceLocator combatantServices;

	AwarenessManager awarenessManager;

	[SerializeField] Transform hitObject;

	

	void Start()
	{
		awarenessManager = combatantServices.GetAwarenessManager();
		suspicionManager = combatantServices.GetSuspicionManager();
		squadTargetManager = combatantServices.GetSquadTargetManager();
	}

	public delegate void OnAwarenessChange();

	public event OnAwarenessChange onAwarenessChange;

	private void OnTriggerEnter(Collider other)
	{
		VisualSuspicionController visualSuspicionController = GetVisualSuspicionControllerFromCollider(other);
		if(visualSuspicionController != null)
		{
			SuspicionTarget suspicionTarget = visualSuspicionController.GetSuspicionTarget();
			if (suspicionTarget.GetCombatantID().GetTeamIndex() != teamIndex && !targetsInFOVCollider.Contains(visualSuspicionController))
			{
				targetsInFOVCollider.Add(visualSuspicionController);
			}
		}
		//ProcessVisibleTargets();
	}

	void Awake()
	{
		InvokeRepeating("ProcessVisibleTargets", Random.Range(0, updateRate), updateRate);
	}
	void Update()
	{
		onAwarenessChange();
	}

	void OnTriggerExit(Collider other)
	{
		VisualSuspicionController visualSuspicionController = GetVisualSuspicionControllerFromCollider(other);
		if (visualSuspicionController != null)
		{
			if(targetsInFOVCollider.Contains(visualSuspicionController))
			{
				targetsInFOVCollider.Remove(visualSuspicionController);
			}
			if (localVisibleTargets.Contains(visualSuspicionController))
			{
				localVisibleTargets.Remove(visualSuspicionController);
				suspicionManager.RemoveSuspicionTarget(visualSuspicionController.GetSuspicionTarget());
				squadTargetManager.RemoveTarget(visualSuspicionController.GetSuspicionTarget().GetCombatantID());
			}
/*			SuspicionTarget suspicionTarget = visualSuspicionController.GetSuspicionTarget();
			if (suspicionTarget.GetCombatantID().GetTeamIndex() != teamIndex)
			{
				targetsInFOVCollider.Remove(visualSuspicionController);
				
			}*/
		}
		//ProcessVisibleTargets();
	}

	private VisualSuspicionController GetVisualSuspicionControllerFromCollider(Collider collider)
	{
		return collider.GetComponent<VisualSuspicionController>();
	}
	
	private void ProcessVisibleTargets()
	{
		List<VisualSuspicionController> combatantsToRemove = new List<VisualSuspicionController>();
		

		foreach (VisualSuspicionController combatant in targetsInFOVCollider)
		{
			CombatantHitpoints hitpoints = combatant.GetHitpoints();
			if(hitpoints == null)
			{
				Debug.Log(combatant.transform);
			}
			Vector3 hitpointPos = hitpoints.GetHitpoint().position;
			
			if (IsVisible(hitpointPos, combatant))
			{
				if (!localVisibleTargets.Contains(combatant))
				{
					localVisibleTargets.Add(combatant);
					suspicionManager.AddSuspicionTarget(combatant.GetSuspicionTarget());
					foreach (SquadTarget target in squadTargetManager.GetSquadTargets())
					{
						if ((!combatant.GetSuspicionTarget().IsFuzzy()) && target.combatantID == combatant.GetSuspicionTarget().GetCombatantID())
						{
							squadTargetManager.AddTarget(combatant.GetSuspicionTarget().GetCombatantID());
						}
					}
					
				}
					
			}
			else
			{
				if (localVisibleTargets.Contains(combatant))
				{
					combatantsToRemove.Add(combatant);
					
				}
				
			}
		}

		foreach (VisualSuspicionController combatant in combatantsToRemove)
		{
			localVisibleTargets.Remove(combatant);
			suspicionManager.RemoveSuspicionTarget(combatant.GetSuspicionTarget());
			squadTargetManager.RemoveTarget(combatant.GetSuspicionTarget().GetCombatantID());
		}
	}

	bool IsVisible(Vector3 pos, VisualSuspicionController combatant)
	{
		Vector3 direction = pos - agentEye.position;
		Debug.DrawLine(pos, agentEye.position);
		if (Physics.SphereCast(agentEye.position, LOSRadius, direction, out RaycastHit hit, Mathf.Infinity, raycastLayerMask, QueryTriggerInteraction.Collide))
		{
			hitObject = hit.collider.transform;
			if (hit.collider == combatant.GetVisualCollider())
			{
				return true;
			}
		}
		return false;
	}

	public bool IsCombatantVisible(Vector3 pos, CombatantID combatant)
	{
		Vector3 direction = pos - agentEye.position;
		if (Physics.SphereCast(agentEye.position, LOSRadius, direction, out RaycastHit hit, Mathf.Infinity, raycastLayerMask, QueryTriggerInteraction.Collide))
		{
			return  (hit.collider) && (hit.collider == combatant.GetCombatantServices().GetVisualCollider());
		}
		return false;
	}

	public bool PointHasLOS(Vector3 pos)
	{
		Vector3 direction = pos - agentEye.position;
		if (!Physics.SphereCast(agentEye.position, LOSRadius, direction, out RaycastHit hit, Mathf.Infinity, raycastLayerMask))
		{
			return true;
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

	public List<CombatantID> GetLocallyVisibleTargets()
	{
		List<CombatantID> targets = new List<CombatantID>();
		foreach (VisualSuspicionController target in localVisibleTargets)
		{
			targets.Add(target.GetSuspicionTarget().GetCombatantID());
		}
		return targets;
	}

	public List<CombatantID> GetAllTargetsInFOV()
	{
		List<CombatantID> targets = new List<CombatantID>();
		foreach (VisualSuspicionController target in localVisibleTargets)
		{
			targets.Add(target.GetSuspicionTarget().GetCombatantID());
		}
		return targets;
	}
}