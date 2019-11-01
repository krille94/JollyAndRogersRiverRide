using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlowingText : MonoBehaviour
{
    public Color firstColor = new Color(1, 0.75f, 0);
    public Color secondColor = Color.white;
    public float colorShiftTime;
    private Color currentColor;

    private bool paused;
    private bool first;

    private float currentColorShiftTime;
    TextMesh text;

    private void Start()
    {
        currentColor = firstColor;
        paused = false;
        first = false;
        text = GetComponent<TextMesh>();
    }

    void Update()
    {
        if(first)
        {
            currentColor += ((firstColor - secondColor) * (Time.deltaTime / colorShiftTime));

            currentColorShiftTime += Time.deltaTime;
            if (currentColorShiftTime > colorShiftTime)
            {
                currentColorShiftTime = 0;
                first = false;
            }
        }
        else
        {
            currentColor -= ((firstColor - secondColor) * (Time.deltaTime / colorShiftTime));
            currentColorShiftTime += Time.deltaTime;
            if (currentColorShiftTime > colorShiftTime)
            {
                currentColorShiftTime = 0;
                first = true;
            }
        }
        text.color = currentColor;

    }
}
