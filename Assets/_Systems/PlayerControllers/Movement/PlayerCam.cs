using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCam : MonoBehaviour
{
    float tilt;
    float fov = 90;

    [SerializeField] float tiltSpeed = 10;
    [SerializeField] float fovSpeed = 10;

    [SerializeField] Camera playerCam;
    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void Update()
    {
        playerCam.fieldOfView = Mathf.Lerp(playerCam.fieldOfView, fov, fovSpeed * Time.deltaTime);
        transform.localEulerAngles = new Vector3(transform.localEulerAngles.x, transform.localEulerAngles.y, Mathf.LerpAngle(transform.localEulerAngles.z, tilt, tiltSpeed * Time.deltaTime));
    }

    public void DoFov(float endValue)
    {
        fov = endValue;
    }

    public void DoTilt(float zTilt)
    {
        tilt = zTilt;
    }
}