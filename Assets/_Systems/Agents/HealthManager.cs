  using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthManager : MonoBehaviour, IDamageable
{
	[SerializeField] float maxHealth;
	[SerializeField] float currentHealth;
	[SerializeField] List<Transform> killables = new List<Transform>();
	[SerializeField] RagdollGibForceManager ragdollGibForceManager;

	[Header("Cheats")]
	[SerializeField] bool hasInfiniteHealth = false;

	List<IKillable> Ikillables = new List<IKillable>();

	public delegate void HealthUpdate();
	public event HealthUpdate OnHealthChange;
	public event HealthUpdate OnDamage;

	void Awake()
	{
		currentHealth = maxHealth;
		UpdateHealth();
	}
	
	void Start()
	{
		foreach(Transform t in killables)
		{
			IKillable newKillable = t.GetComponent<IKillable>();
			if (newKillable != null)
			{
				Ikillables.Add(newKillable);
			}
		}
		Hitbox[] hitboxes = GetComponentsInChildren<Hitbox>();
		foreach(Hitbox hitbox in hitboxes)
		{
			hitbox.SetIDamageable(this);
		}
	}

	public void TakeDamage(float damage)
	{
		if(hasInfiniteHealth)
		{
			return;
		}
		Damage();
		currentHealth -= damage;
		if(currentHealth <= 0)
		{
			Kill();
		}
		UpdateHealth();
		
	}
	
	public void TakeDamage(float damage, Vector3 position, Vector3 direction, float force, Rigidbody body)
	{
		if (hasInfiniteHealth)
		{
			return;
		}
		currentHealth -= damage;
		ragdollGibForceManager.SetGibForce(position, direction, force, body);
		if (currentHealth <= 0)
		{
			Kill();
			ragdollGibForceManager.DoGibForce();
		}
		
		UpdateHealth();

		
	}
	

	public void Kill()
	{
		UpdateHealth();
		foreach (IKillable killable in Ikillables)
		{
			killable.Kill();
		}
	}

	public void Heal(float health)
	{
		currentHealth += health;
		UpdateHealth();
	}

	public float GetCurrentHealth()
	{
		return currentHealth;
	}

	void UpdateHealth()
	{
		if(OnHealthChange != null)
		{
			OnHealthChange();
		}
	}

	public void Damage()
	{
		if (OnDamage != null)
		{
			OnDamage();
		}
	}

	public float GetMaxHealth()
	{
		return maxHealth;
	}

	public float GetCurrentHealthPercentage()
	{
		return currentHealth / maxHealth;
	}
}
