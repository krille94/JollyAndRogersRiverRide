using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseHover : MonoBehaviour
{
    void Start()
    {
        GetComponent<TextMesh>().color = Color.black;
    }

    void OnMouseEnter()
    {
        //GetComponent<AudioSource>().Play();
        GetComponent<TextMesh>().color = Color.red;
    }

    void OnMouseExit()
    {
        GetComponent<TextMesh>().color = Color.black;
    }
}
