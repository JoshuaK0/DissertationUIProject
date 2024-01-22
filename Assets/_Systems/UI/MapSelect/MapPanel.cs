using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class MapPanel : MonoBehaviour
{
    [SerializeField] string levelName;
    [SerializeField] List<TMP_Text> tMP_Texts;

    public void SetLevelName(string newLevelName)
    {
        levelName = newLevelName;
        foreach(var tmp in tMP_Texts)
        {
            tmp.text = levelName;
        }
    }

    public void OnLevelClicked()
    {
        MapSelectManager.Instance.SelectMap(levelName);
        MapSelectManager.Instance.EnterMap();
    }
}
