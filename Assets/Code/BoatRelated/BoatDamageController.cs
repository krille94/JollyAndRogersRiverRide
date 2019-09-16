using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoatDamageController : MonoBehaviour
{
    public int MaxHull = 1000;
    private float hull;

    private void Start()
    {
        hull = MaxHull;
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
