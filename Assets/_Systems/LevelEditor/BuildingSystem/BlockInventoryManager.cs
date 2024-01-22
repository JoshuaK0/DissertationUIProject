using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BlockInventoryManager : MonoBehaviour
{
    static BlockInventoryManager instance;

    [SerializeField]BlockDatabase blockDatabase;

    [SerializeField] Button buttonPrefab;
    [SerializeField] RectTransform inventoryParent;

    [SerializeField] BlockBuilder buildController;

    public static BlockInventoryManager Instance()
    {
        return instance;
    }

    private void Awake()
    {
        instance = this;
    }

    public void SelectedBlockButton(BuildableBlock block)
    {
        buildController.SetBuildableBlock(block);
    }

    void Start()
    {
        foreach(BuildableBlock b in blockDatabase.GetBlocks())
        {
            BlockInventoryButton newButton = Instantiate(buttonPrefab, Vector3.zero, Quaternion.identity, inventoryParent).GetComponent<BlockInventoryButton>();
            newButton.SetBlock(b);
        }
    }
}
