
using UnityEngine;

public interface IDamageable
{
	void TakeDamage(float damage);

	void TakeDamage(float damage, Vector3 position, Vector3 direction, float force, Rigidbody body);
	void Heal(float heal);
}
