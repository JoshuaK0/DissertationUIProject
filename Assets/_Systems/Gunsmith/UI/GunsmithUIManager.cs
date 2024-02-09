using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunsmithUIManager : MonoBehaviour
{
    [SerializeField] GunsmithPartDatabase database;
    [SerializeField] GameObject partLabelButton;
    [SerializeField] Transform gunLabelCanvas;
    [SerializeField] Transform inventoryCanvas;
    [SerializeField] GameObject inventoryTabButton;
    [SerializeField] GameObject inventoryPartButton;

    public static GunsmithUIManager instance;

	PartType selectedPartType;

    List<GunsmithInventoryTab> typeInventories = new List<GunsmithInventoryTab>();

    public static GunsmithUIManager Instance()
    {
        return instance;
    }

    public GameObject GetPartLabelButton()
    {
        return partLabelButton;
    }

    void Awake()
    {
        instance = this;

        foreach (GunsmithPartTypeCollection collection in database.GetPartTypeCollections())
        {
			PartType type = collection.GetPartType();
            GameObject tabButton = Instantiate(inventoryTabButton, Vector3.zero, Quaternion.identity, inventoryCanvas);
            tabButton.GetComponent<GunsmithInventoryTab>().SetTabType(type);
            typeInventories.Add(tabButton.GetComponent<GunsmithInventoryTab>());

            foreach (GameObject part in collection.GetParts())
            {
                GameObject partButton = Instantiate(inventoryPartButton, Vector3.zero, Quaternion.identity, tabButton.transform);
                partButton.GetComponent<GunsmithInventoryPart>().SetPart(part);
            }

            tabButton.SetActive(false);
        }

    }

    public void SelectPartType(PartType partType)
    {
        selectedPartType = partType;

        foreach(GunsmithInventoryTab type in typeInventories)
        {
            if (type.GetPartType() == selectedPartType)
            {
                type.gameObject.SetActive(true);
            }
            else
            {
                type.gameObject.SetActive(false);
            }
        }
    }

    public Transform GetUICanvas()
    {
        return gunLabelCanvas;
    }

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Tab))
        {
            inventoryCanvas.gameObject.SetActive(!inventoryCanvas.gameObject.activeSelf);
            gunLabelCanvas.gameObject.SetActive(inventoryCanvas.gameObject.activeSelf);
        }
    }
}
