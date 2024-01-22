using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.UI;

public class CameraTerrainModifier : MonoBehaviour
{
    public Text textSize;
    public Text textSetHeight;
    [Tooltip("Range where the player can interact with the terrain")]
    public float rangeHit = 100;
    [Tooltip("Force of modifications applied to the terrain")]
    public float modiferStrengh = 10;
    [Tooltip("Size of the brush, number of vertex modified")]
    public float sizeHit = 6;
    [Tooltip("Color of the new voxels generated")][Range(0, Constants.NUMBER_MATERIALS-1)]
    public int buildingMaterial = 0;

    private RaycastHit hit;
    private ChunkManager chunkManager;

    [SerializeField] Transform cameraTransform;

    [SerializeField] bool setHeight;
    [SerializeField] int editHeight;
    void Start()
    {
        chunkManager = ChunkManager.Instance;
        UpdateUI();
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.H))
        {
            setHeight = !setHeight;
            textSetHeight.gameObject.SetActive(setHeight);
        }
        if(setHeight)
        {
            if (Input.GetMouseButton(0) || Input.GetMouseButton(1))
            {
                if (Physics.Raycast(cameraTransform.position, cameraTransform.forward, out hit, rangeHit))
                {
                    Vector3 hitPos = hit.point - hit.normal * 0.5f;
                    Vector3Int roundedPos = new Vector3Int(Mathf.RoundToInt(hitPos.x), Mathf.RoundToInt(hitPos.y), Mathf.RoundToInt(hitPos.z));
                    chunkManager.SetHeightModification(roundedPos, sizeHit, editHeight + (Constants.MAX_HEIGHT/2));

                }
            }
        }
        else if (Input.GetMouseButtonDown(0))
        {
            float modification = -modiferStrengh;
            if (Physics.Raycast(cameraTransform.position, cameraTransform.forward, out hit, rangeHit))
            {
                Vector3 hitPos = hit.point - hit.normal * 0.5f;
                Vector3Int roundedPos = new Vector3Int(Mathf.RoundToInt(hitPos.x), Mathf.RoundToInt(hitPos.y), Mathf.RoundToInt(hitPos.z));
                chunkManager.ModifyChunkData(roundedPos, sizeHit, modification, buildingMaterial);

            }
        }

        else if (Input.GetMouseButtonDown(1))
        {
            float modification = modiferStrengh;
            if (Physics.Raycast(cameraTransform.position, cameraTransform.forward, out hit, rangeHit))
            {
                Vector3 hitPos = hit.point + hit.normal * 0.5f;
                Vector3Int roundedPos = new Vector3Int(Mathf.RoundToInt(hitPos.x), Mathf.RoundToInt(hitPos.y), Mathf.RoundToInt(hitPos.z));
                chunkManager.ModifyChunkData(roundedPos, sizeHit, modification, buildingMaterial);
            }
        }

        //Inputs
        if (Input.GetAxis("Mouse ScrollWheel") > 0)
        {
            sizeHit++;
            UpdateUI();
        }
        else if (Input.GetAxis("Mouse ScrollWheel") < 0)
        {
            sizeHit--;
            UpdateUI();
        }
        
        if(Input.GetKeyDown(KeyCode.Equals) || Input.GetKeyDown(KeyCode.KeypadPlus))
        {
            editHeight++;
            UpdateUI();
        }
        else if((Input.GetKeyDown(KeyCode.Minus) || Input.GetKeyDown(KeyCode.KeypadMinus)) && sizeHit > 1)
        {
            editHeight--;
            UpdateUI();
        }

        if (Input.GetMouseButtonDown(2))
        {
            if (Physics.Raycast(cameraTransform.position, cameraTransform.forward, out hit, rangeHit))
            {
                Vector3 hitPos = hit.point - hit.normal * 0.5f;
                Vector3Int roundedPos = new Vector3Int(Mathf.RoundToInt(hitPos.x), Mathf.RoundToInt(hitPos.y), Mathf.RoundToInt(hitPos.z));
                editHeight = roundedPos.y;
                UpdateUI();
            }
        }
    }

    public void UpdateUI()
    {
        if (textSize != null && textSetHeight != null)
        {
            textSize.text = "(+ -) Brush size: " + sizeHit;
            textSetHeight.text = "Set Height = " + editHeight;
        }
        
    }

    void OnEnable()
    {
        textSize.gameObject.SetActive(true);
        textSetHeight.gameObject.SetActive(setHeight);
    }

    void OnDisable()
    {
        textSize.gameObject.SetActive(false);
        textSetHeight.gameObject.SetActive(false);
    }
}
