using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hitbox : MonoBehaviour, IHitbox
{
	[SerializeField] Transform damageable;
	[SerializeField] HitboxType hitboxType;
	IDamageable hitbox;

	void Start()
	{
		if (hitbox == null && damageable != null)
		{
			hitbox = damageable.GetComponent<IDamageable>();
		}
	}
		

	public void TakeDamage(float damage)
	{
		hitbox.TakeDamage(damage);
	}

	public void TakeDamage(float damage, Vector3 position, Vector3 direction, float force, Rigidbody body)
	{
		hitbox.TakeDamage(damage, position, direction, force, body);
	}

	public void Heal(float heal)
	{
		hitbox.Heal(heal);
	}

	public HitboxType GetHitboxType()
	{
		return hitboxType;
	}

	public void SetIDamageable(IDamageable damageable)
	{
		hitbox = damageable;
	}
}
