using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunsmithInventoryTab : MonoBehaviour
{
    [SerializeField] PartType partType;

    public PartType GetPartType()
    {
        return partType;
    }

    public void SetTabType(PartType newType)
    {
        partType = newType;
    }
}
