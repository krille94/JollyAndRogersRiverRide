using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RiverWalls : MonoBehaviour
{
    public Material material;

    private GameObject riverWallsObject;
    private Mesh mesh;

    [SerializeField]
    private float quadHeight = 10;

    void Start()
    {
        RiverObject river = null;

        if (gameObject.GetComponent<RiverController>())
        {
            river = gameObject.GetComponent<RiverController>().riverAsset;
        }

        if(river != null)
        {
            GenerateWalls(river);
        }
    }

    public void AddComponentsToRenderGeneratedMesh (Mesh mesh)
    {
        riverWallsObject = new GameObject("RiverWalls");
        riverWallsObject.transform.position += Vector3.down * 4;

        riverWallsObject.AddComponent<MeshFilter>();
        riverWallsObject.GetComponent<MeshFilter>().sharedMesh = mesh;

        riverWallsObject.AddComponent<MeshRenderer>();
        riverWallsObject.GetComponent<MeshRenderer>().sharedMaterial = material;

        riverWallsObject.AddComponent<MeshCollider>();
        riverWallsObject.GetComponent<MeshCollider>().sharedMesh = mesh;
    }

    public void GenerateWalls (RiverObject river)
    {
        Debug.Log("Generate");
        mesh = new Mesh();
        mesh.name = "GeneratedRiverWall";

        List<Vector3> vertices = new List<Vector3>();
        List<int> triangles = new List<int>();
        List<Vector2> uvs = new List<Vector2>();

        for (int i = 0; i < river.vertices.Length - 2; i += 2)
        {
            GenerateQuad(river, i, ref vertices, ref triangles, ref uvs, false);
        }

        for (int i = 1; i < river.vertices.Length - 2; i += 2)
        {
            GenerateQuad(river, i, ref vertices, ref triangles, ref uvs, true);
        }

        mesh.vertices = vertices.ToArray();
        mesh.triangles = triangles.ToArray();
        mesh.uv = uvs.ToArray();
        mesh.RecalculateNormals();

        AddComponentsToRenderGeneratedMesh(mesh);
    }

    private void GenerateQuad (RiverObject river, int index, ref List<Vector3> vertices, ref List<int> triangles, ref List<Vector2> uvs, bool fliped)
    {
        int vertCount = vertices.Count;

        vertices.Add(river.vertices[index + 0]);
        vertices.Add(river.vertices[index + 2]);
        vertices.Add(river.vertices[index + 0] + new Vector3(0, quadHeight, 0));
        vertices.Add(river.vertices[index + 2] + new Vector3(0, quadHeight, 0));

        if (fliped)
        {
            triangles.Add(vertCount + 0);
            triangles.Add(vertCount + 1);
            triangles.Add(vertCount + 2);
            triangles.Add(vertCount + 1);
            triangles.Add(vertCount + 3);
            triangles.Add(vertCount + 2);
        }
        else
        {
            triangles.Add(vertCount + 2);
            triangles.Add(vertCount + 1);
            triangles.Add(vertCount + 0);
            triangles.Add(vertCount + 2);
            triangles.Add(vertCount + 3);
            triangles.Add(vertCount + 1);
        }
        
        uvs.Add(new Vector2(0, 0));
        uvs.Add(new Vector2(1, 0));
        uvs.Add(new Vector2(0, 1));
        uvs.Add(new Vector2(1, 1));
    }
}
