using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseHover : MonoBehaviour
{
    Color normal= new Color(1, 0.75f, 0);
    Color highlighted = Color.white;
    TextMesh text;
    void Start()
    {
        text = GetComponent<TextMesh>();
        text.color = normal;
    }

    void OnMouseOver()
    {
        //GetComponent<AudioSource>().Play();
        text.color = highlighted;
    }

    void OnMouseExit()
    {
        text.color = normal;
    }
}
