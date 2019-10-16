using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SpeedValueManager : MonoBehaviour
{
    [SerializeField] public List<SpeedValue> damageTaken = new List<SpeedValue>();
    private static List<SpeedValue> data = new List<SpeedValue>();

    private void Awake()
    {
        data = damageTaken;
    }

    public static List<SpeedValue> GetSpeedValues()
    {
        return data;
    }
}
