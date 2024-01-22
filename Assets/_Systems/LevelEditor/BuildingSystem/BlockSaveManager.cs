using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

public class BlockSaveManager : MonoBehaviour
{
    [SerializeField] List<SavedBlock> savedBlocks = new List<SavedBlock>();
    [SerializeField] BlockDatabase database;
    [SerializeField] float loadSpeed;
    [SerializeField] Transform blockParent;
    [SerializeField] bool loadBlocks;
    [SerializeField] bool saveBlocks;

    [SerializeField] string levelName = "default"; //World selected by the manager
    public const string WORLDS_DIRECTORY = "/mapdata"; //Directory worlds (save folder, that contains the worlds folders)

    public void SetLevel(string levelName)
    {
        this.levelName = levelName;
    }
    void Awake()
    {

        if (!Directory.Exists(Application.persistentDataPath + WORLDS_DIRECTORY))//in case worlds directory not created, create the "worlds" directory 
            Directory.CreateDirectory(Application.persistentDataPath + WORLDS_DIRECTORY);

        if (!Directory.Exists(Application.persistentDataPath + WORLDS_DIRECTORY + "/" + levelName))//in case world not created, create the world (generate folder)
            Directory.CreateDirectory(Application.persistentDataPath + WORLDS_DIRECTORY + "/" + levelName);
    }
    public void LoadBlocks()
    {
        if (!File.Exists(Application.persistentDataPath + WORLDS_DIRECTORY + '/' + levelName + "/blockSaveData.dat"))
        {
            Debug.Log(levelName);
            return;
        }
        using (Stream stream = File.Open(Application.persistentDataPath + WORLDS_DIRECTORY + '/' + levelName + "/blockSaveData.dat", FileMode.Open))
        {
            var bformatter = new System.Runtime.Serialization.Formatters.Binary.BinaryFormatter();
            SurrogateSelector selector = new SurrogateSelector();
            Vector3SerializationSurrogate vector3Surrogate = new Vector3SerializationSurrogate();
            selector.AddSurrogate(typeof(Vector3), new StreamingContext(StreamingContextStates.All), vector3Surrogate);

            bformatter.SurrogateSelector = selector;

            savedBlocks = (List<SavedBlock>)bformatter.Deserialize(stream);
        }
        StartCoroutine(DestroyChildren(blockParent));
    }
    IEnumerator DestroyChildren(Transform transformToDestroyChildren)
    {
        if (Application.isPlaying)
        {
            if (transformToDestroyChildren.childCount > 0)
            {
                while (transformToDestroyChildren.childCount > 0)
                {
                    for (int i = transformToDestroyChildren.childCount; i > 0; --i)
                    {
                        Destroy(transformToDestroyChildren.GetChild(0).gameObject);
                        yield return null;
                    }
                }
            }
            
        }
        else
        {
            while (transformToDestroyChildren.childCount > 0)
            {
                for (int i = transformToDestroyChildren.childCount; i > 0; --i)
                {
                    DestroyImmediate(transformToDestroyChildren.GetChild(0).gameObject);
                    yield return null;
                }
            }
        }
        if (transformToDestroyChildren.childCount == 0)
        {
            foreach (SavedBlock block in savedBlocks)
            {
                BuildableBlock newBlock = Instantiate(database.GetBlocks()[block.blockIndex].gameObject, block.position, Quaternion.Euler(block.rotation), blockParent).GetComponent<BuildableBlock>();
                newBlock.GameReadyMode();
            }
        }
        yield return null;
    }

    public void SaveBlocks()
    {
        savedBlocks.Clear();
        List<Vector3> savedPositions = new List<Vector3>();
        foreach (Transform t in blockParent.transform)
        {
            BuildableBlock b = t.GetComponent<BuildableBlock>();

            if (b != null)
            {
                foreach (BuildableBlock databaseBlock in database.GetBlocks())
                {
                    if (b.GetBlockIndex() == databaseBlock.GetBlockIndex())
                    {
                        if (!savedPositions.Contains(t.position))
                        {
                            Debug.Log(databaseBlock);
                            savedBlocks.Add(new SavedBlock(b.GetBlockIndex(), t.position, t.rotation));
                            savedPositions.Add(t.position);
                        }
                    }
                }
                
            }
        }

        FileStream fs = new FileStream(Application.persistentDataPath + WORLDS_DIRECTORY + '/' + levelName + "/blockSaveData.dat", FileMode.Create);
        BinaryFormatter bf = new BinaryFormatter();
        SurrogateSelector selector = new SurrogateSelector();
        Vector3SerializationSurrogate vector3Surrogate = new Vector3SerializationSurrogate();
        selector.AddSurrogate(typeof(Vector3), new StreamingContext(StreamingContextStates.All), vector3Surrogate);

        bf.SurrogateSelector = selector;
        
        bf.Serialize(fs, savedBlocks);
        fs.Close();
    }
}
