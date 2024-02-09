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

    [SerializeField] float leanAngle;
	[SerializeField] float verticalLeanDistance;
	[SerializeField] float viewmodelLeanAngle;
	[SerializeField] float leanSpeed;
	[SerializeField] float viewmodelLeanSpeed;
	[SerializeField] Transform viewmodelLeanPivotTarget;
    [SerializeField] Transform leanPivotTarget;

    Transform leanPivotPoint;

	float currentLeanAmount;
    float currentLeanRot;
    float currentViewmodelLeanRot;
	float currentViewmodelLeanAmount;

	bool isReloading;
	public delegate void ShotFiredEventHandler(int currentAmmo, Vector3 bulletDirection);

	// Define the event based on the delegate
	public event ShotFiredEventHandler OnShotFired;
	public Transform GetMuzzlePos()
    {
		return muzzlePos;
	}

	public override void Start()
    {
		leanPivotPoint = new GameObject("Lean Pivot Point").transform;
		leanPivotPoint.parent = leanPivotTarget.parent;
		leanPivotPoint.localPosition = leanPivotTarget.localPosition;
        leanPivotPoint.localRotation = leanPivotTarget.localRotation;
        leanPivotPoint.localPosition += -Vector3.up * verticalLeanDistance;
		leanPivotTarget.parent = leanPivotPoint;
		base.Start();
        
		gunSwayVisuals.parent = ADSSwayPivot;
		gunSwayVisuals.localPosition = -ADSSwayPivot.localPosition;
		gunSwayVisuals.localEulerAngles = Vector3.zero;
        
		currentAmmo = gunStats.magazineSize;
        startFOV = playerCameraComponent.fieldOfView;
		viewmodelStartFOV = viewmodelCamera.fieldOfView;
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
		if (Input.GetKeyDown(KeyCode.Q))
        {
            if(currentLeanAmount == leanAngle)
            {
				currentLeanAmount = 0;
                currentViewmodelLeanAmount = 0;
			}
            else if (currentLeanAmount == 0)
            {
				currentLeanAmount = leanAngle;
                currentViewmodelLeanAmount = viewmodelLeanAngle;
			}
            else if (currentLeanAmount == -leanAngle)
            {
				currentLeanAmount = 0;
				currentViewmodelLeanAmount = 0;
			}
		}
		if (Input.GetKeyDown(KeyCode.E))
		{
			if (currentLeanAmount == -leanAngle)
			{
				currentLeanAmount = 0;
                currentViewmodelLeanAmount = 0;
			}
            else if (currentLeanAmount == 0)
            {
			    currentLeanAmount = -leanAngle;
                currentViewmodelLeanAmount = -viewmodelLeanAngle;
			}

			else if (currentLeanAmount == leanAngle)
			{
				currentLeanAmount = 0;
				currentViewmodelLeanAmount = 0;
			}
		}

        DoLean();
        
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

    void DoLean()
    {
		currentLeanRot = Mathf.LerpAngle(currentLeanRot, currentLeanAmount, leanSpeed * Time.deltaTime);

		if (Mathf.Abs(currentLeanRot - currentLeanAmount) <= 1)
        {
			currentLeanRot = currentLeanAmount;
		}
        else if (Mathf.Abs(currentLeanRot) <= 1)
        {
			currentLeanRot = 0f;
        }
		
        currentViewmodelLeanRot = Mathf.LerpAngle(currentViewmodelLeanRot, currentViewmodelLeanAmount, viewmodelLeanSpeed * Time.deltaTime);
		if (Mathf.Abs(currentViewmodelLeanRot - currentViewmodelLeanAmount) <= 1)
		{
			currentViewmodelLeanRot = currentViewmodelLeanAmount;
		}
		else if (Mathf.Abs(currentViewmodelLeanRot) <= 1)
		{
			currentViewmodelLeanRot = 0f;
		}

		leanPivotPoint.localEulerAngles = new Vector3(0, 0, currentLeanRot);
        viewmodelLeanPivotTarget.localEulerAngles = Vector3.forward * currentViewmodelLeanRot;
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
			ADSSensitivity = mouseLook.GetBaseSensitivity() * (gunStats.ADSFOV / startFOV);
			mouseLook.SetModifiedSensitivity(ADSSensitivity);
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
			mouseLook.SetModifiedSensitivity(mouseLook.GetBaseSensitivity());
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
        spreadCrosshair.localScale = Vector3.one * Mathf.Max(inaccuracyPercent * spreadCrosshairScale, 1);
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

		OnShotFired?.Invoke(currentAmmo, newBullet.transform.forward);

		if (ammoCountTracers)
        {
            if((float)currentAmmo/ (float)GetMaxAmmo() <= 0.33f)
            {
                GameObject newBulletTracer = Instantiate(lowAmmoTracers, muzzlePos.position, Quaternion.LookRotation(newBullet.transform.forward));
				IProjectileTraceable tracer = newBulletTracer.GetComponent<IProjectileTraceable>();
                //tracer.InitProjectileTracer();
			}
			else if ((float)currentAmmo / (float)GetMaxAmmo() <= 0.66f)
            {
                if((float)currentAmmo % 2 == 0)
                {
					GameObject newBulletTracer = Instantiate(moderateAmmoTracers, muzzlePos.position, Quaternion.LookRotation(newBullet.transform.forward));
					IProjectileTraceable tracer = newBulletTracer.GetComponent<IProjectileTraceable>();
					//tracer.InitProjectileTracer();
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
