using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(RiverTool))]
public class RiverToolEditor : Editor
{
    bool displayInfo = false;
    bool displayOptions = false;

    float handleMaxDistance = 100;
    float handleSize = 100;

    public override void OnInspectorGUI()
    {
        RiverTool tool = (RiverTool)target;

        tool.autoUpdateMesh = EditorGUILayout.Toggle("AutoUpdate", tool.autoUpdateMesh);
        if (!tool.autoUpdateMesh)
        {
            if (GUILayout.Button("Update Mesh"))
                tool.GenerateMesh();
        }

        EditorGUILayout.LabelField("---------------------------------------------------------------------------------------------------");
        //EditorGUILayout.Space();

        if (GUILayout.Button("Add Row"))
            tool.AddRow();
        if (GUILayout.Button("Remove Row"))
            tool.RemoveRow();

        EditorGUILayout.LabelField("---------------------------------------------------------------------------------------------------");
        //EditorGUILayout.Space();

        if (GUILayout.Button("Reset To Default"))
            tool.SetToDefaultRiverDesign();

        EditorGUILayout.LabelField("---------------------------------------------------------------------------------------------------");
        //EditorGUILayout.Space();

        if (GUILayout.Button("Hide/Show Options"))
            displayOptions = !displayOptions;
        //displayOptions = EditorGUILayout.Foldout(displayOptions, "                                       Options");
        if (displayOptions)
        {
            EditorGUILayout.LabelField("Handle Fade Distance: " + handleMaxDistance.ToString());
            handleMaxDistance = GUILayout.HorizontalSlider(handleMaxDistance, 50, 300);
            EditorGUILayout.LabelField("Handle Size: " + handleSize.ToString());
            handleSize = GUILayout.HorizontalSlider(handleSize, 10, 150);
        }

        EditorGUILayout.LabelField("---------------------------------------------------------------------------------------------------");
        //EditorGUILayout.Space();

        if (GUILayout.Button("Hide/Show Info"))
            displayInfo = !displayInfo;
        //displayInfo = EditorGUILayout.Foldout(displayInfo, "                                                     Info");

        if (displayInfo)
            base.OnInspectorGUI();
    }

    private void OnSceneGUI()
    {
        HandleUtility.AddDefaultControl(GUIUtility.GetControlID(FocusType.Passive));

        RiverTool tool = (RiverTool)target;

        if(tool.mf == null)
        {
            return;
        }
        else
        {
            ///Skiping first vertice, origin moves whole transform
            for (int i = 1; i < tool.vertices.Count; i++)
            {
                float distFromCam = 1;
                if(Camera.current != null)
                {
                    distFromCam = Vector3.Distance(Camera.current.transform.position, tool.transform.TransformPoint(tool.vertices[i]));
                    if(distFromCam > handleMaxDistance)
                    {
                        distFromCam = 0;
                    }
                }
                tool.vertices[i] = tool.transform.InverseTransformPoint(Handles.FreeMoveHandle(tool.transform.TransformPoint(tool.vertices[i]), Quaternion.identity, distFromCam / handleSize, Vector3.one, Handles.CircleHandleCap));
                tool.vertices[i] = new Vector3(tool.vertices[i].x, 0, tool.vertices[i].z);
            }
        }
    }
}
