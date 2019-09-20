using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class RiverToolWindow : EditorWindow
{
    [MenuItem("Window/RiverRide Tools/River Maker")]
    static void Init()
    {
        // Get existing open window or if none, make a new one:
        RiverToolWindow window = (RiverToolWindow)EditorWindow.GetWindow(typeof(RiverToolWindow));
        window.Show();
    }
}
