using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class AmbientSoundsController : MonoBehaviour
{
    public static AmbientSoundsController controller;

    public GameObject playerBoat;

    private void Awake()
    {
        if(controller != null)
        {
            DestroyImmediate(gameObject);
            return;
        }
        controller = this;

        playerBoat = GameObject.FindGameObjectWithTag("Player");
    }
}
