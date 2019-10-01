using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(RiverTool))]
public class RiverToolEditor : Editor
{
    bool displayInfo = false;
    bool displayOptions = false;
    bool displayPointofnoreturnOptions = false;

    bool hideShowNodeTools = true;

    float handleMaxDistance = 100;
    float handleSize = 100;

    RiverTool tool;

    private void OnEnable()
    {
        EditorApplication.update += Update;
        tool = (RiverTool)target;
    }

    private void OnDisable()
    {
        EditorApplication.update -= Update;
    }

    public void Update ()
    {
        if (tool.autoUpdateMesh)
        {
            tool.UpdateMesh();
            tool.UpdateNodes();
        }
    }

    public override void OnInspectorGUI()
    {
        //RiverTool tool = (RiverTool)target;

        tool.autoUpdateMesh = EditorGUILayout.Toggle("AutoUpdate", tool.autoUpdateMesh);
        if (!tool.autoUpdateMesh)
        {
            if (GUILayout.Button("Update River"))
                tool.UpdateRiver();
        }

        if (GUILayout.Button("Build River To Asset"))
            tool.BuildRiverPrefab();

        tool.uniqName = GUILayout.TextField(tool.uniqName);

        if (GUILayout.Button("Build River From Asset"))
            tool.GetRiverPrefab();

        if (GUILayout.Button("Connect With River From Controller"))
            tool.GetRiverFromController();
        if (GUILayout.Button("Connect With River From MeshFilter"))
            tool.GetRiverFromMesh();

        EditorGUILayout.LabelField("---------------------------------------------------------------------------------------------------");
        //EditorGUILayout.Space();

        if (GUILayout.Button("Add Strait Row"))
            tool.AddStraitRow();

        GUILayout.Space(5);
        if (GUILayout.Button("DownwardAngle: " + tool.downwardAngle.ToString() + " (Default: -5)"))
            tool.downwardAngle = -5;
        tool.downwardAngle = GUILayout.HorizontalSlider(tool.downwardAngle, -20, -1);
        if (GUILayout.Button("Add Downward Row"))
            tool.AddDownwardRow();

        if (GUILayout.Button("Remove Row"))
            tool.RemoveRow();

        EditorGUILayout.LabelField("---------------------------------------------------------------------------------------------------");
        //EditorGUILayout.Space();

        if (GUILayout.Button("!Danger-PointOfNoReturn!"))
            displayPointofnoreturnOptions = !displayPointofnoreturnOptions;
        if(displayPointofnoreturnOptions)
        {
            if (GUILayout.Button("Reset To Default"))
                tool.SetToDefaultRiverDesign();
        }

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

            hideShowNodeTools = GUILayout.Toggle(hideShowNodeTools, "Node Tools");
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
            Vector3 newPos = tool.transform.InverseTransformPoint(Handles.FreeMoveHandle(tool.transform.TransformPoint(tool.vertices[i]), Quaternion.identity, distFromCam / handleSize, Vector3.one, Handles.CircleHandleCap));
            if (newPos.y != tool.vertices[i].y)
                tool.vertices[i] = new Vector3(newPos.x, tool.vertices[i].y, newPos.z);
            else
                tool.vertices[i] = newPos;
        }

        if (hideShowNodeTools)
        {
            if (tool.nodes.Count > 0)
            {
                Handles.color = Color.white;
                for (int i = 0; i < tool.nodes.Count; i++)
                {
                    float distFromCam = 1;
                    if (Camera.current != null)
                    {
                        distFromCam = Vector3.Distance(Camera.current.transform.position, tool.transform.TransformPoint(tool.nodes[i].centerVector));
                        if (distFromCam > handleMaxDistance)
                        {
                            distFromCam = 0;
                            continue;
                        }
                    }

                    tool.nodes[i].centerVectorOffset = Handles.FreeMoveHandle(tool.transform.TransformPoint(tool.nodes[i].centerVector + tool.nodes[i].centerVectorOffset), Quaternion.identity, distFromCam / handleSize, Vector3.one, Handles.RectangleHandleCap) - tool.nodes[i].centerVector;
                    tool.nodes[i].centerVectorOffset.y = 0;
                    if (tool.nodes[i].centerVectorOffset.x > 10)
                        tool.nodes[i].centerVectorOffset.x = 10;
                    else if (tool.nodes[i].centerVectorOffset.x < -10)
                        tool.nodes[i].centerVectorOffset.x = -10;
                    if (tool.nodes[i].centerVectorOffset.z > 10)
                        tool.nodes[i].centerVectorOffset.z = 10;
                    else if (tool.nodes[i].centerVectorOffset.z < -10)
                        tool.nodes[i].centerVectorOffset.z = -10;

                    //tool.nodes[i].flowDirectionOffset_Angle = Handles.RadiusHandle(Quaternion.identity, tool.nodes[i].centerVector + tool.transform.position + tool.nodes[i].centerVectorOffset + Vector3.up, tool.nodes[i].flowDirectionOffset_Angle);
                    tool.nodes[i].flowDirectionOffset = Handles.RotationHandle(Quaternion.Euler(tool.nodes[i].flowDirectionOffset), tool.nodes[i].centerVector + tool.transform.position + tool.nodes[i].centerVectorOffset).eulerAngles;

                    Handles.ArrowHandleCap(0, tool.nodes[i].centerVector + tool.transform.position + tool.nodes[i].centerVectorOffset, Quaternion.Euler(tool.nodes[i].flowDirection + tool.nodes[i].flowDirectionOffset), (distFromCam / handleSize) * 10, EventType.Repaint);
                }

                Handles.color = Color.red;
                for (int i = 0; i < tool.nodes.Count; i++)
                {
                    if (i + 1 < tool.nodes.Count)
                    {
                        Vector3 pOne = tool.nodes[i].centerVector + tool.nodes[i].centerVectorOffset + tool.transform.position;
                        Vector3 pTwo = tool.nodes[i + 1].centerVector + tool.nodes[i + 1].centerVectorOffset + tool.transform.position;
                        Handles.DrawLine(pOne + tool.nodes[i].flowDirection, pTwo + tool.nodes[i + 1].flowDirection);
                    }
                }
            }
        }
    }
}
