using System.Collections;
using System.Collections.Generic;
using Unity.AI.Navigation;
using UnityEngine;

public class LevelLoader : MonoBehaviour
{
    [SerializeField] string mapName;

    [SerializeField] BlockSaveManager blockSaveManager;
    [SerializeField] WorldManager worldManager;
    [SerializeField] NoiseManager noiseManager;

	[SerializeField] bool loadBlocks;
    [SerializeField] bool saveBlocks;

    [SerializeField] bool spawnUnits;

    [SerializeField] bool regenerateWorld;

    void Start()
    {
        mapName = MapSelectManager.Instance.GetSelectedMap();
        blockSaveManager.SetLevel(mapName);
        worldManager.SetLevel(mapName);
        noiseManager.CreateTerrain();
        blockSaveManager.LoadBlocks();

        if(spawnUnits)
        {
			Invoke("LoadAgents", 0.1f);
			
		}
		Invoke("InitializeObjectives", 0.2f);
	}

    void LoadAgents()
    {
		foreach (Transform g in FindObjectOfType<NavMeshSurface>().transform.parent.GetComponentsInChildren<Transform>())
		{
			g.gameObject.isStatic = true;
		}

		FindObjectOfType<NavMeshSurface>().BuildNavMesh();

		UnitInitializer.Instance.InitializeUnits();
	}

    void InitializeObjectives()
    {
		//ObjectiveManager.Instance().InitializeObjectives();
	}

    public string GetMapName()
    {
        return mapName;
    }

    void Update()
    {
        if (loadBlocks)
        {
            blockSaveManager.LoadBlocks();
            loadBlocks = false;
        }
        if (saveBlocks)
        {
            blockSaveManager.SaveBlocks();
            saveBlocks = false;
        }
        if(regenerateWorld)
        {
            noiseManager.regenerateWorld = true;
			worldManager.SetLevel(mapName);
			noiseManager.CreateTerrain();
            regenerateWorld = false;
		}
    }
}
