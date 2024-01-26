using UnityEngine;

public class MouseLook : MonoBehaviour
{
	public Rigidbody playerRigidbody;
	public Transform cameraPivot;
	public Transform cameraTransform;
	public float mouseSensitivity;

	public Transform rotationPivot; // The predetermined pivot point for rotation

	private float verticalRotation = 0f;

	public bool isRigidbody;

	float mouseY;
	float mouseX;

	void Update()
	{
		// Mouse movement
		mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity;

		// Rotate the camera for up/down look
		verticalRotation -= mouseY;
		verticalRotation = Mathf.Clamp(verticalRotation, -90f, 90f);
		cameraTransform.localEulerAngles = new Vector3(verticalRotation, 0f, 0f);

		// Prepare horizontal rotation for FixedUpdate
		mouseX = Input.GetAxis("Mouse X") * mouseSensitivity;
		if (!isRigidbody)
		{
			cameraPivot.eulerAngles = cameraPivot.eulerAngles + new Vector3(0f, mouseX, 0f);
		}

	}

	void FixedUpdate()
	{
		if (isRigidbody)
		{
			// Calculate rotation around predetermined point
			RotateAroundPoint(playerRigidbody, rotationPivot.position, Quaternion.Euler(0f, mouseX, 0f));
		}
	}

	void RotateAroundPoint(Rigidbody rb, Vector3 point, Quaternion rotation)
	{
		Vector3 direction = rb.position - point; // Direction from pivot to object
		direction = rotation * direction; // Rotate the direction
		rb.MovePosition(point + direction); // Move to the new position
		rb.MoveRotation(rb.rotation * rotation); // Rotate the rigidbody
	}
}
