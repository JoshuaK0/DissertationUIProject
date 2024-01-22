using UnityEngine;

public class UIBillboard : MonoBehaviour
{
	public Transform playerTransform; // Reference to the player's transform

	void Update()
	{
		if (playerTransform != null)
		{
			// Rotate the object to face the player
			transform.LookAt(playerTransform);
		}
	}
}
