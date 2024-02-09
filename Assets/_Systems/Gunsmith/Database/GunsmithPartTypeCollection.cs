using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class GunsmithPartTypeCollection
{
    [SerializeField] PartType partType;
    [SerializeField] List<GameObject> parts;

    public PartType GetPartType()
    {
        return partType;
    }

    public List<GameObject> GetParts()
    {
        return parts;
    }
}