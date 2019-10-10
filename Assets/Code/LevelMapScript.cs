using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelMapScript : MonoBehaviour
{
    public GameObject boat;
    public GameObject boatIcon;        // A plane showing the level's map
    public GameObject startpoint; // Where the boat first spawns
    public GameObject endpoint;   // The goal

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
        end = river.riverAsset.GetNodeFromPosition(river.transform.position, endpoint.transform.position).centerVector;

        levelStartX = 0;
        levelEndX = 0;
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

        mapStartZ = 4;
        mapEndZ = -3;
        mapStartX = 0;
        mapEndX = -4;
        mapLength = mapEndZ - mapStartZ;
        mapWidth = mapEndX - mapStartX;
        levelLength = (levelStartZ - levelEndZ) / mapLength;
        levelWidth = (levelEndX - levelStartX) / mapWidth;
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

        iconPos = linemap.GetPosition(0);
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 boatLocation = boat.transform.position;

        boatLocation.x /= levelWidth;
        boatLocation.z /= levelLength;

        //if (PlayerData.distanceTraveled<boatPos) PlayerData.distanceTraveled=boatPos;

        iconPos.x = mapStartX + boatLocation.x+0.5f;
        iconPos.z = mapStartZ + boatLocation.z;
        if(boatIcon != null)
            boatIcon.transform.localPosition = new Vector3(iconPos.x, 1, iconPos.z);
    }
}
