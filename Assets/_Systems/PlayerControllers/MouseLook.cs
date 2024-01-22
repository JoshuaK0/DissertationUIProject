using UnityEngine;

public class MouseLook : MonoBehaviour
{
	public Rigidbody playerRigidbody;
	public Transform cameraPivot;
	public Transform cameraTransform;
	public float mouseSensitivity;

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
		if(isRigidbody)
		{
			
			Quaternion horizontalRotation = Quaternion.Euler(0f, mouseX, 0f);
			playerRigidbody.MoveRotation(playerRigidbody.rotation * horizontalRotation);
		}
		
	}
}