using UnityEngine;

[System.Serializable]
public class SpeedValue
{
    public int minimumSpeed;
    public float maximumSpeed;

    public float turnForwardForce;
    public float forwardForce;
    public float backwardForce = 500;
    public float turnBackwardForce;
    public float paddleTime;
    
    public SpeedValue()
    {
    }
}
