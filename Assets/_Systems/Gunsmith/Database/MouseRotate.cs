using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseRotate : MonoBehaviour
{
    public float panSensitivity;
    public float zoomSensitivity;
    public Vector2 zoomMinMax;
    public float smooth;
    public Transform xTransform;
    public Transform yTransform;

    public Transform cameraTransform;

    private Vector3 rotation;
    private float zoomDist;
    private void Update()
    {
        if(Input.GetMouseButton(2))
        {
            DoRotate();
        }
        DoZoom();
    }

    private void DoRotate()
    {
        rotation.y += Input.GetAxis("Mouse X") * panSensitivity;
        rotation.x = Mathf.Clamp(rotation.x -= Input.GetAxis("Mouse Y") * panSensitivity, -90, 90);
        yTransform.localEulerAngles = new Vector3(0, Mathf.LerpAngle(yTransform.localEulerAngles.y, rotation.y, smooth * Time.deltaTime), 0);
        xTransform.localEulerAngles = new Vector3(Mathf.LerpAngle(xTransform.localEulerAngles.x, rotation.x, smooth * Time.deltaTime), 0, 0);
    }

    private void DoZoom()
    {
        zoomDist = Mathf.Clamp(zoomDist += zoomSensitivity * Input.mouseScrollDelta.y, -zoomMinMax.y, -zoomMinMax.x);
        cameraTransform.localPosition = new Vector3(0, 0, Mathf.Lerp(cameraTransform.localPosition.z, zoomDist, smooth * Time.deltaTime));
    }
}
