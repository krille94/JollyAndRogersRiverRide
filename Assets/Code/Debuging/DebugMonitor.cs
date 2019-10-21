using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct DebugMonitoredObject
{
    public enum DebugTypes { rigidbodyVelocity, rigidbodyVelocityIgnoreY }
    public DebugTypes type;
    public GameObject obj;
}

public class DebugMonitor : MonoBehaviour
{
    private bool displayMonitor = false;
    public GoldChestContainer container;

    [Header("Level Objects")]
    public List<DebugMonitoredObject> monitoredObjects = new List<DebugMonitoredObject>();
        
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

        GUI.BeginGroup(new Rect(Screen.width - 200, 0, 200, 50 + (25 * monitoredObjects.Count)));
        GUI.Box(new Rect(0, 0, 200, 50 + (25 * monitoredObjects.Count)),"Debug Monitor");
        if(GUI.Button(new Rect(0,0,25,25), "X")) { this.enabled = false; }
        int heightIndex = 1;

        Rect rect = new Rect(10, 25, 200, 25);
        float msec = Time.deltaTime * 1000.0f;
        float fps = 1.0f / Time.deltaTime;
        string text = string.Format("{0:0.0} ms ({1:0.} fps)", msec, fps);
        GUI.Label(rect, text);

        if (container != null)
        {
            GUI.Label(new Rect(0, 50, 200, 25), "Gold Amount: " + (float)container.gold / (float)container.maxGold + "%");

            heightIndex++;
        }

        GUI.Label(new Rect(0, 50, 200, 25), "-------------------------------------------");

        heightIndex++;        

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
            if (monitored.type == DebugMonitoredObject.DebugTypes.rigidbodyVelocityIgnoreY)
            {
                GUI.Label(new Rect(0, 25 * heightIndex, 200, 25), "Obj: " + monitored.obj.name);
                heightIndex++;
                monitored.obj.GetComponent<Rigidbody>().velocity = new Vector3(monitored.obj.GetComponent<Rigidbody>().velocity.x, 0, monitored.obj.GetComponent<Rigidbody>().velocity.z);
                float velocity = monitored.obj.GetComponent<Rigidbody>().velocity.magnitude;
                GUI.Label(new Rect(0, 25 * heightIndex, 200, 25), "Vel: " + velocity);
                heightIndex++;
            }
            GUI.Label(new Rect(0, 25 * heightIndex, 200, 25), "---------------------------------------------------------------------");
            heightIndex++;
        }

        GUI.EndGroup();
    }
}
