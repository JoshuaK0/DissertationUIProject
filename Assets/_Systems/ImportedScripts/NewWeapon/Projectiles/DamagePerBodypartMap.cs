using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamagePerBodypartMap
{
	public float headDamage;
	public float torsoDamage;
	public float limbsDamage;

	public DamagePerBodypartMap(float newHeadDamage, float newTorsoDamage, float newLimbsDamage)
	{
		headDamage = newHeadDamage;
		torsoDamage = newTorsoDamage;
		limbsDamage = newLimbsDamage;
	}
}
