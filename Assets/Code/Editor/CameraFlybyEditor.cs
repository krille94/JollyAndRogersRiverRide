using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(MenuCameraFlyby))]
public class CameraFlybyEditor : Editor
{
    private int selectedIndex = -1;
    private float duration = 2;

    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        MenuCameraFlyby menuCamera = (MenuCameraFlyby)target;

        EditorGUILayout.LabelField("Min Duration To Next Point");
        duration = EditorGUILayout.FloatField(duration);

        if(GUILayout.Button("Add Pos"))
        {
            menuCamera.AddCameraPosition(menuCamera.cam.transform.position, menuCamera.cam.transform.rotation, duration);
        }

        EditorGUILayout.LabelField("Selected Index");
        selectedIndex = EditorGUILayout.IntField(selectedIndex);

        if (GUILayout.Button("Overite Pos"))
        {
            if (selectedIndex != -1)
            {
                menuCamera.SaveCameraPosition(selectedIndex, menuCamera.cam.transform.position, menuCamera.cam.transform.rotation, duration);
                selectedIndex = -1;
            }
        }

        if(GUILayout.Button("Remove Pos"))
        {
            if (selectedIndex != -1)
            {
                menuCamera.RemoveCameraPosition(selectedIndex);
                selectedIndex = -1;
            }
        }
    }

    private void OnSceneGUI()
    {
        MenuCameraFlyby tool = (MenuCameraFlyby)target;
        Handles.color = Color.green;
        for (int i = 0; i < tool.positionPoints.Length - 1; i++)
        {
            Vector3 pOne = tool.positionPoints[i].position;
            Vector3 pTwo = tool.positionPoints[i + 1].position;
            Handles.DrawLine(pOne, pTwo);
        }
    }
}
