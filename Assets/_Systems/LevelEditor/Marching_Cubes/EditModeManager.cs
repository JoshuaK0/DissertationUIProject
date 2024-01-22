using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class EditModeManager : MonoBehaviour
{
    [SerializeField] bool generate;
    [SerializeField] bool clear;
    [SerializeField] ChunkManager chunkManager;
    [SerializeField] NoiseManager noiseManager;

    private void Update()
    {
        if(clear)
        {
            clear = false;
            for (int i = chunkManager.transform.childCount; i > 0; --i)
            {
                if(chunkManager.transform.GetChild(0).gameObject != null)
                {
                    DestroyImmediate(chunkManager.transform.GetChild(0).gameObject);
                }
            }
        }
        if(generate)
        {
            generate = false;
            noiseManager.CreateTerrain();
            
        }
        if(!Application.isPlaying)
        {
            chunkManager.Update();
        }
        
    }
}
