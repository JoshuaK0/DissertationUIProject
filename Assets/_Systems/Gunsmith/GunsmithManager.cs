using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GunsmithManager : MonoBehaviour
{
    public static GunsmithManager instance;

    [SerializeField]
    Dictionary<GunsmithPart, GameObject> parts = new Dictionary<GunsmithPart, GameObject>();

    [SerializeField] List<GameObject> partPrefabs;

	[SerializeField] GunsmithPartDatabase partDatabase;

    [SerializeField] Transform partParent;

    [SerializeField] GunsmithPart selectedPart;

    [SerializeField] float partMoveSpeed;

    public float GetPartMoveSpeed()
    {
        return partMoveSpeed;
    }

    public void SelectPart(GunsmithPart part)
    {
        selectedPart = part;
    }

    public List<GameObject> GetPrefabs()
    {
		return partPrefabs;
	}

    public static GunsmithManager Instance()
    {
        return instance;
    }

    void Awake()
    {
        instance = this;
		AddMissingPart(partDatabase.GetPartTypeCollections()[0].GetParts()[0]);
    }

    public List<GunsmithPart> GetParts()
    {
		return parts.Keys.ToList();
	}

    public void AddPartThroughButton(GameObject newPart)
    {
        Vector3 instantiatePos = Vector3.zero;
		PartType partType = newPart.GetComponent<GunsmithPart>().GetPartType();
        foreach (GunsmithPart part in parts.Keys)
        {
            if (part.GetPartType() == partType)
            {
                selectedPart = part;
                instantiatePos = part.transform.position;
            }
        }

        if (selectedPart != null)
        {
            
            selectedPart.DestroyGunsmithPart();
            parts.Remove(selectedPart);
        }

        

        GameObject newPartGameObject = Instantiate(newPart, instantiatePos, Quaternion.identity, partParent);
        newPartGameObject.transform.localEulerAngles = Vector3.zero;

        parts.Add(newPartGameObject.GetComponent<GunsmithPart>(), newPart);

        
        newPartGameObject.GetComponent<GunsmithPart>().AttatchParts();

		if (GunsmithUIManager.Instance() != null)
		{
			newPartGameObject.GetComponent<GunsmithPart>().CreateUI();
		}

		selectedPart = newPartGameObject.GetComponent<GunsmithPart>();
		partPrefabs = parts.Values.ToList();
	}

    public void AddMissingPart(GameObject newPart)
    {
        GameObject newPartGameObject = Instantiate(newPart, Vector3.zero, Quaternion.identity, partParent);
        newPartGameObject.transform.localEulerAngles = Vector3.zero;
        
        parts.Add(newPartGameObject.GetComponent<GunsmithPart>(), newPart);
        newPartGameObject.GetComponent<GunsmithPart>().AttatchParts();

		if (GunsmithUIManager.Instance() != null)
		{
			newPartGameObject.GetComponent<GunsmithPart>().CreateUI();
		}

		partPrefabs = parts.Values.ToList();

	}

    public void RemovePart(GunsmithPart partToRemove)
    {
        parts.Remove(partToRemove);
    }

    public GunsmithPartDatabase GetPartDatabase()
    {
        return partDatabase;
    }

    void Update()
    {
        
        foreach (GunsmithPart part in parts.Keys)
        {
            part.AttatchParts();
        }
        foreach (GunsmithPart part in parts.Keys.ToList())
        {
            part.FillInMissingParts();
        }
    }

    public Transform GetPartsParent()
    {
        return partParent;
    }
}
