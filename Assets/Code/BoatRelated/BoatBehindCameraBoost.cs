using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoatBehindCameraBoost : MonoBehaviour
{
    public new CameraController camera;
    public RiverController river;

    public int lastPassedNodeIndex = 0;

    public RiverNode closestNode;

    public float force = 10;

    void Update()
    {
        closestNode = river.riverAsset.GetNodeFromPosition(transform.position);
        if(closestNode.index > lastPassedNodeIndex)
        {
            lastPassedNodeIndex = closestNode.index;
        }

        if(closestNode.index < lastPassedNodeIndex)
        {
            GetComponent<Rigidbody>().AddForce(river.riverAsset.GetNodeFromIndex(closestNode.index).flowDirection * force * Time.deltaTime);
        }
    }
}
