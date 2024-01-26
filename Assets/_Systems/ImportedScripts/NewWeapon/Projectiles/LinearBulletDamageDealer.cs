using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LinearBulletDamageDealer : MonoBehaviour, IDamageInflictable
{
	[Header("Hit Effect")]
	[SerializeField] GameObject hitEnvironmentEffect;
	[SerializeField] GameObject hitCombatantEffect;
	[SerializeField] LayerMask hitLayerMask;

	[SerializeField] float environmentDirBlend;
	[SerializeField] float combatantDirBlend;
	[SerializeField] float gibForce;
	public void InitDamageInfo(DamagePerBodypartMap damageMap)
	{
		RaycastHit hit;
		if (Physics.Raycast(transform.position, transform.forward, out hit, Mathf.Infinity, hitLayerMask))
		{
			IHitbox hitbox = hit.transform.GetComponent<IHitbox>();
			if (hitbox != null)
			{
				Vector3 effectRot = (-transform.forward.normalized * combatantDirBlend) + (hit.normal.normalized * (1 - combatantDirBlend));
				HitboxType hitboxType = hitbox.GetHitboxType();

				float damage = 0;

				if (hitboxType == HitboxType.Head)
				{
					damage = damageMap.headDamage;
				}
				else if (hitboxType == HitboxType.Torso)
				{
					damage = damageMap.torsoDamage;
				}
				else if (hitboxType == HitboxType.Limbs)
				{
					damage = damageMap.limbsDamage;
				}
				else
				{
					Debug.LogError("HitboxType not found");
				}

				hit.transform.GetComponent<IHitbox>().TakeDamage(damage, hit.point, transform.forward, gibForce, hit.rigidbody);
				//hit.transform.GetComponent<IDamageable>().TakeDamage(damage);
				//hit.transform.GetComponent<IDamageable>().ApplyForce(transform.forward * force);
				GameObject newEffect = Instantiate(hitCombatantEffect, hit.point, Quaternion.FromToRotation(Vector3.forward, effectRot));
				BulletEffect bulletEffect = newEffect.GetComponent<BulletEffect>();
				bulletEffect.transform.parent = hit.collider.transform;
				bulletEffect.PlayEffect();
			}
			else
			{
				Vector3 effectRot = (-transform.forward.normalized * environmentDirBlend) + (hit.normal.normalized * (1 - environmentDirBlend));

				GameObject newEffect = Instantiate(hitEnvironmentEffect, hit.point, Quaternion.FromToRotation(Vector3.forward, effectRot));
				BulletEffect bulletEffect = newEffect.GetComponent<BulletEffect>();
				bulletEffect.transform.parent = hit.collider.transform;
				bulletEffect.PlayEffect();
			}
		}
	}
}
