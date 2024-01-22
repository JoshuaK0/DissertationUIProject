using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ViewmodelAnimator : MonoBehaviour
{
	
	[SerializeField] Transform macroRecoilHolder;
	[SerializeField] Transform macroRecoilPivot;
	
	[SerializeField] Transform microRecoilHolder;
	[SerializeField] Transform microRecoilPivot;
	[SerializeField] Transform microRecoilTarget;
	[SerializeField] SpringRecoil recoilPositionSpring;
	[SerializeField] SpringRecoil recoilRotationSpring;

	[SerializeField] float macroRecoilSmoothing;

	void Awake()
	{
		macroRecoilPivot.SetParent(macroRecoilHolder.parent);
		macroRecoilHolder.SetParent(macroRecoilPivot);

		microRecoilTarget.SetParent(microRecoilPivot);
	}

	void Update()
	{
		microRecoilHolder.localPosition = recoilPositionSpring.GetValue();

		microRecoilHolder.localRotation = Quaternion.Euler(recoilRotationSpring.GetValue());

		macroRecoilPivot.localEulerAngles = new Vector3(
			Mathf.LerpAngle(macroRecoilPivot.localEulerAngles.x, 0, macroRecoilSmoothing * Time.deltaTime),
			macroRecoilPivot.localEulerAngles.y,
			macroRecoilPivot.localEulerAngles.z);
	}

	public void AddMicroViewmodelRotation(Vector3 positionalRecoil, Vector3 rotationalRecoil)
	{
		recoilPositionSpring.SetValue(positionalRecoil);
		Vector3 randomRotationalRecoil = new Vector3(rotationalRecoil.x, Random.Range(-rotationalRecoil.y, rotationalRecoil.y), Random.Range(-rotationalRecoil.z, rotationalRecoil.z));
		recoilRotationSpring.SetValue(randomRotationalRecoil);
	}

	public void AddMacroViewmodelRotation(Vector3 macroRecoil)
	{
		macroRecoilPivot.localEulerAngles += macroRecoil;
	}
}
