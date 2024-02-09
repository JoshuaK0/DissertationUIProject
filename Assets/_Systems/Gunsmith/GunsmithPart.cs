using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
public class GunsmithPart : MonoBehaviour
{
    [SerializeField] PartType partType;
    [SerializeField] List<GunsmithAttatchmentPoint> attatchmentPoints = new List<GunsmithAttatchmentPoint>();

    [SerializeField] GameObject labelButton;

    public void CreateUI()
    {
        if(GunsmithUIManager.Instance()!= null)
        {
			labelButton = Instantiate(GunsmithUIManager.Instance().GetPartLabelButton(), Vector3.zero, Quaternion.identity, GunsmithUIManager.Instance().GetUICanvas());
			labelButton.GetComponent<GunsmithPartLabel>().InitButton(this, partType);
		}
    }
    public void AttatchParts()
    {
        foreach (GunsmithAttatchmentPoint attatchmentPoint in attatchmentPoints)
        {
            attatchmentPoint.SetOwnerPart(this);
        }
        
        foreach (GunsmithPart part in GunsmithManager.Instance().GetParts())
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
                                if(partType != PartType.Barrel || part.GetPartType() != PartType.Muzzle)
                                {
                                    Vector3 targetPos = attatchToPoint.transform.localPosition + attatchToPoint.GetOwnerPart().transform.localPosition - attatchFromPoint.transform.localPosition;
                                    if(GunsmithManager.Instance().GetPartMoveSpeed() > 0)
                                    {
										transform.localPosition = Vector3.Slerp(transform.localPosition, targetPos, GunsmithManager.Instance().GetPartMoveSpeed() * Time.deltaTime);
									}
                                    else
                                    {
										transform.localPosition = targetPos;
									}
                                    transform.localRotation = attatchToPoint.transform.localRotation;
                                }
                            }
                            else
                            {
                                transform.localPosition = Vector3.zero;
                            }
                        }
                    }
                }
            }
        }
    }

    public void FillInMissingParts()
    {
        foreach (GunsmithAttatchmentPoint part in attatchmentPoints)
        {
            part.FillMissingParts();
        }
    }
    
    public PartType GetPartType()
    {
        return partType;
    }

    public List<GunsmithAttatchmentPoint> GetAttatchmentPoints()
    {
        return attatchmentPoints;
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.white;

        Gizmos.DrawSphere(transform.position, 0.02f);
    }

    public void DestroyGunsmithPart()
    {
        Destroy(labelButton);
        Destroy(gameObject);
    }
}