using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[ExecuteInEditMode]
public class RiverTool : MonoBehaviour
{
    public string uniqName = "RiverAssetName";

    public List<Vector3> vertices = new List<Vector3>();
    public List<int> tris = new List<int>();
    public List<Vector2> uvs = new List<Vector2>();

    public int lenght = 2;

    [HideInInspector]public bool autoUpdateMesh = false;

    public List<RiverNode> nodes = new List<RiverNode>();

    public float downwardAngle = -5;

    #region Main Methods
    public void GetRiverFromMesh()
    {
        if(gameObject.GetComponent<MeshFilter>())
        {
            if(gameObject.GetComponent<MeshFilter>().sharedMesh != null)
            {
                Mesh m = gameObject.GetComponent<MeshFilter>().sharedMesh;
                vertices.Clear();
                for (int i = 0; i < m.vertices.Length; i++)
                {
                    vertices.Add(m.vertices[i]);
                }
                tris.Clear();
                for (int i = 0; i < m.triangles.Length; i++)
                {
                    tris.Add(m.triangles[i]);
                }
                UpdateUVs();
                UpdateNodes();
            }
        }
    }
#if UNITY_EDITOR
    public void BuildRiverPrefab ()
    {
        if(AssetDatabase.LoadAssetAtPath("Assets/Resources/RiverBuilds/"+ uniqName +".asset", typeof(RiverObject)) != null)
        {
            Debug.Log("River With Name"+ uniqName +" Exist! No Option We Auto Overrides.");

            RiverObject obj = (RiverObject)AssetDatabase.LoadAssetAtPath<RiverObject>("Assets/Resources/RiverBuilds/" + uniqName + ".asset");
            obj.vertices = vertices.ToArray();
            obj.tris = tris.ToArray();
            obj.uvs = uvs.ToArray();
            obj.nodes = nodes.ToArray();
            obj.lenght = lenght;

            AssetDatabase.SaveAssets();
            return;
        }

        RiverObject river = ScriptableObject.CreateInstance<RiverObject>();
        river.name = uniqName;

        river.vertices = vertices.ToArray();
        river.tris = tris.ToArray();
        river.uvs = uvs.ToArray();
        river.nodes = nodes.ToArray();
        river.lenght = lenght;

        AssetDatabase.CreateAsset(river, "Assets/Resources/RiverBuilds/"+river.name+".asset");
        AssetDatabase.SaveAssets();
    }

    public void GetRiverPrefab()
    {
        RiverObject obj = (RiverObject)AssetDatabase.LoadAssetAtPath<RiverObject>("Assets/Resources/RiverBuilds/" + uniqName + ".asset");
        if(obj == null)
        {
            Debug.Log("Did not find river with this name.!");
            return;
        }

        vertices.Clear();

        uvs.Clear();
        tris.Clear();
        nodes.Clear();
        for (int i = 0; i < obj.vertices.Length; i++)
        {
            vertices.Add(obj.vertices[i]);
        }
        for (int i = 0; i < obj.uvs.Length; i++)
        {
            uvs.Add(obj.uvs[i]);
        }
        for (int i = 0; i < obj.tris.Length; i++)
        {
            tris.Add(obj.tris[i]);
        }
        for (int i = 0; i < obj.nodes.Length; i++)
        {
            nodes.Add(obj.nodes[i]);
        }
        lenght = obj.lenght;
    }
#endif
    public void GetRiverFromController ()
    {
        RiverObject obj = gameObject.GetComponent<RiverController>().riverAsset;
        if(obj == null)
        {
            Debug.LogWarning("River Controller Got No RiverAssets");
            return;
        }

        vertices.Clear();
        uvs.Clear();
        tris.Clear();
        nodes.Clear();

        for (int i = 0; i < obj.vertices.Length; i++)
        {
            vertices.Add(obj.vertices[i]);
        }
        for (int i = 0; i < obj.uvs.Length; i++)
        {
            uvs.Add(obj.uvs[i]);
        }
        for (int i = 0; i < obj.tris.Length; i++)
        {
            tris.Add(obj.tris[i]);
        }
        for (int i = 0; i < obj.nodes.Length; i++)
        {
            nodes.Add(obj.nodes[i]);
        }
        lenght = obj.lenght;
    }

