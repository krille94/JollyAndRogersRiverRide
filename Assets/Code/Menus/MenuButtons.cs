using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuButtons : MonoBehaviour
{
    public bool isStart;
    public bool isHighscores;
    public bool isQuit;
    public AudioSource audioSource;

    void OnMouseUp()
    {
        if (isStart)
        {
            YourScore.ResetScore();
            Time.timeScale = 1;
            SceneManager.LoadScene(2);
        }
        if (isHighscores)
        {
            YourScore.ResetScore();
            SceneManager.LoadScene(3);
        }
        if (isQuit)
        {
            Application.Quit();
        }
    }
}
