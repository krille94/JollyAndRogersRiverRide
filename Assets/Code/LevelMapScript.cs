using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelMapScript : MonoBehaviour
{
    public GameObject boat;
    public GameObject map;        // A plane showing the level's map
    public GameObject startpoint; // Where the boat first spawns
    public GameObject endpoint;   // The goal

    float startZ;
    float endZ;
    float mapStartZ;
    float mapEndZ;

    float levelLength;
    float mapLength;
    Vector3 iconPos;

    // Start is called before the first frame update
    void Start()
    {
        startZ = startpoint.transform.position.z;
        endZ = endpoint.transform.position.z;

        mapStartZ = -4;
        mapEndZ = 4;
        levelLength = endZ - startZ;
        mapLength = mapEndZ - mapStartZ;

        iconPos = transform.localPosition;
        iconPos.z = mapEndZ;
    }

    // Update is called once per frame
    void Update()
    {
        float boatPos = (endZ - boat.transform.position.z) / levelLength;
        iconPos.z = mapStartZ + (boatPos * mapLength);
        transform.localPosition = new Vector3(1, 1, iconPos.z);
    }
}
