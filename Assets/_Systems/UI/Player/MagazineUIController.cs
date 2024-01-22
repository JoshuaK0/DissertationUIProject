using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


public class MagazineUIController : MonoBehaviour
{
	[SerializeField] GunController gunController;
	[SerializeField] Slider hazardSlider;
	[SerializeField] TMP_Text ammoCounter;
	[SerializeField] TMP_Text reloadText;
	[SerializeField] Slider whiteSlider;
	[SerializeField] Color fullBackgroundColor;
	[SerializeField] Color emptybackgrounColor;
	[SerializeField] float reloadHazardSlideSpeed;
	[SerializeField] bool showMagSize;
	[SerializeField] bool showBackground;
	[SerializeField] string seperationText;

	GunStats gunStats;
	int maxAmmo;
	int currentAmmo;

	bool startReload = false;
	
	void Start()
	{
		gunStats = gunController.GetGunStats();
		maxAmmo = gunStats.magazineSize;
	}

	void Update()
	{
		currentAmmo = gunController.GetCurrentAmmo();
		
		if (currentAmmo == maxAmmo)
		{
			ammoCounter.gameObject.SetActive(true);

			reloadText.gameObject.SetActive(false);
			whiteSlider.value = 0;

			hazardSlider.value = 0;
			ammoCounter.alpha = 0;
		}
		else if (currentAmmo > 0)
		{
			ammoCounter.gameObject.SetActive(true);

			reloadText.gameObject.SetActive(false);
			//hazardSlider.value = gunController.GetShotTimer()/gunStats.fireRate;
			if (showBackground)
			{
				whiteSlider.value = 1;
			}
			else
			{
				whiteSlider.value = 0;
			}
			hazardSlider.value = 0;
			ammoCounter.alpha = 1;
			if (showMagSize)
			{
				ammoCounter.text = gunController.GetCurrentAmmo().ToString() + seperationText + gunController.GetMaxAmmo().ToString();

			}
			else
			{
				ammoCounter.text = gunController.GetCurrentAmmo().ToString();
			}
		}
		else if (currentAmmo == 0)
		{
			ammoCounter.alpha = 0;
			if(!gunController.IsReloading())
			{
				whiteSlider.value = 0;
			}
			hazardSlider.value = Mathf.Lerp(hazardSlider.value, 1, reloadHazardSlideSpeed * Time.deltaTime);
			ammoCounter.gameObject.SetActive(false);
			
			reloadText.gameObject.SetActive(true);
			
		}
		if(gunController.IsReloading())
		{
			if (startReload == true)
			{
				whiteSlider.value = 0;
				startReload = false;
			}
			whiteSlider.value += (1/gunStats.reloadTime) * Time.deltaTime;
		}
		else
		{
			startReload = true;
		}
	}
}
