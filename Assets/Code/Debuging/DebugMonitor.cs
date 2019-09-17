using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct DebugMonitoredObject
{
    public enum DebugTypes { rigidbodyVelocity }
    public DebugTypes type;
    public GameObject obj;
}

public class DebugMonitor : MonoBehaviour
{
    public List<DebugMonitoredObject> monitoredObjects = new List<DebugMonitoredObject>();

    private bool displayMonitor = false;

    // Start is called before the first frame update
    void Start()
    {
        if (monitoredObjects.Count > 0)
            displayMonitor = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (monitoredObjects.Count > 0)
            displayMonitor = true;
        else
            displayMonitor = false;
    }

    // OnGUI draws graphics
    private void OnGUI()
    {
        if (displayMonitor == false)
            return;

        GUI.BeginGroup(new Rect(Screen.width - 200, 0, 200, 25 + (75 * monitoredObjects.Count)));
        GUI.Box(new Rect(0, 0, 200, 25 + (75 * monitoredObjects.Count)),"Debug Monitor");
        if(GUI.Button(new Rect(0,0,25,25), "X")) { this.enabled = false; }

        int heightIndex = 1;
        for (int i = 0; i < monitoredObjects.Count; i++)
        {
            DebugMonitoredObject monitored = monitoredObjects[i];
            if (monitored.obj == null)
                continue;
            if(monitored.type == DebugMonitoredObject.DebugTypes.rigidbodyVelocity)
            {
                GUI.Label(new Rect(0, 25 * heightIndex, 200, 25), "Obj: " + monitored.obj.name);
                heightIndex++;
                GUI.Label(new Rect(0, 25 * heightIndex, 200, 25), "Vel: " + monitored.obj.GetComponent<Rigidbody>().velocity.magnitude);
                heightIndex++;
            }
            GUI.Label(new Rect(0, 25 * heightIndex, 200, 25), "---------------------------------------------------------------------");
            heightIndex++;
        }

        GUI.EndGroup();
    }
}
