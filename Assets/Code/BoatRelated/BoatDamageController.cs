using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class BoatDamageController : MonoBehaviour
{
    public int MaxHull = 1000;
    private float hull;

    public UnityEvent onDeath;

    private void Start()
    {
        hull = MaxHull;
    }

    // Update is called once per frame
    void Update()
    {
        if(hull<=0)
        {
            Time.timeScale = 0;
            onDeath.Invoke();
        }

        if (Input.GetKeyDown(KeyCode.S))
        {
            hull -= 1;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        hull -= collision.relativeVelocity.magnitude;
    }

    private void OnGUI()
    {
        GUI.Box(new Rect(0, 0, 100, 25), "HULL: " + hull.ToString("F0") + " / " + MaxHull.ToString());
    }
}
