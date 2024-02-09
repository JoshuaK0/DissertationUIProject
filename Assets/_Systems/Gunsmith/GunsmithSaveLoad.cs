using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class GunsmithSaveLoad : MonoBehaviour
{
	[SerializeField] GunsmithManager manager;
	[SerializeField] GunsmithPartDatabase partDatabase;

	[SerializeField] List<GameObject> databaseCollection = new List<GameObject>();

	void Awake()
	{
		foreach(GunsmithPartTypeCollection partsList in partDatabase.GetPartTypeCollections())
		{
			foreach(GameObject part in partsList.GetParts())
			{
				databaseCollection.Add(part);
			}
		}
	}
	public void SaveGun()
	{
		List<int> partsIndices = new List<int>();
		foreach (GameObject partPrefab in manager.GetPrefabs())
		{
			partsIndices.Add(databaseCollection.IndexOf(partPrefab));
		}
		foreach (var x in partsIndices)
		{
			Debug.Log(x.ToString());
		}
		GunsmithGunSave gunSave = new GunsmithGunSave(partsIndices.ToArray());

		string json = JsonUtility.ToJson(gunSave);
		string filePath = Path.Combine(Application.persistentDataPath, "gunSave");
		File.WriteAllText(filePath, json);
		Debug.Log($"GunsmithGunSave saved to {filePath}");
	}

	public GunsmithGunSave LoadGunsmithGunSave(string saveName)
	{
		string filePath = Path.Combine(Application.persistentDataPath, "gunSave");
		if (File.Exists(filePath))
		{
			string json = File.ReadAllText(filePath);
			GunsmithGunSave gunSave = JsonUtility.FromJson<GunsmithGunSave>(json);
			return gunSave;
		}
		else
		{
			Debug.LogError($"Save file not found: {filePath}");
			return null; // Or alternatively, return a new GunsmithGunSave with default values
		}
	}

	public List<GameObject> GetPrefabsFromSave(GunsmithGunSave gunSave)
	{
		List<GameObject> prefabs = new List<GameObject>();
		foreach(int partIndex in gunSave.databaseIndex)
		{
			prefabs.Add(databaseCollection[partIndex]);
		}
		return prefabs;
	}

	public void CreateGun(List<GameObject> prefabs)
	{
		foreach(GameObject pref in prefabs)
		{
			Instantiate(pref);
		}
	}
}
