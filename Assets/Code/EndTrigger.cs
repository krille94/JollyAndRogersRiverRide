using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EndTrigger : MonoBehaviour
{
    public bool completed = false;

    public UnityEvent onCompleted;

    private void Start()
    {

    }

    private void OnTriggerEnter(Collider other)
    {
        //Time.timeScale = 0;
        completed = true;
        PlayerData.distanceTraveled = 1; // Means you traveled 100% of the way
        onCompleted.Invoke();
    }
}
