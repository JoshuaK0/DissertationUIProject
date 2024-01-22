using InfimaGames.Animated.ModernGuns;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetSprinting : FSMBehaviour
{
	[SerializeField] WeaponAnimator weaponAnimator;
	[SerializeField] WeaponAnimator cameraAnimator;
	public override void EnterBehaviour()
	{
		weaponAnimator.SetSprinting(true);
		cameraAnimator.SetSprinting(true);
	}

	public override void ExitBehaviour()
	{
		weaponAnimator.SetSprinting(false);
		cameraAnimator.SetSprinting(false);
	}
}
