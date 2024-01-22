using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class GunsmithAttatchmentPoint : MonoBehaviour
{
    [SerializeField] string attatchesToType;
    [SerializeField] GunsmithPart ownerPart;

    [SerializeField] GunsmithAttatchmentPoint attatchedTo;

    public string GetAttatchesToType()
    {
        return attatchesToType;
    }

    public void SetOwnerPart(GunsmithPart newOwner)
    {
        ownerPart = newOwner;
    }

    public GunsmithPart GetOwnerPart()
    {
        return ownerPart;
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawSphere(transform.position, 0.02f);
    }

    public void SetAttatchmentPoint(GunsmithAttatchmentPoint newPoint)
    {
        attatchedTo = newPoint;
    }

    public void FillMissingParts()
    {

        if (attatchedTo == null)
        {
            if(ownerPart.GetPartType() == "Reciever" || ownerPart.GetPartType() == "Barrel" || ownerPart.GetPartType() == "Stock" || ownerPart.GetPartType() == "Magazine")
            {
                GameObject partToCreate = null;
                foreach (GunsmithPartTypeCollection partTypeCollection in GunsmithManager.Instance().GetPartDatabase().GetPartTypeCollections())
                {
                    if (partTypeCollection.GetPartType() == attatchesToType)
                    {
                        partToCreate = partTypeCollection.GetParts()[0];
                    }
                }
                if (partToCreate != null)
                {
                    GunsmithManager.Instance().AddMissingPart(partToCreate);
                }
            }
            else
            {
                GunsmithManager.Instance().RemovePart(ownerPart);
                ownerPart.DestroyGunsmithPart();
            }
            
        }
    }
}