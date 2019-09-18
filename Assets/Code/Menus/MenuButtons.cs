﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuButtons : MonoBehaviour
{
    public bool isStart;
    public bool isQuit;
    public AudioSource audioSource;

    void OnMouseUp()
    {
        if (isStart)
        {
            YourScore.score = 0;
            SceneManager.LoadScene(1);
        }
        if (isQuit)
        {
            Application.Quit();
        }
    }
}
