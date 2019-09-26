using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
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
            //audioSource.Play();
            AudioMixer mixer = GetComponent<AudioSource>().outputAudioMixerGroup.audioMixer;

            float vol=0;
            mixer.GetFloat("volume", out vol);
            if (vol==-80)
            {
                mixer.SetFloat("volume", 0);
                //AudioListener.volume = 0;
                if(mixer.name=="Music")
                    GetComponent<TextMesh>().text="Music: ON";
                else
                    GetComponent<TextMesh>().text = "SFX: ON";
            }
            else
            {
                mixer.SetFloat("volume", -80);
                //AudioListener.volume = 1;
                if (mixer.name == "Music")
                    GetComponent<TextMesh>().text = "Music: OFF";
                else
                    GetComponent<TextMesh>().text = "SFX: OFF";
            }
            GetComponent<AudioSource>().Play();
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
