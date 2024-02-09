using UnityEngine;

public class AmmoTracers : MonoBehaviour
{
	[SerializeField] private GameObject moderateAmmoTracers;
	[SerializeField] private GameObject lowAmmoTracers;
	[SerializeField] GunController gunController;
	private GunStats gunStats;
	private Transform muzzlePos;

	private void Awake()
	{
		// Initialize references if needed, for example:
		gunStats = gunController.GetGunStats();
		muzzlePos = gunController.GetMuzzlePos();
	}

	private void OnEnable()
	{
		// Subscribe to the OnShotFired event
		GunController gunController = FindObjectOfType<GunController>(); // Consider a more specific way to get the GunController if needed
		if (gunController != null)
		{
			gunController.OnShotFired += HandleShotFired;
		}
	}

	private void OnDisable()
	{
		// Unsubscribe from the OnShotFired event
		GunController gunController = FindObjectOfType<GunController>();
		if (gunController != null)
		{
			gunController.OnShotFired -= HandleShotFired;
		}
	}

	private void HandleShotFired(int currentAmmo, Vector3 bulletDirection)
	{
		TrySpawnTracer(currentAmmo, bulletDirection);
	}

	public void TrySpawnTracer(int currentAmmo, Vector3 bulletDirection)
	{
		// Assuming gunStats and muzzlePos are set up correctly
		if (gunStats == null || muzzlePos == null) return;

		GameObject tracerPrefab = null;

		// Determine which tracer to spawn based on current ammo
		if ((float)currentAmmo / gunStats.magazineSize <= 0.33f)
		{
			tracerPrefab = lowAmmoTracers;
		}
		else if ((float)currentAmmo / gunStats.magazineSize <= 0.66f && currentAmmo % 2 == 0)
		{
			tracerPrefab = moderateAmmoTracers;
		}

		// Spawn the tracer
		if (tracerPrefab != null)
		{
			GameObject tracer = Instantiate(tracerPrefab, muzzlePos.position, Quaternion.LookRotation(bulletDirection));
			IProjectileTraceable traceable = tracer.GetComponent<IProjectileTraceable>();
			traceable.InitProjectileTracer(bulletDirection);
		}
	}
}
