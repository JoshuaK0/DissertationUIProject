using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseLook : MonoBehaviour
{
    public Transform cameraHolder;
    public Transform cameraTransform;
    public float mouseSensitivity;

    private Vector2 rotation;

    // Update is called once per frame
    void Update()
    {
        rotation.y += Input.GetAxis("Mouse X") * mouseSensitivity;
        rotation.x = Mathf.Clamp(rotation.x -= Input.GetAxis("Mouse Y") * mouseSensitivity, -90, 90);
        cameraHolder.eulerAngles = new Vector3(0, rotation.y, 0);
        cameraTransform.localEulerAngles = new Vector3(rotation.x, 0, 0);
    }
}
