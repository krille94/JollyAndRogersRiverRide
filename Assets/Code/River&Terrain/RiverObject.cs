using UnityEngine;

public class RiverObject : ScriptableObject
{
    public Vector3[] vertices;
    public int[] tris;
    public Vector2[] uvs;
    public int lenght;
    public RiverNode[] nodes;

    public RiverNode GetNodeFromPosition (Vector3 offset, Vector3 pos)
    {
        RiverNode node = null;

        float dist = Mathf.Infinity;
        int index = 0;
        for (index = 0; index < nodes.Length; index++)
        {
            float newDist = Vector3.Distance(pos, offset + nodes[index].centerVector);
            if(newDist < dist)
            {
                node = nodes[index];
                dist = newDist;
            }
        }

        return node;
    }

    public Vector3 GetFlow (Vector3 riverObjectOffset, Vector3 boatPos)
    {
        RiverNode node = null;

        float dist = Mathf.Infinity;
        int index = 0;
        for (int i = 0; i < nodes.Length; i++)
        {
            float newDist = Vector3.Distance(boatPos, riverObjectOffset + nodes[i].centerVector);
            if (newDist < dist)
            {
                index = i;
                node = nodes[index];
                dist = newDist;
            }
        }

        //Vector3 res = Quaternion.ToEulerAngles(Quaternion.LookRotation(node.flowDirection, Vector3.forward) * Quaternion.AngleAxis(node.flowDirectionOffset_Angle, Vector3.right));
        //return res * -1;

        RiverNode lastNode = null;
        RiverNode nextNode = null;
        if (index - 1 >= 0)
            lastNode = nodes[index - 1];
        if (index + 1 < nodes.Length)
            nextNode = nodes[index + 1];

        if (lastNode == null || nextNode == null)
            return node.finalFlowDirection;

        float distLast = Vector3.Distance(boatPos, riverObjectOffset + lastNode.centerVector);
        float distNext = Vector3.Distance(boatPos, riverObjectOffset + nextNode.centerVector);

        if(distLast > distNext)
        {
            Vector3 res = node.finalFlowDirection;// + Quaternion.AngleAxis(node.flowDirectionOffset_Angle, Vector3.right).eulerAngles + nextNode.flowDirection + Quaternion.AngleAxis(nextNode.flowDirectionOffset_Angle, Vector3.right).eulerAngles;
            return res.normalized;
        }
        else
        {
            Vector3 res = node.finalFlowDirection;// + Quaternion.AngleAxis(node.flowDirectionOffset_Angle, Vector3.right).eulerAngles + lastNode.flowDirection + Quaternion.AngleAxis(lastNode.flowDirectionOffset_Angle, Vector3.right).eulerAngles;
            return res.normalized;
        }        
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
