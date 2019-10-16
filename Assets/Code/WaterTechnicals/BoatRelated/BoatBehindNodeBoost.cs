using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoatBehindNodeBoost : MonoBehaviour
{
    private RiverController river;

    public int lastPassedNodeIndex = 0;

    public RiverNode closestNode;

    public float force = 10;
    [SerializeField] bool applyingForce;

    private void Start()
    {
        if(river == null)
        {
            river = (RiverController)GameObject.FindGameObjectWithTag("River").GetComponent<RiverController>();
        }
    }

    void Update()
    {
        closestNode = river.riverAsset.GetNodeFromPosition(transform.position);
        if(closestNode.index > lastPassedNodeIndex)
        {
            lastPassedNodeIndex = closestNode.index;
        }

        if(closestNode.index < lastPassedNodeIndex)
        {
            GetComponent<Rigidbody>().AddForce(river.riverAsset.GetNodeFromIndex(closestNode.index).flowDirection * force * Time.deltaTime * (lastPassedNodeIndex - closestNode.index));
            applyingForce = true;
        }
        else
        {
            applyingForce = false;
        }
    }
}
