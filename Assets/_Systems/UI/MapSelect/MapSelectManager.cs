using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MapSelectManager : MonoBehaviour
{
    static MapSelectManager instance;
    public static MapSelectManager Instance
    {
        get
        {
            return instance;
        }
    }
    
    [SerializeField] RectTransform levelPanelContainer;
    [SerializeField] GameObject levelPanel;

    [SerializeField] string levelLoadSceneName;
    public const string WORLDS_DIRECTORY = "/mapdata"; //Directory worlds (save folder, that contains the worlds folders)
    string[] levels;

    string selectedMap;
    void Awake()
    {
        instance = this;
        GameObject.DontDestroyOnLoad(gameObject);
    }
    void Start()
    {
        levels = Directory.GetDirectories(Application.persistentDataPath + WORLDS_DIRECTORY);
        foreach (string level in levels)
        {
            string levelName = Path.GetFileName(level);
            GameObject newLevelPanel = Instantiate(levelPanel, levelPanelContainer);
            newLevelPanel.GetComponent<MapPanel>().SetLevelName(levelName);
        }
    }

    public void RestartLevel()
    {
		SceneManager.LoadScene(levelLoadSceneName);
	}


	public void SelectMap(string selectedMap)
    {
        this.selectedMap = selectedMap;
    }
    public void EnterMap()
    {
        SceneManager.LoadScene(levelLoadSceneName);
    }

    public string GetSelectedMap()
    {
        return selectedMap;
    }
}
