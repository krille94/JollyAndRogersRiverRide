using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditor.SceneManagement;

[CustomEditor(typeof(MenuButtons))]
public class MenuButtonsEditor : Editor
{
    MenuButtons tool;

    private void OnEnable()
    {
        tool = (MenuButtons)target;
    }

    public override void OnInspectorGUI()
    {
        tool.buttonAction = (MenuButtons.ButtonActions)EditorGUILayout.EnumPopup("Button Action", tool.buttonAction);

        //RiverTool tool = (RiverTool)target;
        if (tool.buttonAction.ToString() == "StartGame")
        {
            //tool.CurrentMenu = (GameObject)EditorGUILayout.ObjectField("Boat", tool.CurrentMenu, typeof(GameObject), true);
        }
        if (tool.buttonAction.ToString()=="ChangeOptions")
        {
            tool.optionType = (MenuButtons.OptionTypes) EditorGUILayout.EnumPopup("Option Type", tool.optionType);
            if(tool.optionType.ToString()=="ResetHighscores")
            {
                tool.CurrentMenu = (GameObject)EditorGUILayout.ObjectField("Move From Menu", tool.CurrentMenu, typeof(GameObject), true);
                tool.NextMenu = (GameObject)EditorGUILayout.ObjectField("Move To Menu", tool.NextMenu, typeof(GameObject), true);
            }
        }
        if (tool.buttonAction.ToString() == "ChangeMenu"|| tool.buttonAction.ToString() == "Highscores")
        {
            tool.CurrentMenu = (GameObject)EditorGUILayout.ObjectField("Move From Menu", tool.CurrentMenu, typeof(GameObject), true);
            tool.NextMenu = (GameObject)EditorGUILayout.ObjectField("Move To Menu", tool.NextMenu, typeof(GameObject), true);
        }
        if (tool.buttonAction.ToString() == "ResumeGame")
        {
            tool.CurrentMenu = (GameObject)EditorGUILayout.ObjectField("Pause Menu", tool.CurrentMenu, typeof(GameObject), true);
        }

        if (GUI.changed)
        {
            EditorUtility.SetDirty(tool);
        }
    }

}
