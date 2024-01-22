using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootAtBehaviour : FSMBehaviour
{
	[SerializeField] Vector2 burstSize;
	[SerializeField] Vector2 burstInterval;
	[SerializeField] float fireRate;
	[SerializeField] float accuracy;
	[SerializeField] float muzzleVelocity;
	[SerializeField] float damage;

	[SerializeField] GameObject projectilePrefab;
	[SerializeField] Transform muzzle;

	[SerializeField] float aimingTolerance;
	[SerializeField] HarmonicSpringVector3 aimSpring;
	[SerializeField] AudioSource gunshotAudioSource;
	[SerializeField] AudioClip shotSound;
	[SerializeField] Vector2 minMaxPitch;

	bool isInBehaviour = false;

	int burstCount;

	float fireTime;

	bool newBurst;

	CombatantFSM combatantFSM;

	public override void EnterBehaviour()
	{
		isInBehaviour = true;
		combatantFSM = fsm.GetComponent<CombatantFSM>();
		StartBurst();
	}

	public override void UpdateBehaviour()
	{
		var lookPos = (combatantFSM.GetTargetLKP() - muzzle.position);
		lookPos = lookPos.normalized;
		var rotation = Quaternion.LookRotation(lookPos);
		aimSpring.SetTarget(rotation.eulerAngles);
		muzzle.eulerAngles = aimSpring.GetValue();
		
		if (burstCount > 0)
		{
			if (fireTime <= 0 && burstCount > 0)
			{
				fireTime = fireRate;
				burstCount -= 1;
				if(burstCount == 0)
				{
					newBurst = true;
				}
				if(GetCurrentInnacuracy() <= aimingTolerance)
				{
					DoShot();
				}
			}
			else
			{
				fireTime -= Time.deltaTime;
			}
		}
		else if(newBurst)
		{
			newBurst = false;
			Invoke("StartBurst", Random.Range(burstInterval.x, burstInterval.y));
		}
	}

	float GetCurrentInnacuracy()
	{
		Vector3 idealAimingDirection = combatantFSM.GetTargetLKP() - muzzle.position;
		return Vector3.Angle(muzzle.forward, idealAimingDirection);
	}

	public void StartBurst()
	{
		burstCount = Random.Range((int)burstSize.x, (int)burstSize.y);
	}
	public void DoShot()
	{
		gunshotAudioSource.pitch = Random.Range(minMaxPitch.x, minMaxPitch.y);
		gunshotAudioSource.PlayOneShot(shotSound);
		Vector3 bulletRotation = ApplyInaccuracy(muzzle.forward, accuracy);
		GameObject newBullet = Instantiate(projectilePrefab, muzzle.position, Quaternion.LookRotation(bulletRotation));
		newBullet.GetComponent<Rigidbody>().AddForce(bulletRotation * muzzleVelocity, ForceMode.VelocityChange);

		RaycastHit hit;

		if (Physics.Raycast(muzzle.position, bulletRotation, out hit, 1000))
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

	public override void ExitBehaviour()
	{
		isInBehaviour = false;
	}

	void Update()
	{
		if(!isInBehaviour)
		{
			aimSpring.SetTarget(Quaternion.LookRotation(transform.parent.forward).eulerAngles);
			muzzle.rotation = Quaternion.LookRotation(transform.parent.forward);
		}
	}
}
