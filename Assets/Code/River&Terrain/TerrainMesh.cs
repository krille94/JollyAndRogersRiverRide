using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TerrainMesh : MonoBehaviour
{
    [SerializeField] new MeshCollider collider;
    [SerializeField] new MeshFilter filter;

    private void Start()
    {
        if (collider == null)
            collider = GetComponent<MeshCollider>();
        if (filter == null)
            filter = GetComponent<MeshFilter>();

        GenerateTerrain();
    }

    private void GenerateTerrain ()
    {
        Mesh mesh = new Mesh();
        List<Vector3> vertices = new List<Vector3>();
        List<int> triangles = new List<int>();

        vertices.Add(new Vector3(0, 0, 0));
        vertices.Add(new Vector3(1, 0, 0));
        vertices.Add(new Vector3(0, 0, 1));
        vertices.Add(new Vector3(1, 0, 1));

        triangles.Add(0);
        triangles.Add(2);
        triangles.Add(3);
        triangles.Add(2);
        triangles.Add(3);
        triangles.Add(0);

        mesh.triangles = triangles.ToArray();
        mesh.vertices = vertices.ToArray();

        mesh.RecalculateNormals();

        collider.sharedMesh = mesh;
        filter.sharedMesh = mesh;
    }
}
