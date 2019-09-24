using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuButtons : MonoBehaviour
{
    public GameObject CurrentMenu;
    public GameObject NextMenu;

    public enum ButtonActions { None, StartGame, ResumeGame, Highscores, ChangeMenu, ChangeMusic, QuitGame }
    public ButtonActions buttonAction = 0;

    void OnMouseUp()
    {
        if (buttonAction.ToString()=="StartGame")
        {
            YourScore.ResetScore();
            YourScore.playedGame = true;
            Time.timeScale = 1;
            SceneManager.LoadScene(2);
        }
        if (buttonAction.ToString() == "Highscores")
        {
            if (YourScore.playedGame) YourScore.playedGame = false;
            GetComponent<TextMesh>().color = Color.black;
            CurrentMenu.SetActive(false);
            NextMenu.SetActive(true);
        }
        if (buttonAction.ToString() == "ChangeMusic")
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
        if (buttonAction.ToString() == "ChangeMenu")
        {
            GetComponent<TextMesh>().color = Color.black;
            CurrentMenu.SetActive(false);
            NextMenu.SetActive(true);
        }
        if (buttonAction.ToString() == "QuitGame")
        {
            Application.Quit();
        }
    }
}
