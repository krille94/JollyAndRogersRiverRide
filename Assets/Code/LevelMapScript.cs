using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelMapScript : MonoBehaviour
{
    public GameObject boat;
    public GameObject boatIcon;        // A plane showing the level's map
    //public GameObject startpoint; // Where the boat first spawns
    public GameObject endpoint;   // The goal

    public GameObject endPointGraphic;
    public LineRenderer dotRenderer;

    Vector3 start;
    Vector3 end;
    float mapStartZ;
    float mapEndZ;
    float mapStartX;
    float mapEndX;

    float levelStartX;
    float levelEndX;
    float levelStartZ;
    float levelEndZ;

    float levelLength;
    float levelWidth;
    float mapLength;
    float mapWidth;
    Vector3 iconPos;
    int amountOfNodes;

    private RiverController river;
    LineRenderer linemap;

    // Start is called before the first frame update
    void Start()
    {
        float windowWidth = (float)(Screen.width * 3) / (float)(Screen.height * 4);
        transform.localPosition = new Vector3(2.7f * windowWidth, 1.35f, 4);
        //Debug.Log(transform.localPosition);
        if (boatIcon == null)
            Debug.LogWarning("BoatIcon Missing!");

        linemap = GetComponent<LineRenderer>();
        if (linemap == null)
        {
            linemap = gameObject.AddComponent<LineRenderer>();
            Debug.LogWarning("LineRenderer Missing!\nAdded temp");
        }

        river = RiverController.instance;
        iconPos = Vector3.zero;

        start=river.riverAsset.nodes[0].centerVector;

        if(endpoint)
        {
            end = river.riverAsset.GetNodeFromPosition(river.transform.position, endpoint.transform.position).centerVector;
        }

        levelStartX = 0;
        levelEndX = 10000;
        int i = 0;
        foreach(RiverNode node in river.riverAsset.nodes)
        {
            if (node.centerVector.x > levelStartX) levelStartX = node.centerVector.x;
            if (node.centerVector.x < levelEndX) levelEndX = node.centerVector.x;

            if (node.centerVector.z > levelStartZ) levelStartZ = node.centerVector.z;
            if (node.centerVector.z < levelEndZ) levelEndZ = node.centerVector.z;

            i++;

            if (node.centerVector == end)
                break;
        }
        if (i > river.riverAsset.nodes.Length - 1)
            i = river.riverAsset.nodes.Length - 1;
        amountOfNodes = i;

        mapStartZ = 2;
        mapEndZ = -2.5f;
        mapStartX = 3;
        mapEndX = -1;
        mapLength = mapEndZ - mapStartZ;
        mapWidth = mapEndX - mapStartX;
        levelLength = (levelStartZ - levelEndZ) / mapLength;
        levelWidth = (levelStartX - levelEndX) / mapWidth;
        linemap.positionCount = i;
        for (int o=0; o<i; o++)
        {
            Vector3 linepos = river.riverAsset.nodes[o].centerVector;
            linepos.x /= levelWidth;
            linepos.z /= levelLength;
            linepos.x += mapStartX;
            linepos.z += mapStartZ;
            linepos.y = 0;
            linemap.SetPosition(o, linepos);
            //Debug.Log(linepos.z);
        }
        
        dotRenderer.positionCount = 14;
        dotRenderer.SetPosition(0, linemap.GetPosition(0));

        int dot=1;
        for (int o = 8; o < i; o+=8)
        {
            dotRenderer.SetPosition(dot, linemap.GetPosition(o));
            dot++;
        }

        iconPos = linemap.GetPosition(0);
        endPointGraphic.transform.localPosition = linemap.GetPosition(i - 1);//+new Vector3(0,0.02f,0);
        linemap.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 boatLocation = boat.transform.position;

        boatLocation.x /= levelWidth;
        boatLocation.z /= levelLength;

        //if (PlayerData.distanceTraveled<boatPos) PlayerData.distanceTraveled=boatPos;

        iconPos.x = mapStartX + boatLocation.x + (boatIcon.transform.localScale.x / 2);
        iconPos.z = mapStartZ + boatLocation.z + (boatIcon.transform.localScale.z / 2);
        if(boatIcon != null)
            boatIcon.transform.localPosition = new Vector3(iconPos.x, 0.02f, iconPos.z);
    }
}
