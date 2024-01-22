using UnityEngine;

public class WeaponSway : MonoBehaviour
{
    public float drag =2.5f;
    public float dragThreshold = -5f;
    public float smooth = 5;
    public Vector2 maxDrag;

    private Quaternion localRotation;
    Vector3 localPosition;

    [SerializeField] Transform gunVIsuals;
    [SerializeField] Transform pivotPoint;
	[SerializeField] HarmonicSpringVector3 swayRotationSpring;
    [SerializeField] HarmonicSpringVector3 swayPositionSpring;

    [SerializeField] float highSwayThreshold;
    [SerializeField] float lowSwayThreshold;
    [SerializeField] float positionFactor;

    [SerializeField] float tiltFactor;
    [SerializeField] float maxPosition;

	bool usingSpring;

	void Start()
    {
        localRotation = pivotPoint.localRotation;
        localPosition = pivotPoint.localPosition;

        //gunVIsuals.parent = pivotPoint;
    }
    
    void Update()
    {
        float yRot = Mathf.Clamp((Input.GetAxis("Mouse Y")) * drag, -maxDrag.y, maxDrag.y);
        float xRot = Mathf.Clamp(-(Input.GetAxis("Mouse X")) * drag, -maxDrag.x, maxDrag.x);
        float yPos = Mathf.Clamp((Input.GetAxis("Mouse Y")) * positionFactor, -maxPosition, maxPosition);
        float xPos = Mathf.Clamp(-(Input.GetAxis("Mouse X")) * positionFactor, -maxPosition, maxPosition);
        
		Vector2 swayVector = new Vector2(xRot, yRot);

        if(!usingSpring)
        {
            if(swayVector.magnitude > highSwayThreshold)
            {
                usingSpring = true;
                swayRotationSpring.SetValue(pivotPoint.localEulerAngles);
                swayPositionSpring.SetValue(pivotPoint.localPosition);
				return;
            }
            else
            {
				Quaternion newRotation = Quaternion.Euler(localRotation.x + yRot, localRotation.y + xRot, (localRotation.z + xRot) * tiltFactor);
                Vector3 newPosition = new Vector3(localPosition.x + xPos, localPosition.y + yPos, localPosition.z);

				pivotPoint.localRotation = Quaternion.Slerp(pivotPoint.localRotation, newRotation, (Time.deltaTime * smooth));
                pivotPoint.localPosition = Vector3.Slerp(pivotPoint.localPosition, newPosition, Time.deltaTime * smooth);

			}

		}
        else if(usingSpring)
        {
			if (Vector3.Distance(pivotPoint.localEulerAngles, swayRotationSpring.GetTarget()) < lowSwayThreshold)
            {
                usingSpring = false;
                return;
            }
            else
            {
                
				swayRotationSpring.SetTarget(new Vector3(localRotation.x + yRot, localRotation.y + xRot, (localRotation.z + xRot) * tiltFactor));
                swayPositionSpring.SetTarget(new Vector3(localPosition.x + xPos, localPosition.y + yPos, localPosition.z));
				pivotPoint.localEulerAngles = swayRotationSpring.GetValue();
                pivotPoint.localPosition = swayPositionSpring.GetValue();
			}
		    
		}
	}
}