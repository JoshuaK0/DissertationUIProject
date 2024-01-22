using UnityEngine;

public interface IHitbox
{
	HitboxType GetHitboxType();
	void TakeDamage(float damage);

	void TakeDamage(float damage, Vector3 position, Vector3 direction, float force, Rigidbody body);
	void Heal(float heal);
}
