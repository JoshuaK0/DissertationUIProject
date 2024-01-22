using UnityEngine;
[System.Serializable]
public class SavedBlock
{
    public int blockIndex;
    public Vector3 position;
    public Vector3 rotation;

    public SavedBlock(int blockIndex, Vector3 position, Quaternion rotation)
    {
        this.blockIndex = blockIndex;
        this.position = position;
        this.rotation = rotation.eulerAngles;
    }
}
