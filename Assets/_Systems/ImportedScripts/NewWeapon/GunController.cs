using CameraShake;
using InfimaGames.Animated.ModernGuns;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunController : FiniteStateMachine
{
    [Header("References")]
    [SerializeField] GunInputHandler inputHandler;
    [SerializeField] GunStats gunStats;
    [SerializeField] PlayAudioShot fireAudioShot;
    [SerializeField] Transform viewmodel;
    [SerializeField] ViewmodelAnimator viewmodelAnimator;
    [SerializeField] ProjectileSuspicion projectileSuspicion;

    [SerializeField] Transform cameraRecoilHolder;
    [SerializeField] Transform bulletOrientation;
    [SerializeField] Transform muzzlePos;
    [SerializeField] Rigidbody rb;
    [SerializeField] Transform playerCamera;
    [SerializeField] HarmonicSpringVector3 ADSSpring;
    [SerializeField] WeaponAnimator weaponAnimator;
	[SerializeField] WeaponAnimator cameraAnimator;
	[SerializeField] RectTransform crosshair;
    [SerializeField] RectTransform spreadCrosshair;
    [SerializeField] RectTransform ADSReticle;
    [SerializeField] float spreadCrosshairScale;
    [SerializeField] Camera playerCameraComponent;
    [SerializeField] Camera viewmodelCamera;
    [SerializeField] MouseLook mouseLook;

    float startMouseSensitivity;
    float ADSSensitivity;
    
    [Header("Gun States")]
    [SerializeField] FSMState sprintState;
    [SerializeField] FSMState reloadState;

    [SerializeField] Transform gunSwayVisuals;
    [SerializeField] Transform defaultSwayPivot;
    [SerializeField] Transform ADSSwayPivot;
    

    int currentAmmo;
    float fireRateTimer;

    float startFOV;
    float viewmodelStartFOV;

    bool isADS = false;
    float currentRecoilIndex;
    Vector3 currentRecoil;
    float currentPrecision;
    float inaccuracyPercent;

	[Header("Camera stats")]
	[SerializeField] float cameraRecoilSmoothing;

    [SerializeField] bool ammoCountTracers;
    [SerializeField] GameObject moderateAmmoTracers;
    [SerializeField] GameObject lowAmmoTracers;

	bool isReloading;


	public override void Start()
    {
        base.Start();
        
		gunSwayVisuals.parent = ADSSwayPivot;
		gunSwayVisuals.localPosition = -ADSSwayPivot.localPosition;
		gunSwayVisuals.localEulerAngles = Vector3.zero;
        
		currentAmmo = gunStats.magazineSize;
        startFOV = playerCameraComponent.fieldOfView;
		viewmodelStartFOV = viewmodelCamera.fieldOfView;
        startMouseSensitivity = mouseLook.mouseSensitivity;
        ADSSensitivity = startMouseSensitivity * (gunStats.ADSFOV / startFOV);
	}

	public int GetCurrentAmmo()
	{
		return currentAmmo;
	}

	public GunStats GetGunStats()
	{
		return gunStats;
	}

    public int GetMaxAmmo()
    {
		return gunStats.magazineSize;
	}
	public override void Update()
    {
        
        viewmodel.localPosition = ADSSpring.GetValue();
        base.Update();

		if (isADS)
		{
			cameraRecoilHolder.localRotation = Quaternion.Lerp(Quaternion.Euler(cameraRecoilHolder.localEulerAngles), Quaternion.Euler(currentRecoil), cameraRecoilSmoothing * Time.deltaTime);
			bulletOrientation.localRotation = Quaternion.Euler(cameraRecoilHolder.localEulerAngles);
		}
        else
        {
			cameraRecoilHolder.localRotation = Quaternion.Lerp(Quaternion.Euler(cameraRecoilHolder.localEulerAngles), Quaternion.Euler(currentRecoil), cameraRecoilSmoothing * Time.deltaTime);
			bulletOrientation.localRotation = Quaternion.Euler(cameraRecoilHolder.localEulerAngles * 2);
		}

		if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            //ChangeState(sprintState);
        }
        if(Input.GetKeyDown(KeyCode.R))
        {
            weaponAnimator.OnReloadDown();
			isReloading =   true;

			Invoke("Reload", gunStats.reloadTime);
            currentAmmo = 0;
            
            //ChangeState(reloadState);
        }

        if (currentAmmo > 0 && fireRateTimer <= 0)
        {
            if (gunStats.fireMode == GunFireMode.Semi)
            {
                if (Input.GetMouseButtonDown(0))
                {
                    Fire();
                }
            }
            else if (gunStats.fireMode == GunFireMode.Auto)
            {
                if (Input.GetMouseButton(0))
                {
                    Fire();
                }
            }
        }
		if (!Input.GetMouseButton(0) && fireRateTimer <= 0)
		{
			currentRecoilIndex = Mathf.Lerp(currentRecoilIndex, 0, gunStats.recoilCooldownSpeed * Time.deltaTime);
		}

		if (fireRateTimer > 0)
        {
            
            fireRateTimer -= Time.deltaTime;
        }
        if(currentRecoil != Vector3.zero)
        {
            currentRecoil = Vector3.Lerp(currentRecoil, Vector3.zero, gunStats.recoilCooldownSpeed * Time.deltaTime);
        }

        if (inputHandler.holdADS)
        {
            isADS = Input.GetMouseButton(1);
        }
        else if (!inputHandler.holdADS)
        {
            if (Input.GetMouseButtonDown(1))
            {
                isADS = !isADS;
				UpdateAimPos();
			}
        }
        UpdateFOV();
        UpdatePrecision();
    }

    void Reload()
    {
		currentAmmo = gunStats.magazineSize;
        isReloading = false;
	}

    public float GetShotTimer()
    {
		return fireRateTimer;
	}

    public bool IsReloading()
    {
        return isReloading;
    }

    void UpdateFOV()
    {
        if (isADS)
        {
            mouseLook.mouseSensitivity = ADSSensitivity;
			if (Mathf.Abs(gunStats.ADSFOV - playerCameraComponent.fieldOfView) > 0.1)
            {
                playerCameraComponent.fieldOfView = Mathf.Lerp(playerCameraComponent.fieldOfView, gunStats.ADSFOV, gunStats.FOVSpeed * Time.deltaTime);
            }
            else
            {
                playerCameraComponent.fieldOfView = gunStats.ADSFOV;
            }
			if (Mathf.Abs(gunStats.viewmodelADSFOV - viewmodelCamera.fieldOfView) > 0.1)
			{
				viewmodelCamera.fieldOfView = Mathf.Lerp(viewmodelCamera.fieldOfView, gunStats.viewmodelADSFOV, gunStats.viewmodelFOVSpeed * Time.deltaTime);
			}
			else
			{
				viewmodelCamera.fieldOfView = gunStats.viewmodelADSFOV;
			}
		}
        else
        {
			mouseLook.mouseSensitivity = startMouseSensitivity;
			if (Mathf.Abs(startFOV - playerCameraComponent.fieldOfView) > 0.1)
			{
				playerCameraComponent.fieldOfView = Mathf.Lerp(playerCameraComponent.fieldOfView, startFOV, gunStats.FOVSpeed * Time.deltaTime);
			}
			else
			{
				playerCameraComponent.fieldOfView = startFOV;
			}
			if (Mathf.Abs(startFOV - viewmodelCamera.fieldOfView) > 0.1)
			{
				viewmodelCamera.fieldOfView = Mathf.Lerp(viewmodelCamera.fieldOfView, viewmodelStartFOV, gunStats.viewmodelFOVSpeed * Time.deltaTime);
			}
			else
			{
				viewmodelCamera.fieldOfView = viewmodelStartFOV;
			}
		}
    }

    void UpdatePrecision()
    {
        inaccuracyPercent = Mathf.Clamp01(rb.velocity.magnitude / gunStats.precisionSpeedThreshold);
		currentPrecision = Mathf.Lerp(gunStats.restPrecision, gunStats.movingPrecision, inaccuracyPercent);
        spreadCrosshair.localScale = Vector3.one * inaccuracyPercent * spreadCrosshairScale;
	}
    void Fire()
    {
        weaponAnimator.OnFire();
		GameObject newBullet = Instantiate(gunStats.bulletProjectilePrefab, bulletOrientation.position, bulletOrientation.rotation);

		CameraShaker.Shake(new BounceShake(gunStats.shakeParams));
		Vector3 randomInaccuracy = Vector3.zero;
		randomInaccuracy.x = Random.Range(-currentPrecision, currentPrecision);
		randomInaccuracy.y = Random.Range(-currentPrecision, currentPrecision);
        newBullet.transform.eulerAngles += randomInaccuracy;

		if(ammoCountTracers)
        {
            if((float)currentAmmo/ (float)GetMaxAmmo() <= 0.33f)
            {
                GameObject newBulletTracer = Instantiate(lowAmmoTracers, muzzlePos.position, Quaternion.LookRotation(newBullet.transform.forward));
				IProjectileTraceable tracer = newBulletTracer.GetComponent<IProjectileTraceable>();
                tracer.InitProjectileTracer();
			}
			else if ((float)currentAmmo / (float)GetMaxAmmo() <= 0.66f)
            {
                if((float)currentAmmo % 2 == 0)
                {
					GameObject newBulletTracer = Instantiate(moderateAmmoTracers, muzzlePos.position, Quaternion.LookRotation(newBullet.transform.forward));
					IProjectileTraceable tracer = newBulletTracer.GetComponent<IProjectileTraceable>();
					tracer.InitProjectileTracer();
				}
            }

		}


		DamagePerBodypartMap damagePerBodypartMap = new DamagePerBodypartMap(gunStats.headShotDamage, gunStats.torsoDamage, gunStats.limbsDamage);
        newBullet.GetComponent<IDamageInflictable>().InitDamageInfo(damagePerBodypartMap);

		projectileSuspicion.DoProjectile(bulletOrientation.position, newBullet.transform.forward);

		fireRateTimer = gunStats.fireRate;
        currentAmmo -= 1;
        currentRecoilIndex += 1;
        viewmodelAnimator.AddMicroViewmodelRotation(gunStats.viewmodelPositionalRecoil, gunStats.viewmodelRotationalRecoil);

		float macroRecoilY = gunStats.recoilCurve.Evaluate(Mathf.Round(currentRecoilIndex) / (float)gunStats.magazineSize);

		if (!isADS)
        {
			viewmodelAnimator.AddMacroViewmodelRotation(-macroRecoilY * Vector3.right * gunStats.viewmodelMacroRecoilScale);
		}

        currentRecoil += new Vector3(-macroRecoilY * gunStats.recoilScale, 0f, 0f);
        cameraRecoilHolder.localRotation = Quaternion.Lerp(Quaternion.Euler(cameraRecoilHolder.localEulerAngles), Quaternion.Euler(currentRecoil), cameraRecoilSmoothing * Time.deltaTime);
		bulletOrientation.localRotation = Quaternion.Euler(cameraRecoilHolder.localEulerAngles * 2);
	
	}
    void UpdateAimPos()
    {
        if(isADS)
        {
			weaponAnimator.SetAiming(true);
			cameraAnimator.SetAiming(true);
			spreadCrosshair.gameObject.SetActive(false);
            crosshair.gameObject.SetActive(false);
			ADSReticle.gameObject.SetActive(true);

			gunSwayVisuals.parent = ADSSwayPivot;
			gunSwayVisuals.localPosition = -ADSSwayPivot.localPosition;
			gunSwayVisuals.localEulerAngles = Vector3.zero;


		}
		else
        {
			weaponAnimator.SetAiming(false);
			cameraAnimator.SetAiming(false);
			spreadCrosshair.gameObject.SetActive(true);
			crosshair.gameObject.SetActive(true);
			ADSReticle.gameObject.SetActive(false);
            

			gunSwayVisuals.parent = defaultSwayPivot;
			gunSwayVisuals.localPosition = -defaultSwayPivot.localPosition;
            gunSwayVisuals.localEulerAngles = Vector3.zero;
		}
	}
}
