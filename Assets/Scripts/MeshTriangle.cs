using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshFilter))]
public class MeshTriangle : MonoBehaviour
{
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
            new Vector3(0,0,0),
            new Vector3(0,1,0),
            new Vector3(1,1,0)
        };
        // create integers 
        triangles = new int[] { 0, 1, 2 };
    }

    private void UpdateMesh()
    {
        mesh.Clear();
        mesh.vertices = vertices;
        mesh.triangles = triangles;
    }


}
