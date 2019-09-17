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
        Time.timeScale = 0;
        completed = true;
        onCompleted.Invoke();
    }
}
