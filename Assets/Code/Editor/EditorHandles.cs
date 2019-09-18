using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEditor;

[CustomEditor(typeof(AmbientSoundObject)), CanEditMultipleObjects]
public class EditorHandles : Editor
{
    protected virtual void OnSceneGUI()
    {
        AmbientSoundObject handles = (AmbientSoundObject)target;

        EditorGUI.BeginChangeCheck();

        Vector3 targetPos = Handles.FreeMoveHandle(new Vector3(handles.triggerRangeHandle.x, handles.transform.position.y, handles.triggerRangeHandle.z), Quaternion.identity, 0.1f * handles.triggerRange, Vector3.one, Handles.CubeHandleCap);

        if(EditorGUI.EndChangeCheck())
        {
            handles.triggerRangeHandle = targetPos;
            handles.triggerRange = Vector3.Distance(handles.transform.position, targetPos);
        }
    }
}
