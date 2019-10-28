using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PostEffect : MonoBehaviour
{
    public RenderTexture render;

    private Material material;

    // Creates a private material used to the effect
    void Awake()
    {
        material = new Material(Shader.Find("Hidden/FadeToBlack"));
    }

    public void FadeToBlack ()
    {
        StartCoroutine("Fader");
        
    }

    public void FadeToClear ()
    {

    }

    private bool isFading = false;
    private float fadeTime = 0;
    IEnumerator Fader()
    {
        isFading = true;
        while (fadeTime < 1)
        {
            fadeTime += Time.deltaTime;
            Debug.Log(fadeTime.ToString());
            yield return new WaitForEndOfFrame();
        }
        fadeTime = 0;
        isFading = false;
    }

    private void Update()
    {
        
    }

    private void OnRenderImage(RenderTexture source, RenderTexture destination)
    {
        //if (!isFading)
        //{
        //    Graphics.Blit(source, destination);
        //    return;
        //}
        Debug.LogError("");
        material.SetFloat("_bwBlend", fadeTime);
        Graphics.Blit(source, destination, material);
    }
}
