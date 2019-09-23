using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

[CustomEditor(typeof(RiverObject))]
public class RiverObjectEditor : Editor
{
    Mesh mesh;
    Material material;

    PreviewRenderUtility renderUtility;
    /*
    public override VisualElement CreateInspectorGUI()
    {
        mesh = new Mesh();

        mesh.vertices = ((RiverObject)target).vertices;
        mesh.triangles = ((RiverObject)target).tris;
        mesh.uv = ((RiverObject)target).uvs;

        renderUtility = new PreviewRenderUtility();

        return base.CreateInspectorGUI();
    }
    */
    private void OnEnable()
    {
        mesh = new Mesh();

        mesh.vertices = ((RiverObject)target).vertices;
        mesh.triangles = ((RiverObject)target).tris;
        mesh.uv = ((RiverObject)target).uvs;

        renderUtility = new PreviewRenderUtility();
    }

    private void OnDisable()
    {
        renderUtility.Cleanup();
    }
    /*
    protected override void OnHeaderGUI()
    {
        base.OnHeaderGUI();


    }

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        Rect rect = new Rect(40, 200, 200, 200);
        renderUtility.BeginPreview(rect, "");
        renderUtility.DrawMesh(mesh, Vector3.one, Quaternion.identity, material, 0);
        renderUtility.EndAndDrawPreview(rect);
    }*/
}

public class RiverObject : ScriptableObject
{
    public Vector3[] vertices;
    public int[] tris;
    public Vector2[] uvs;
    public RiverNode[] nodes;
}

[System.Serializable]
public class RiverNode
{
    public Vector3 flowDirection;

    public Vector3 centerVector;

    private Vector3 rightVector;
    private Vector3 leftVector;    

    public RiverNode (Vector3 rVec, Vector3 lVec, Vector3 dir)
    {
        rightVector = rVec;
        leftVector = lVec;

        flowDirection = dir.normalized;

        centerVector = (lVec - rVec) / 2 + rVec;
    }
}
