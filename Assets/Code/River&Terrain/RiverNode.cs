using UnityEngine;

[System.Serializable]
public class RiverNode
{
    public Vector3 flowDirection;
    public Vector3 flowDirectionOffset;
    public Vector3 finalFlowDirection;

    public Vector3 centerVector;
    public Vector3 centerVectorOffset;

    public RiverNode (Vector3 rVec, Vector3 lVec, Vector3 saveFlowDir, Vector3 saveCenterOffset, Vector3 saveFlowOffset)
    {
        flowDirection = saveFlowDir.normalized;

        centerVector = (lVec - rVec) / 2 + rVec;

        centerVectorOffset = saveCenterOffset;
        flowDirectionOffset = saveFlowOffset;

        finalFlowDirection = flowDirection;
        //if (flowDirectionOffset == Vector3.zero)
        //    flowDirectionOffset = Vector3.one;
        //finalFlowDirection = flowDirection + Quaternion.LookRotation(flowDirectionOffset, Vector3.forward).eulerAngles;
        //finalFlowDirection.Normalize();
        //finalFlowDirection.y = 0;
    }
}
