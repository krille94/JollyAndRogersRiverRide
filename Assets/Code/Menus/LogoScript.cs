using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LogoScript : MonoBehaviour
{
    public float PreFadeTimer = 0.5f;
    public float FadeInTimer=1;
    public float FadeOutTimer=1;
    public float PostFadeTimer = 0.5f;
    public float PauseTimer=2;
    float timer;
    float alpha;
    Color color;
    new Component[] renderer;

    // Start is called before the first frame update
    void Start()
    {
        alpha = 0.5f;
        color = Color.white;
        color.a = alpha;
        renderer = GetComponentsInChildren(typeof(MeshRenderer));
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;
        if (timer >= FadeInTimer + PauseTimer + FadeOutTimer+PostFadeTimer+PreFadeTimer)
        {
            SceneManager.LoadScene(1);
        }
        else if (timer >= FadeInTimer + PauseTimer + FadeOutTimer + PreFadeTimer)
        {
            if(alpha>0)
            {
                alpha = 0;
                SetNewFade();
            }
        }
        else if (timer>=FadeInTimer+PauseTimer+PreFadeTimer)
        {
            alpha = 1 - ((timer - (FadeInTimer + PauseTimer+PreFadeTimer)) / FadeOutTimer);
            SetNewFade();
        }
        else if (timer <= PauseTimer + FadeInTimer + PreFadeTimer && timer > PreFadeTimer + FadeInTimer)
        {
            alpha = 1;
            SetNewFade();
        }
        else if (timer <= FadeInTimer + PreFadeTimer && timer > PreFadeTimer)
        {
            alpha = ((timer - PreFadeTimer) / FadeInTimer);
            SetNewFade();
        }
        else if(alpha>0)
        {
            alpha = 0;
            SetNewFade();
        }
    }

    void SetNewFade()
    {
        color.a = alpha;

        foreach (Renderer r in renderer)
            foreach (Material mat in r.materials)
            {
                mat.color = color;
            }
    }
}
