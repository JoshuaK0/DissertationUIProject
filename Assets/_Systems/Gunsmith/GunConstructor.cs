using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class GunConstructor : MonoBehaviour
{
	[SerializeField] GunsmithSaveLoad saveLoader;
	[SerializeField] Transform parent;
	void Start()
	{
		List<GunsmithPart> parts = new List<GunsmithPart>();
		foreach (GameObject gunPart in saveLoader.GetPrefabsFromSave(saveLoader.LoadGunsmithGunSave("S")))
		{
			GunsmithPart newPart = Instantiate(gunPart, parent).GetComponent<GunsmithPart>();
			parts.Add(newPart);
			
		}
		foreach (GunsmithPart currentPart in parts)
		{
			foreach (GunsmithAttatchmentPoint attatchmentPoint in currentPart.GetAttatchmentPoints())
			{
				attatchmentPoint.SetOwnerPart(currentPart);
			}
		}
		GunsmithPart receiver = null;
		foreach (GunsmithPart currentPart in parts)
		{
			PartType partType = currentPart.GetPartType();
			if (partType == PartType.Receiver)
			{
				receiver = currentPart;
			}
			List<GunsmithAttatchmentPoint> attatchmentPoints = currentPart.GetAttatchmentPoints();
			foreach (GunsmithPart part in parts)
			{
				foreach (GunsmithAttatchmentPoint attatchToPoint in part.GetAttatchmentPoints())
				{
					if (attatchToPoint.GetAttatchesToType() == partType)
					{
						foreach (GunsmithAttatchmentPoint attatchFromPoint in attatchmentPoints)
						{
							if (attatchFromPoint.GetAttatchesToType() == attatchToPoint.GetOwnerPart().GetPartType())
							{
								attatchFromPoint.SetAttatchmentPoint(attatchToPoint);

								if (partType != PartType.Receiver)
								{
									if (partType != PartType.Barrel || part.GetPartType() != PartType.Muzzle)
									{
										Vector3 targetPos = attatchToPoint.transform.localPosition + attatchToPoint.GetOwnerPart().transform.localPosition - attatchFromPoint.transform.localPosition;
										currentPart.transform.localPosition = targetPos;
										currentPart.transform.localRotation = attatchToPoint.transform.localRotation;
									}
								}
								else
								{
									currentPart.transform.localPosition = Vector3.zero;
								}
							}
						}
					}
				}
			}
		}
		

		GameObject weaponRoot = new GameObject("WeaponRoot");
		weaponRoot.transform.SetParent(null);
		transform.SetParent(null);
		transform.eulerAngles = Vector3.right * 90f;
		transform.position = Vector3.zero;
		weaponRoot.transform.position = receiver.gameObject.GetComponent<RecieverData>().GetGripSocket().position;
		transform.SetParent(weaponRoot.transform);
		weaponRoot.transform.SetParent(FindObjectOfType<GripSocket>().socket);
		weaponRoot.transform.localPosition = Vector3.zero;
		weaponRoot.transform.localRotation = Quaternion.identity;
		
	}
}
