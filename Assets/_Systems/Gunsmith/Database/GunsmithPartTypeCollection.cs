using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class GunsmithPartTypeCollection
{
    [SerializeField] string partType;
    [SerializeField] List<GameObject> parts;

    public string GetPartType()
    {
        return partType;
    }

    public List<GameObject> GetParts()
    {
        return parts;
    }
}