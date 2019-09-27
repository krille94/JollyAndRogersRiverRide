using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RiverWallGenerator : MonoBehaviour
{
    public RiverObject river;

    public MeshFilter meshFilter;

    void Start()
    {
        if (river == null)
            return;
        if (meshFilter == null)
            return;

        GenerateWalls();
    }

    private void GenerateWalls ()
    {
        List<Vector3> vertices = new List<Vector3>();
        int vertIndex = 0;

        for (int i = 0; i < river.vertices.Length; i += 2)
        {
            vertices.Add(river.vertices[i]);
            //add code
        }
        
        Mesh mesh = new Mesh();
        mesh.vertices = vertices.ToArray();
        Vector2[] uv = new Vector2[vertices.Count];
        for (int i = 0; i < vertices.Count; i++)
        {
            uv[i] = new Vector2(vertices[i].x, vertices[i].z);
        }
        mesh.uv = uv;
        mesh.RecalculateNormals();
        meshFilter.sharedMesh = mesh;
    }
}
