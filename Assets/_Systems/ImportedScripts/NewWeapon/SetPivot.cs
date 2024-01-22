using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetPivot : MonoBehaviour
{
	[SerializeField] Transform pivotPoint;
	[SerializeField] Transform transformToPivot;

	void Awake()
	{
		Vector3 priorPosition = pivotPoint.localPosition;
		pivotPoint.SetParent(transformToPivot.parent);
		pivotPoint.localEulerAngles = Vector3.zero;
		GameObject pivotSocket = new GameObject("Pivot Socket");
		pivotSocket.transform.parent = pivotPoint.transform;
		pivotSocket.transform.localPosition = priorPosition;
		transformToPivot.SetParent(pivotSocket.transform);
		transformToPivot.localPosition = Vector3.zero;
	}
}
