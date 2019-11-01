using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseHover : MonoBehaviour
{
    Color normal= new Color(1, 0.75f, 0);
    Color highlighted = Color.white;
    TextMesh text;
    public bool selected = false;
    public bool setSelection = false;

    void OnEnable()
    {
        setSelection = false;
        text = GetComponent<TextMesh>();
        //text.color = normal;
    }

    public void SetSelected(bool yn)
    {
        setSelection = true;
        selected = yn;
    }

    public bool GetSelected()
    {
        if(selected&&!setSelection)
            return true;
        return false;
    }

    private void OnMouseUp()
    {
        setSelection = false;
        selected = false;
    }

    void OnMouseOver()
    {
        //GetComponent<AudioSource>().Play();
        //text.color = highlighted;
        selected = true;
    }

    void OnMouseExit()
    {
        setSelection = false;
        selected = false;
        //text.color = normal;
    }
}
