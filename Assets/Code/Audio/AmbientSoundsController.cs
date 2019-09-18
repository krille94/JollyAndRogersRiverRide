using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmbientSoundsController : MonoBehaviour
{
    public static AmbientSoundsController controller;

    public GameObject playerBoat;

    private void Awake()
    {
        controller = this;

        playerBoat = GameObject.FindGameObjectWithTag("Player");
    }
}
