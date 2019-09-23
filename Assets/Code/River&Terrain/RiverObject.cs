using UnityEngine;

public class RiverObject : ScriptableObject
{
    public Vector3[] vertices;
    public int[] tris;
    public Vector2[] uvs;
    public int lenght;
    public RiverNode[] nodes;

    public RiverNode GetNodeFromPosition (Vector3 pos)
    {
        RiverNode node = null;

        float dist = Mathf.Infinity;

        for (int i = 0; i < nodes.Length; i++)
        {
            float newDist = Vector3.Distance(pos, nodes[i].centerVector);
            if(newDist < dist)
            {
                node = nodes[i];
                dist = newDist;
            }
        }

        return node;
    }

    public Mesh GetMesh()
    {
        Mesh mesh = new Mesh();

        mesh.vertices = vertices;
        mesh.triangles = tris;
        mesh.uv = uvs;
        mesh.RecalculateNormals();

        return mesh;
    }
}
