using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunsmithInventoryTab : MonoBehaviour
{
    [SerializeField] string partType;

    public string GetPartType()
    {
        return partType;
    }

    public void SetTabType(string newType)
    {
        partType = newType;
    }
}
