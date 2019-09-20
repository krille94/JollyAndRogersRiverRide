using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class RiverTool : MonoBehaviour
{
    public MeshFilter mf;
    public MeshRenderer mr;

    public List<Vector3> vertices = new List<Vector3>();
    public List<int> tris = new List<int>();
    public List<Vector2> uvs = new List<Vector2>();

    public int lenght = 2;

    [HideInInspector]public bool autoUpdateMesh = false;

    #region Main Methods
    private void Update()
    {
        if (autoUpdateMesh)
        {
            GenerateMesh();
            UpdateUVs();

            mr.sharedMaterial.mainTextureOffset = new Vector2(Time.time,Time.time);
        }
    }

    public void GenerateMesh()
    {
        Mesh mesh = new Mesh();
        mesh.name = "RiverMesh";

        mesh.vertices = vertices.ToArray();
        mesh.triangles = tris.ToArray();
        mesh.uv = uvs.ToArray();

        mesh.RecalculateNormals();

        mf.sharedMesh = mesh;
    }
    #endregion

    #region Editor Update Methods
    public void UpdateUVs()
    {
        uvs.Clear();
        for (int i = 0; i < vertices.Count; i++)
        {
            uvs.Add(new Vector2(vertices[i].x, vertices[i].z));
        }
    }
    #endregion

    #region Construction Methods
    public void AddRow()
    {
        lenght++;

        Vector3 last_2_pos = vertices[vertices.Count - 2];
        Vector3 last_1_pos = vertices[vertices.Count - 1];

        Vector3 last_3_pos = vertices[vertices.Count - 3];
        Vector3 last_4_pos = vertices[vertices.Count - 4];

        Vector3 dir_1 = last_1_pos - last_3_pos;
        dir_1.Normalize();

        Vector3 dir_2 = last_2_pos - last_4_pos;
        dir_2.Normalize();

        vertices.Add(last_2_pos + (dir_2 * 2));
        vertices.Add(last_1_pos + (dir_1 * 2));


        //uvs.Add(new Vector2(0, lenght - 1));
        //uvs.Add(new Vector2(1, lenght - 1));
        UpdateUVs();

        tris.Add((vertices.Count-1) - 1);
        tris.Add((vertices.Count-1) - 2);
        tris.Add((vertices.Count-1) - 3);

        tris.Add((vertices.Count - 1) - 0);
        tris.Add((vertices.Count - 1) - 2);
        tris.Add((vertices.Count - 1) - 1);
    }

    public void RemoveRow()
    {
        if (lenght <= 2)
            return;

        vertices.RemoveAt(vertices.Count - 1);
        vertices.RemoveAt(vertices.Count - 1);

        uvs.RemoveAt(uvs.Count - 1);
        uvs.RemoveAt(uvs.Count - 1);

        tris.RemoveAt(tris.Count - 1);
        tris.RemoveAt(tris.Count - 1);
        tris.RemoveAt(tris.Count - 1);

        tris.RemoveAt(tris.Count - 1);
        tris.RemoveAt(tris.Count - 1);
        tris.RemoveAt(tris.Count - 1);

        lenght--;
    }

    public void SetToDefaultRiverDesign()
    {
        vertices.Clear();
        tris.Clear();
        uvs.Clear();

        lenght = 2;

        vertices.Add(new Vector3(0, 0, 0));
        vertices.Add(new Vector3(1, 0, 0));
        vertices.Add(new Vector3(0, 0, 1));
        vertices.Add(new Vector3(1, 0, 1));

        tris.Add(0);
        tris.Add(2);
        tris.Add(1);

        tris.Add(2);
        tris.Add(3);
        tris.Add(1);

        uvs.Add(new Vector2(0, 0));
        uvs.Add(new Vector2(1, 0));
        uvs.Add(new Vector2(0, 1));
        uvs.Add(new Vector2(1, 1));

        AddRow();
        AddRow();
        AddRow();
    }
    #endregion
}
