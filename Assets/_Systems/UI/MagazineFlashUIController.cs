using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagazineFlashUIController : MonoBehaviour
{
	[SerializeField] FlashWarning flashWarning;
	[SerializeField] GunController gunController;

	void Update()
	{
		if (gunController.IsReloading())
		{
			flashWarning.SetWarningLevel(0);
			return;
		}
		// Get the current magazine capacity
		int magazineCapacity = gunController.GetMaxAmmo();

		// Get the current magazine ammo
		int ammoLeft = gunController.GetCurrentAmmo();

		// Calculate the warning level
		float percentage = 1 - (float)ammoLeft / (float)magazineCapacity;

		float warningLevel = 0;

		if(percentage > (1f/3f))
		{
			warningLevel = 1 - ((float)ammoLeft / ((float)magazineCapacity * (2f / 3f)));
		}

		// Set the warning level
		flashWarning.SetWarningLevel(warningLevel);
	}
}
