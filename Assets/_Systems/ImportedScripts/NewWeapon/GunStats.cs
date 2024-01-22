using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CameraShake;

public class GunStats : MonoBehaviour
{
    public float fireRate;
    public GunFireMode fireMode;
    public int magazineSize;
    public float restPrecision;
    public float movingPrecision;
    public float precisionChangeSpeed;
    public float precisionSpeedThreshold;
    public float reloadTime;
    public Vector3 shoulderPos;
    public Vector3 ADSPos;
    public AudioClip fireSound;
    public AnimationCurve recoilCurve;
    public float recoilScale;
    public float recoilCooldownSpeed;
    public Vector3 viewmodelPositionalRecoil;
	public Vector3 viewmodelRotationalRecoil;
    public float viewmodelMacroRecoilScale;
    public GameObject bulletProjectilePrefab;
    public BounceShake.Params shakeParams;
    public float viewmodelFOV;
    public float viewmodelADSFOV;
    public float ADSFOV;
    public float viewmodelFOVSpeed;
    public float FOVSpeed;

    public float headShotDamage;
    public float torsoDamage;
    public float limbsDamage;
}
