using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelMapScript : MonoBehaviour
{
    public GameObject boat;
    public GameObject map;        // A plane showing the level's map
    public GameObject startpoint; // Where the boat first spawns
    public GameObject endpoint;   // The goal

    Vector3 start;
    Vector3 end;
    float mapStartZ;
    float mapEndZ;

    Vector3 levelLayout;
    float levelLength;
    float mapLength;
    Vector3 iconPos;
    int amountOfNodes;

    private RiverController river;

    // Start is called before the first frame update
    void Start()
    {
        river = RiverController.instance;

        //Vector3 target = river.riverAsset.GetNodeFromPosition(river.transform.position, body.transform.position).centerVector;

        // = river.riverAsset.GetNodeFromPosition(river.transform.position, startpoint.transform.position).centerVector;
        //end = river.riverAsset.GetNodeFromPosition(river.transform.position, endpoint.transform.position).centerVector;
        start=river.riverAsset.nodes[0].centerVector;

        int i = 0;
        foreach(RiverNode node in river.riverAsset.nodes)
        {
            levelLayout += node.centerVector;
            i++;
            if (i == 14)
                break;
        }
        amountOfNodes = i;
        //end = river.riverAsset.nodes[i-1].centerVector;
        end = river.riverAsset.GetNodeFromPosition(river.transform.position, endpoint.transform.position).centerVector;
        mapStartZ = 4;
        mapEndZ = -4;
        levelLength = (levelLayout.x + levelLayout.z) / 2;
        mapLength = mapEndZ - mapStartZ;

        iconPos = transform.localPosition;
        iconPos.z = mapEndZ;
    }

    // Update is called once per frame
    void Update()
    {
        RiverNode boatLocation = river.riverAsset.GetNodeFromPosition(river.transform.position, boat.transform.position);

        float boatPos = 0;
        int i = 0;
        foreach (RiverNode node in river.riverAsset.nodes)
        {
            boatPos += ((node.centerVector.x + node.centerVector.z) / 2);

            if (node==boatLocation)
            {
                //float alter = ((node.centerVector.x - boat.transform.position.x) + (node.centerVector.z - boat.transform.position.z));
                //boatPos -= alter;
                break;
            }
        }

        boatPos /= levelLength;

        if (PlayerData.distanceTraveled<boatPos) PlayerData.distanceTraveled=boatPos;

        iconPos.z = mapStartZ + (boatPos * mapLength);
        transform.localPosition = new Vector3(1, 1, iconPos.z);
    }
}
