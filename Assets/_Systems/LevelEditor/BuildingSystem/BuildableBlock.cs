using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class BuildableBlock : MonoBehaviour
{
    
    int blockCost;
    [SerializeField] string blockName;
    [SerializeField] int blockIndex;
    [SerializeField] bool globalRotation;
    [SerializeField] GameObject blueprintBlock;
    [SerializeField] GameObject gameReadyBlock;

    [SerializeField] Renderer blockRenderer;

    [SerializeField] Material clearMat;
    [SerializeField] Material blockedMat;

    int collisionCount;

    void Start()
    {
        if(globalRotation)
        {
            transform.rotation = Quaternion.identity;
        }
    }

    public string GetBlockName()
    {
        return blockName;
    }

    public int GetBlockIndex()
    {
        return blockIndex;
    }

    public void SetBlockIndex(int newIndex)
    {
        blockIndex = newIndex;
    }

    void Awake()
    {
        Rigidbody rb = GetComponent<Rigidbody>();
        rb.useGravity = false;
        rb.isKinematic = true;
    }
    public void BlueprintMode()
    {
        blueprintBlock.SetActive(true);
        gameReadyBlock.SetActive(false);
    }

    public void GameReadyMode()
    {
        blueprintBlock.SetActive(false);
        gameReadyBlock.SetActive(true);
    }

    public void DestroyBlock()
    {
        Destroy(gameObject);
    }

    public bool isSpaceClear()
    {
        return collisionCount <= 0;
    }
    void OnTriggerEnter(Collider other)
    {
        collisionCount++;
        blockRenderer.material = blockedMat;
    }

    void OnTriggerExit(Collider other)
    {
        collisionCount--;
        if (collisionCount <= 0)
        {
            blockRenderer.material = clearMat;
        }
    }
}
