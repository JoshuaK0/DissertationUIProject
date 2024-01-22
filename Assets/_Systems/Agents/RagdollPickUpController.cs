using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RagdollPickUpController : MonoBehaviour
{
	[SerializeField] Transform playerCamera;
	[SerializeField] LayerMask layerMask;

	bool isPickUp;

	Transform ragdollPickUp;

	RagdollManager ragdollManager;

	void Update()
	{
		if(Input.GetKeyDown(KeyCode.G))
		{
			if(!isPickUp)
			{
				RaycastHit hit;
				if (Physics.Raycast(playerCamera.position, playerCamera.forward, out hit, 5f, layerMask, QueryTriggerInteraction.Collide))
				{
					ragdollManager = hit.collider.transform.GetComponent<RagdollManager>();
					ragdollManager?.DisableRagdoll();
					ragdollPickUp = ragdollManager.GetPickUpTransform();

					isPickUp = !isPickUp;
				}
			}
			else
			{

				ragdollManager?.EnableRagdoll();

				isPickUp = false;
				ragdollPickUp = null;
				ragdollManager = null;

			}
			
		}

		if(isPickUp)
		{
			ragdollPickUp.transform.position = playerCamera.position + playerCamera.forward * 2f;
		}
	}
}
