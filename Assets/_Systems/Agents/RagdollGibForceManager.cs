using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RagdollGibForceManager : MonoBehaviour
{
	Rigidbody gibForceHitbox;

	Vector3 gibForceDirection;
	Vector3 gibForcePosition;
	float gibForceAmount;

	public void SetGibForce(Vector3 posiiton, Vector3 direction, float force, Rigidbody hitbox)
	{
		gibForcePosition = posiiton;
		gibForceDirection = direction;
		gibForceHitbox = hitbox;
		gibForceAmount = force;
	}

	public void DoGibForce()
	{
		gibForceHitbox.AddForceAtPosition(gibForceDirection * gibForceAmount, gibForcePosition, ForceMode.VelocityChange);
	}
}
