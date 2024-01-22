using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Database", menuName = "ScriptableObjects/GunsmithPartDatabase", order = 1)]
public class GunsmithPartDatabase : ScriptableObject
{
    [SerializeField] List<GunsmithPartTypeCollection> parts;

    public List<GunsmithPartTypeCollection> GetPartTypeCollections()
    {
        return parts;
    }
}