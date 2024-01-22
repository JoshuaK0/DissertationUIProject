using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockBuilder : MonoBehaviour
{
    [SerializeField] BuildableBlock buildableBlock;
    [SerializeField] float buildRange;
    [SerializeField] float fastBuildRange;
    float currentBuildRange;

    Vector3 hitPos;    
    bool hasPos;

    Vector3 blockRot;

    [SerializeField] RectTransform inventoryTab;

    BuildableBlock testBlock;
    public BuildableBlock hitBlock;
    [SerializeField] CameraTerrainModifier terrainEditor;
    [SerializeField] MouseLook mouseLook;

    [SerializeField] Transform parentTransform;
    bool fastBuild;
    [SerializeField] float fastBuildRate;
    float lastBuildTime;

    void Awake()
    {
        ToggleInventoryTab(false);
        currentBuildRange = buildRange;
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.F))
        {
            fastBuild = !fastBuild;
            if(fastBuild)
            {
                currentBuildRange = fastBuildRange;
            }
            else
            {
                currentBuildRange = buildRange;
            }
        }
        
        if(fastBuild)
        {
            if(Time.time - lastBuildTime < fastBuildRate)
            {
                return;
            }
            if (Input.GetMouseButton(1) && hasPos && testBlock.isSpaceClear())
            {
                Build();
            }
            if (Input.GetMouseButton(0) && hitBlock != null)
            {
                hitBlock.DestroyBlock();
                if (testBlock != null)
                {
                    Destroy(testBlock.gameObject);
                    CreateTestBlock();
                }
            }
        }
        else
        {
            if (Input.GetMouseButtonDown(1) && hasPos && testBlock.isSpaceClear())
            {
                Build();
            }
            if (Input.GetMouseButtonUp(0) && hitBlock != null)
            {
                hitBlock.DestroyBlock();
                if (testBlock != null)
                {
                    Destroy(testBlock.gameObject);
                    CreateTestBlock();
                }
            }
        }
        
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            ToggleInventoryTab(!inventoryTab.gameObject.activeSelf);
        }
        
        if (buildableBlock?.GetBlockName() == "TerrainEditor")
        {
            terrainEditor.enabled = true;
            if (testBlock != null)
            {
                Destroy(testBlock.gameObject);
            }
            return;
        }
        else
        {
            terrainEditor.enabled = false;
        }

        if(Input.GetKeyDown(KeyCode.Q))
        {
            blockRot += Vector3.up * 90;
        }

        if (Input.GetKeyDown(KeyCode.E))
        {
            blockRot -= Vector3.up * 90;
        }


        GetHitPos();
        if(hasPos)
        {
            if (testBlock == null)
            {
                CreateTestBlock();
            }
            else
            {
                MoveTestBlock();
            }
        }
        else
        {
            if(testBlock != null)
            {
                Destroy(testBlock.gameObject);
            }
        }


        

        
    }

    void MoveTestBlock()
    {
        testBlock.transform.position = hitPos;
        testBlock.transform.eulerAngles = blockRot;
    }

    void CreateTestBlock()
    {
        if (buildableBlock == null)
        {
            return;
        }
        testBlock = Instantiate(buildableBlock, hitPos, Quaternion.identity).GetComponent<BuildableBlock>();
        testBlock.BlueprintMode();
    }

    void Build()
    {
        GameObject newBlock = Instantiate(buildableBlock.gameObject, hitPos, Quaternion.Euler(blockRot));
        newBlock.transform.parent = parentTransform;
        BuildableBlock buildableObject = newBlock.GetComponent<BuildableBlock>();
        buildableObject.GameReadyMode();

    }

    void GetHitPos()
    {
        Ray ray = Camera.main.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0.5f));
        RaycastHit hit;

        if(Physics.Raycast(ray, out hit, currentBuildRange))
        {
            Vector3 hitPosTemp = hit.point + hit.normal * 0.5f + (Vector3.up * 0.5f * Mathf.Sign(hit.point.y));
            float outputX = Mathf.Sign(hitPosTemp.x) * (Mathf.Abs((int)hitPosTemp.x) + 0.5f);
            float outputY = Mathf.Sign(hitPosTemp.y) * (Mathf.Abs((int)hitPosTemp.y));
            float outputZ = Mathf.Sign(hitPosTemp.z) * (Mathf.Abs((int)hitPosTemp.z) + 0.5f);
            
            hitPos = new Vector3(outputX, outputY, outputZ);
            hasPos = true;

            if (hit.transform != null)
            {
                hitBlock = hit.transform.GetComponent<BuildableBlock>();
            }
            else
            {
                hitBlock = null;
            }
        }
        else
        {
            hasPos = false;
        }    
    }

    internal void SetBuildableBlock(BuildableBlock newBlock)
    {
        buildableBlock = newBlock;
    }

    void ToggleInventoryTab(bool isActive)
    {
        inventoryTab.gameObject.SetActive(isActive);
        Cursor.visible = isActive;
        if(isActive)
        {
            Cursor.lockState = CursorLockMode.None;
            mouseLook.enabled = false;
        }
        else
        {
            Cursor.lockState = CursorLockMode.Locked;
            mouseLook.enabled = true;
        }
    }
}
