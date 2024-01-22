using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "BlockDatabase", menuName = "ScriptableObjects/TankEditor", order = 1)]
public class BlockDatabase : ScriptableObject
{
    [SerializeField] List<BuildableBlock> availableBlocks;

    public List<BuildableBlock> GetBlocks()
    {
        return availableBlocks;
    }

    public List<BuildableBlock> GetAvailableBlocks()
    {
        return availableBlocks;
    }

    void OnValidate()
    {
        for (int i = 0; i < availableBlocks.Count; i++)
        {
            availableBlocks[i].SetBlockIndex(i);
        }
    }
}
