using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GunsmithInventoryPart : MonoBehaviour
{
    [SerializeField] GameObject part;
    [SerializeField] TextMeshProUGUI text;

    public void SetPart(GameObject newPart)
    {
        part = newPart;
        text.text = part.name;
    }

    public void OnButtonClicked()
    {
        GunsmithManager.Instance().AddPartThroughButton(part);
    }
}
