using UnityEngine;
using UnityEngine.UI;

public class GridMagazineUI : MonoBehaviour
{
	public GameObject bulletPrefab;       // Assign your bullet image prefab
	public GunController gun;                       // Reference to your gun script
	public Color midAmmoColor = Color.white; // Color when ammo is more than 2/3
	public Color lowAmmoColor = Color.red;    // Color when ammo is less than 2/3
	public AnimationCurve colorLerpCurve;

	private GridLayoutGroup gridLayoutGroup;
	private int maxAmmo;                  // Maximum ammo capacity

	void Start()
	{
		gridLayoutGroup = GetComponent<GridLayoutGroup>();
		maxAmmo = gun.GetMaxAmmo();       // Replace with your method of getting max ammo
		UpdateAmmoDisplay();
	}

	void Update()
	{
		UpdateAmmoDisplay();
	}

	private void UpdateAmmoDisplay()
	{
		int currentAmmo = gun.GetCurrentAmmo();  // Replace with your method of getting current ammo
		int childCount = gridLayoutGroup.transform.childCount;

		// Add or remove bullets if needed
		while (childCount < currentAmmo)
		{
			Instantiate(bulletPrefab, gridLayoutGroup.transform);
			childCount++;
		}
		while (childCount > currentAmmo)
		{
			Destroy(gridLayoutGroup.transform.GetChild(--childCount).gameObject);
		}

		Color currentColor = Color.white;  // Default color
		if (currentAmmo < maxAmmo * (2f / 3f))
		{
			float lerpFactor = (float)currentAmmo / (maxAmmo * (2f / 3f));
			currentColor = Color.Lerp(lowAmmoColor, midAmmoColor, colorLerpCurve.Evaluate(lerpFactor));
		}

		// Update bullet colors
		for (int i = 0; i < childCount; i++)
		{
			Image bulletImage = gridLayoutGroup.transform.GetChild(i).GetComponent<Image>();
			if (bulletImage != null)
			{
				bulletImage.color = currentColor;
			}
		}
	}
}
