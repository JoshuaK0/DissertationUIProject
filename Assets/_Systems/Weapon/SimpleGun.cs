using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class SimpleGun : MonoBehaviour
{
	[SerializeField] float fireRate;
	[SerializeField] float accuracy;
	[SerializeField] Transform muzzle;
	[SerializeField] GameObject bulletPrefab;
	[SerializeField] float damage;
	[SerializeField] float muzzleVelocity;

	float fireRateCooldown;

	void Update()
	{
		if (fireRateCooldown <= 0)
		{
			if(Input.GetMouseButton(0))
			{
				fireRateCooldown = fireRate;
				DoShot();
			}
		}
		else
		{
			fireRateCooldown -= Time.deltaTime;
		}
		
	}

	void DoShot()
	{
		Vector3 inaccuracy = ApplyInaccuracy(muzzle.forward, accuracy);
		GameObject newBullet = Instantiate(bulletPrefab, muzzle.position, Quaternion.LookRotation(inaccuracy));
		newBullet.transform.eulerAngles = inaccuracy;
		newBullet.GetComponent<Rigidbody>().AddForce(inaccuracy * muzzleVelocity, ForceMode.VelocityChange);

		RaycastHit hit;

		if (Physics.Raycast(muzzle.position, inaccuracy, out hit, 1000))
		{
			if (hit.collider.attachedRigidbody != null)
			{
				
				IDamageable damageable = hit.collider.attachedRigidbody.GetComponent<IDamageable>();
				if (damageable != null)
				{
					damageable.TakeDamage(damage);
				}
			}
		}
	}

	Vector3 ApplyInaccuracy(Vector3 muzzleForward, float spreadRadius)
	{
		Vector3 candidate = Random.insideUnitSphere * spreadRadius + muzzleForward;
		return candidate.normalized;
	}
}
