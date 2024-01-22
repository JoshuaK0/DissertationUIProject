using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LocalTargetManager : MonoBehaviour
{
	List<CombatantID> targets;

	public void AddTarget(CombatantID target)
	{
		targets.Add(target); 
	}

	public void RemoveTarget(CombatantID target)
	{
		targets.Remove(target);
	}
}
