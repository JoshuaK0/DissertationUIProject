using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
public class ProceduralFOVBox : MonoBehaviour
{
    [SerializeField] Vector3 frontFace;
    [SerializeField] Vector3 middleFace;
    [SerializeField] Vector3 backFace;

	[SerializeField] float FOVEyeHeight;

	Vector3 eyePosition;
	void Start()
    {
        CreateCube(frontFace, middleFace, backFace);
    }

    void CreateCube(Vector3 frontFace, Vector3 middleFace, Vector3 backFace)
    {
        int[] triangles = 
        {
            0, 2, 1, //face front
		    0, 3, 2,
            2, 3, 4, //face top
		    2, 4, 5,
            1, 2, 5, //face right
		    1, 5, 6,
            0, 7, 4, //face left
		    0, 4, 3,
/*            5, 4, 7, //face back
		    5, 7, 6,*/
            0, 6, 7, //face bottom
		    0, 1, 6,
            
            5, 4, 8, //face top
		    9, 5, 8,
            6, 5, 9, //face right
		    6, 9, 10,
            4, 7, 8, //face left
		    11, 8, 7,


            7, 6, 10, //face bottom
		    7, 10, 11,

            
            8, 11, 9, //face back
		    11, 10, 9
        };
        
        Vector3[] vertices = 
        {
            //front face
            new Vector3 (-frontFace.x, -frontFace.y, frontFace.z), // bottom left 0
            new Vector3 (frontFace.x, -frontFace.y, frontFace.z), // top left 1
            new Vector3 (frontFace.x, frontFace.y, frontFace.z), // bottom right 2 
            new Vector3 (-frontFace.x, frontFace.y, frontFace.z), // top right 3

            //middle face
            new Vector3 (-middleFace.x, middleFace.y, middleFace.z), // top left 4
            new Vector3 (middleFace.x, middleFace.y, middleFace.z), // top right 5
            new Vector3 (middleFace.x, -middleFace.y, middleFace.z), // bottom right 6
            new Vector3 (-middleFace.x, -middleFace.y, middleFace.z), // bottom left 7

            //middle face
            new Vector3 (-backFace.x, backFace.y, backFace.z), // top left 8 
            new Vector3 (backFace.x, backFace.y, backFace.z), // top right 9
            new Vector3 (backFace.x, -backFace.y, backFace.z), // bottom right 10
            new Vector3 (-backFace.x, -backFace.y, backFace.z), // bottom left 11
        };

        Mesh mesh = GetComponent<MeshFilter>().sharedMesh;
        
        if(mesh == null)
        {
            mesh = new Mesh();
            
        }

        GetComponent<MeshFilter>().sharedMesh = mesh;
        GetComponent<MeshCollider>().sharedMesh = mesh;
        
        mesh.Clear();
        mesh.vertices = vertices;
        mesh.triangles = triangles;

        mesh.RecalculateNormals();
    }

    private void OnDrawGizmosSelected()
    {
        CreateCube(frontFace, middleFace, backFace);
        Gizmos.color = Color.red;

        Gizmos.DrawWireMesh(GetComponent<MeshFilter>().sharedMesh, transform.position, transform.rotation);
    }
}
