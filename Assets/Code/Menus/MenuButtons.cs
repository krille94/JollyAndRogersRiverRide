using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuButtons : MonoBehaviour
{
    public GameObject CurrentMenu;
    public GameObject NextMenu;

    public enum ButtonActions { None, StartGame, ResumeGame, Highscores, ChangeMenu, ChangeMusic, QuitToMainMenu, QuitApplication }
    public ButtonActions buttonAction = 0;

    void OnMouseUp()
    {
        if (buttonAction.ToString()=="StartGame")
        {
            PlayerData.ResetScore();
            PlayerData.playedGame = true;
            Time.timeScale = 1;
            SceneManager.LoadScene(2);
        }
        if (buttonAction.ToString() == "ResumeGame")
        {
            Time.timeScale = 1;
            CurrentMenu.SetActive(false);
        }
        if (buttonAction.ToString() == "Highscores")
        {
            if (PlayerData.playedGame) PlayerData.playedGame = false;
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
        if (buttonAction.ToString() == "QuitToMainMenu")
        {
            PlayerData.ResetScore();
            SceneManager.LoadScene(1);
        }
        if (buttonAction.ToString() == "QuitApplication")
        {
            Application.Quit();
        }
    }
}
