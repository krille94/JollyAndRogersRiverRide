using UnityEngine;

[System.Serializable]
public class RiverNode
{
    public Vector3 flowDirection;
    public float flowDirectionOffset_Angle = 1;

    public Vector3 centerVector;
    public Vector3 centerVectorOffset;

    public RiverNode (Vector3 rVec, Vector3 lVec, Vector3 dir, Vector3 saveOffset, float saveFlowOffset)
    {
        flowDirection = dir.normalized;

        centerVector = (lVec - rVec) / 2 + rVec;

        centerVectorOffset = saveOffset;
        if (saveFlowOffset == 0)
            flowDirectionOffset_Angle = 1;
        else
            flowDirectionOffset_Angle = saveFlowOffset;
    }
}