    public void GenerateMesh()
    {
        Mesh mesh = new Mesh();
        mesh.name = "RiverMesh";

        mesh.vertices = vertices.ToArray();
        UpdateNodes();
        mesh.triangles = tris.ToArray();
        UpdateUVs();
        mesh.uv = uvs.ToArray();

        mesh.RecalculateNormals();

        transform.GetComponent<MeshFilter>().sharedMesh = mesh;
        for (int i = 0; i < transform.childCount; i++)
        {
            if (transform.GetChild(i).GetComponent<MeshFilter>())
                transform.GetChild(i).GetComponent<MeshFilter>().sharedMesh = mesh;
        }
    }

    public void UpdateRiver()
    {
        UpdateMesh();
        UpdateNodes();
    }

    public void UpdateMesh()
    {
        Mesh mesh = transform.GetComponent<MeshFilter>().sharedMesh;
        if(mesh == null)
        {
            Debug.LogError("Missing mesh to modify");
            GenerateMesh();
            return;
        }

        mesh.Clear();

        mesh.vertices = vertices.ToArray();
        mesh.triangles = tris.ToArray();
        mesh.uv = uvs.ToArray();
        UpdateUVs();
        mesh.RecalculateNormals();

        for (int i = 0; i < transform.childCount; i++)
        {
            if (transform.GetChild(i).GetComponent<MeshFilter>())
                transform.GetChild(i).GetComponent<MeshFilter>().sharedMesh = mesh;
        }
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

    public void UpdateNodes()
    {
        RiverNode[] tempNodes = nodes.ToArray();

        nodes.Clear();

        int tempNodeIndex = 0;

        for (int i = 0; i < vertices.Count; i += 2)
        {
            if (i < vertices.Count && i-3 > -1)
            {
                Vector3 last_1_pos = vertices[i];

                Vector3 last_3_pos = vertices[i - 2];

                if (tempNodes.Length > tempNodeIndex)
                    nodes.Add(new RiverNode(vertices[i + 1], vertices[i], GetRiverFlow(last_1_pos, last_3_pos), tempNodes[tempNodeIndex].centerVectorOffset, tempNodes[tempNodeIndex].flowDirectionOffset));
                else
                    nodes.Add(new RiverNode(vertices[i + 1], vertices[i], GetRiverFlow(last_1_pos, last_3_pos), Vector3.zero, Vector3.zero));

                tempNodeIndex++;
            }
            else if(i + 2 < vertices.Count && i == 0)
            {
                Vector3 last_1_pos = vertices[i];

                Vector3 last_3_pos = vertices[i + 2];

                if (tempNodes.Length > tempNodeIndex)
                    nodes.Add(new RiverNode(vertices[i + 1], vertices[i], GetRiverFlow(last_3_pos, last_1_pos), tempNodes[tempNodeIndex].centerVectorOffset, tempNodes[tempNodeIndex].flowDirectionOffset));
                else
                    nodes.Add(new RiverNode(vertices[i + 1], vertices[i], GetRiverFlow(last_3_pos, last_1_pos), Vector3.zero, Vector3.zero));

                tempNodeIndex++;
            }
            else if (i + 2 < vertices.Count && i == 2)
            {
                Vector3 last_1_pos = vertices[i];

                Vector3 last_3_pos = vertices[i + 2];
                
                if (tempNodes.Length > tempNodeIndex)
                    nodes.Add(new RiverNode(vertices[i + 1], vertices[i], GetRiverFlow(last_3_pos, last_1_pos), tempNodes[tempNodeIndex].centerVectorOffset, tempNodes[tempNodeIndex].flowDirectionOffset));
                else
                    nodes.Add(new RiverNode(vertices[i + 1], vertices[i], GetRiverFlow(last_3_pos, last_1_pos), Vector3.zero, Vector3.zero));

                tempNodeIndex++;
            }
        }
    }
    #endregion

    #region Construction Methods
    private Vector3 GetRiverFlow (Vector3 topVec, Vector3 botVec)
    {
        Vector3 res = Vector3.zero;

        res = topVec - botVec;

        return res.normalized;
    }

    public void AddStraitRow()
    {
        lenght++;

        Vector3 last_2_pos = vertices[vertices.Count - 2];
        Vector3 last_1_pos = vertices[vertices.Count - 1];

        Vector3 last_3_pos = vertices[vertices.Count - 3];
        Vector3 last_4_pos = vertices[vertices.Count - 4];

        Vector3 dir_1 = GetRiverFlow(last_1_pos, last_3_pos) * 10;
        Vector3 dir_2 = GetRiverFlow(last_2_pos, last_4_pos) * 10;

        Vector3 newVector_1 = last_2_pos + dir_1;
        Vector3 newVector_2 = last_1_pos + dir_2;

        if (newVector_1.y != last_2_pos.y)
            newVector_1 -= new Vector3(0, downwardAngle, 0);
        if (newVector_2.y != last_1_pos.y)
            newVector_2 -= new Vector3(0, downwardAngle, 0);

        if (newVector_1.y > last_2_pos.y)
            newVector_1.y = last_2_pos.y;
        if (newVector_2.y > last_1_pos.y)
            newVector_2.y = last_1_pos.y;

        vertices.Add(newVector_1);
        vertices.Add(newVector_2);
        
        //uvs.Add(new Vector2(0, lenght - 1));
        //uvs.Add(new Vector2(1, lenght - 1));
        UpdateUVs();

        tris.Add((vertices.Count-1) - 1);
        tris.Add((vertices.Count-1) - 2);
        tris.Add((vertices.Count-1) - 3);

        tris.Add((vertices.Count - 1) - 0);
        tris.Add((vertices.Count - 1) - 2);
        tris.Add((vertices.Count - 1) - 1);

        RiverNode node = new RiverNode(vertices[vertices.Count - 1], vertices[vertices.Count - 2], dir_1, Vector3.zero, Vector3.zero);
        nodes.Add(node);
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

    public void AddDownwardRow()
    {
        lenght++;

        Vector3 last_1_pos = vertices[vertices.Count - 1];
        Vector3 last_2_pos = vertices[vertices.Count - 2];        

        Vector3 last_3_pos = vertices[vertices.Count - 3];
        Vector3 last_4_pos = vertices[vertices.Count - 4];

        Vector3 newVector_1 = last_2_pos + new Vector3(0, downwardAngle, 0);
        Vector3 newVector_2 = last_1_pos + new Vector3(0, downwardAngle, 0);

        Vector3 dir_1 = GetRiverFlow(last_1_pos, last_3_pos);
        Vector3 dir_2 = GetRiverFlow(last_2_pos, last_4_pos);

        vertices.Add(newVector_1 + (dir_1 * 10));
        vertices.Add(newVector_2 + (dir_2 * 10));

        UpdateUVs();

        tris.Add((vertices.Count - 1) - 1);
        tris.Add((vertices.Count - 1) - 2);
        tris.Add((vertices.Count - 1) - 3);

        tris.Add((vertices.Count - 1) - 0);
        tris.Add((vertices.Count - 1) - 2);
        tris.Add((vertices.Count - 1) - 1);

        RiverNode node = new RiverNode(vertices[vertices.Count - 1], vertices[vertices.Count - 2], dir_1, Vector3.zero, Vector3.zero);
        nodes.Add(node);
    }

    public void MakeRowDownward ()
    {

    }

    public void SetToDefaultRiverDesign()
    {
        vertices.Clear();
        tris.Clear();
        uvs.Clear();
        nodes.Clear();

        lenght = 2;

        vertices.Add(new Vector3(0, 0, 0));
        vertices.Add(new Vector3(10, 0, 0));
        vertices.Add(new Vector3(0, 0, 10));
        vertices.Add(new Vector3(10, 0, 10));

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

        AddStraitRow();
        AddStraitRow();
        AddStraitRow();

        GenerateMesh();
    }
    #endregion
}
