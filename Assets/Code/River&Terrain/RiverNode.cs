using UnityEngine;

[System.Serializable]
public class RiverNode
{
    public Vector3 flowDirection;

    public Vector3 centerVector;

    private Vector3 rightVector;
    private Vector3 leftVector;    

    public RiverNode (Vector3 rVec, Vector3 lVec, Vector3 dir)
    {
        rightVector = rVec;
        leftVector = lVec;

        flowDirection = dir.normalized;

        centerVector = (lVec - rVec) / 2 + rVec;
    }
}
