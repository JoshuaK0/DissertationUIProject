using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.Rendering.DebugUI;

public class LinearBulletTracer : MonoBehaviour, IProjectileTraceable
{
    [SerializeField] float muzzleVelocity;
	[SerializeField] float lifeTime;
    [SerializeField] Rigidbody rb;

	Vector3 dir;

	bool fired = false;

	public void InitProjectileTracer(Vector3 direction)
    {
		dir = direction;
		
		Destroy(gameObject, lifeTime);
	}

	public void FixedUpdate()
	{
		if (!fired)
		{
			rb.AddForce(dir.normalized * muzzleVelocity, ForceMode.VelocityChange);
			fired = true;
		}
	}
}
