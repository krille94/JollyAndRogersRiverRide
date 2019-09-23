using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuButtons : MonoBehaviour
{
    public GameObject CurrentMenu;
    public GameObject NextMenu;

    public bool isStart;
    public bool isHighscores;
    public bool isChangeMenu;
    public bool isChangeMusic;
    public bool isQuit;
    
    void OnMouseUp()
    {
        if (isStart)
        {
            YourScore.ResetScore();
            YourScore.playedGame = true;
            Time.timeScale = 1;
            SceneManager.LoadScene(2);
        }
        if (isHighscores)
        {
            if (YourScore.playedGame) YourScore.playedGame = false;
            GetComponent<TextMesh>().color = Color.black;
            CurrentMenu.SetActive(false);
            NextMenu.SetActive(true);
        }
        if (isChangeMusic)
        {
            GetComponent<AudioSource>().Play();
            //audioSource.Play();
            if(AudioListener.volume==1)
            {
                AudioListener.volume = 0;
                GetComponent<TextMesh>().text="Audio: OFF";
            }
            else
            {
                AudioListener.volume = 1;
                GetComponent<TextMesh>().text = "Audio: ON";
            }
        }
        if (isChangeMenu)
        {
            GetComponent<TextMesh>().color = Color.black;
            CurrentMenu.SetActive(false);
            NextMenu.SetActive(true);
        }
        if (isQuit)
        {
            Application.Quit();
        }
    }
}
