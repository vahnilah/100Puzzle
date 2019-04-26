using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;


[RequireComponent(typeof(MeshFilter))]
public class MeshSquare : MonoBehaviour
{

    public float gap = 0.01f;
    float c = 0.5f;

    Mesh mesh;

    Vector3[] vertices;
    int[] triangles;


    private void Awake()
    {
        mesh = GetComponent<MeshFilter>().mesh;
    }

    // Start is called before the first frame update
    void Start()
    {
        MakeMeshData();
        UpdateMesh();
    }

    private void MakeMeshData()
    {
        // create vertices
        vertices = new Vector3[]{
            new Vector2(0 - c + gap,0 - c + gap), // bottom left
            new Vector2(0 - c + gap,1 - c - gap), // top left
            new Vector2(1 - c - gap,0 - c + gap), // bottom right
            new Vector2(1 - c - gap,1 - c - gap)  // top right
        };
        //vertices = new Vector3[]{
        //    new Vector2(0,0),
        //    new Vector2(0,1),
        //    new Vector2(1,0),
        //    new Vector2(1,1)
        //};
        // create integers 
        triangles = new int[] { 0, 1, 2, 2, 1, 3 };
    }

    private void UpdateMesh()
    {
        mesh.Clear();
        mesh.vertices = vertices;
        mesh.triangles = triangles;
    }



}
