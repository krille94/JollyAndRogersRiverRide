using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(RiverSpeedController))]
public class RiverSpeedEditor : Editor
{

    GUIStyle customGuiStyle;

    bool GlobalValues = false;
    bool OtherValues = false;
    bool HelpValues = true;
    bool DefaultValues = false;

    public override void OnInspectorGUI()
    {
        customGuiStyle = new GUIStyle();
        customGuiStyle.fontSize = 25;
        customGuiStyle.richText = true;

        RiverSpeedController controller = (RiverSpeedController)target;

        if (GUILayout.Button("Calculate"))
            controller.Calculate();

        //-----------------------GLOBAL----------------------------\\
        if(!GlobalValues)
            GlobalValues = EditorGUILayout.Foldout(GlobalValues, "Global Values <<CLOSED>>", true, customGuiStyle);
        else
            GlobalValues = EditorGUILayout.Foldout(GlobalValues, "Global Values <<OPEN>>", true, customGuiStyle);
        GUILayout.Space(5);
        //<<Stuff>>
        if (GlobalValues)
        {
            controller.minimumSpeed = (int)GUILayout.HorizontalSlider(controller.minimumSpeed, 1, 100);
            GUILayout.Label("Force: " + controller.minimumSpeed.ToString("F1"));
             
            controller.defaultMovementDirection = EditorGUILayout.Vector3Field("Direction: ", controller.defaultMovementDirection);
        }
        //<<Stuff>>
        GUILayout.Space(25);
        //-----------------------OTHER---------------------------\\
        if(!OtherValues)
            OtherValues = EditorGUILayout.Foldout(OtherValues, "Other Values <<CLOSED>>", true, customGuiStyle);
        else
            OtherValues = EditorGUILayout.Foldout(OtherValues, "Other Values <<OPEN>>", true, customGuiStyle);
        GUILayout.Space(5);
        //<<Stuff>>
        if(OtherValues)
        {

        }
        //<<Stuff>>
        GUILayout.Space(25);
        //-----------------------HELP------------------------\\
        if(!HelpValues)
            HelpValues = EditorGUILayout.Foldout(HelpValues, "Help <<CLOSED>>", true, customGuiStyle);
        else
            HelpValues = EditorGUILayout.Foldout(HelpValues, "Help <<OPEN>>", true, customGuiStyle);
        GUILayout.Space(5);
        //<<Stuff>>
        if (HelpValues)
        {
            GUILayout.Space(5);//ImageSpacing
            GUILayout.Label((Texture2D)Resources.Load("EditorInspector/RiverForceAndDirections") as Texture2D);
            GUILayout.Space(-150);//ImageSpacing
        }
        //<<Stuff>>
        //-----------------------DEFAULTINSPECTOR-----------------------------\\
        GUILayout.Space(25);
        DefaultValues = GUILayout.Toggle(DefaultValues, "Draw Unity Default Inspectory Stuff");
        if(DefaultValues)
            DrawDefaultInspector();
    }
}
