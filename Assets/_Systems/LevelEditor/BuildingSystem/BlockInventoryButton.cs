using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BlockInventoryButton : MonoBehaviour
{
    [SerializeField] BuildableBlock block;
    [SerializeField] Text text;
    public void EditorInventoryButtonClicked()
    {
        BlockInventoryManager.Instance().SelectedBlockButton(block);
    }

    public void SetBlock(BuildableBlock newBlock)
    {
        block = newBlock;
        text.text = block.name;
    }
}
