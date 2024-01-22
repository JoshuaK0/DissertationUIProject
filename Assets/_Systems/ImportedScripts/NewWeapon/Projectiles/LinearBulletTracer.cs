using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.Rendering.DebugUI;

public class LinearBulletTracer : MonoBehaviour, IProjectileTraceable
{
    [SerializeField] float muzzleVelocity;
	[SerializeField] float lifeTime;
    [SerializeField] Rigidbody rb;

	public void InitProjectileTracer()
    {
		rb.AddForce(transform.forward * muzzleVelocity, ForceMode.VelocityChange);
		Destroy(gameObject, lifeTime);
	}
}
