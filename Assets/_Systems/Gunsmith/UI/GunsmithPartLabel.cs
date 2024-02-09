using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using static UnityEngine.GraphicsBuffer;

public class GunsmithPartLabel : MonoBehaviour
{
	PartType partType;
    [SerializeField] GunsmithPart part;

    [SerializeField] TMP_Text text;
    public void InitButton(GunsmithPart ownerPart, PartType newType)
    {
        partType = newType;
        this.part = ownerPart;
        //text.text = partType.ToString();
    }

    public void OnButtonClicked()
    {
        GunsmithUIManager.Instance().SelectPartType(partType);
        GunsmithManager.Instance().SelectPart(part);
    }

    void Update()
    {
        transform.position = part.transform.position;
        transform.rotation = Camera.main.transform.rotation;
    }
}
