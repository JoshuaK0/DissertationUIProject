using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileSuspicion : MonoBehaviour
{
	[SerializeField] SuspicionTarget suspicionTarget;
	[SerializeField] float radius;
	[SerializeField] LayerMask lm;
	[SerializeField] float range;
	public void DoProjectile(Vector3 origin, Vector3 direction)
	{
		RaycastHit hit;
		float distance = range;
		if(Physics.Raycast(origin, direction, out hit, Mathf.Infinity))
		{
			distance = hit.distance;
		}
		Collider[] colliders =  Physics.OverlapCapsule(origin, origin + (direction*distance), radius, lm, QueryTriggerInteraction.Collide);
		foreach(Collider collider in colliders)
		{
			AuditorySensor sensor = collider.gameObject.GetComponent<AuditorySensor>();
			if(sensor != null )
			{
				
				sensor.AddSuspicionTarget(suspicionTarget);
			}
		}
	}
}
