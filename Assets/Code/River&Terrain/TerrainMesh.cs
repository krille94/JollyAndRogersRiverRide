using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TerrainMesh : MonoBehaviour
{
    [SerializeField] new MeshCollider collider;
    [SerializeField] MeshFilter filter;

    [Header("Modify Terrain In Update")]
    public bool modifyCollider = false;
    [Range(0.0f, 1.0f)] public float modifyFreq = 0.75f;
    [Range(0.0f, 10.0f)] public float waveheight = 0.25f;

    private void Start()
    {
        if (collider == null)
            collider = GetComponent<MeshCollider>();
        if (filter == null)
            filter = GetComponent<MeshFilter>();

        //GenerateTerrain();
    }

    private void Update()
    {
        ModifyTerrain();
    }

    private void ModifyTerrain ()
    {
        if (Random.value > modifyFreq)
            return;

        Mesh mesh = filter.mesh;

        Vector3[] verts = mesh.vertices;

        for (int i = 0; i < verts.Length; i++)
        {
            float targetHeight = Random.value * waveheight;
            float oldHeight = verts[i].y;
            float newHeight = Mathf.Lerp(oldHeight, targetHeight, 0.5f);
            verts[i] = new Vector3(verts[i].x, newHeight, verts[i].z);
        }

        mesh.RecalculateNormals();

        mesh.vertices = verts;

        filter.sharedMesh = mesh;
        if(modifyCollider)
            collider.sharedMesh = mesh;

        for (int i = 0; i < transform.childCount; i++)
        {
            transform.GetChild(i).GetComponent<MeshFilter>().sharedMesh = mesh;
        }

        //Debug.Log("Water Mesh Modifyed");
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
