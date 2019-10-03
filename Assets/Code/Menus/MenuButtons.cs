using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;

public class MenuButtons : MonoBehaviour
{
    public GameObject CurrentMenu;
    public GameObject NextMenu;

    public enum ButtonActions { None, StartGame, ResumeGame, Highscores, ChangeMenu, ChangeOptions, QuitToMainMenu, QuitApplication }
    public ButtonActions buttonAction = 0;

    public enum OptionTypes { None, Audio, Controls, HoldPaddle }
    public OptionTypes optionType = 0;

    private void Start()
    {
    }

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
            GetComponent<TextMesh>().color = new Color(1, 0.75f, 0);
            CurrentMenu.SetActive(false);
            NextMenu.SetActive(true);
        }
        if (buttonAction.ToString() == "ChangeOptions")
        {
            if (optionType.ToString() == "HoldPaddle")
            {
                if (UserSettings.GetAutoPaddle())
                {
                    UserSettings.SetInt("Auto Paddle", 0);
                    GetComponent<TextMesh>().text = "Hold to row: OFF";
                }
                else
                {
                    UserSettings.SetInt("Auto Paddle", 1);
                    GetComponent<TextMesh>().text = "Hold to row: ON";
                }
                UserSettings.ReadSettings();
            }
            if (optionType.ToString() == "Controls")
            {
                if (UserSettings.GetControlScheme()==1)
                {
                    UserSettings.SetInt("Control Scheme", 2);
                    GetComponent<TextMesh>().text = "One Paddle";
                }
                else
                {
                    UserSettings.SetInt("Control Scheme", 1);
                    GetComponent<TextMesh>().text = "Two Paddles";
                }
                UserSettings.ReadSettings();
            }
            if (optionType.ToString() == "Audio")
            {
                //audioSource.Play();
                AudioMixer mixer = GetComponent<AudioSource>().outputAudioMixerGroup.audioMixer;

                float vol = 0;
                mixer.GetFloat("volume", out vol);
                if (vol == -80)
                {
                    vol = 0;
                    mixer.SetFloat("volume", vol);
                    //AudioListener.volume = 0;
                    if (mixer.name == "Music")
                        GetComponent<TextMesh>().text = "Music: ON";
                    else
                        GetComponent<TextMesh>().text = "SFX: ON";
                }
                else
                {
                    vol = -80;
                    mixer.SetFloat("volume", vol);
                    //AudioListener.volume = 1;
                    if (mixer.name == "Music")
                        GetComponent<TextMesh>().text = "Music: OFF";
                    else
                        GetComponent<TextMesh>().text = "SFX: OFF";
                }
                GetComponent<AudioSource>().Play();
                UserSettings.SetFloat(mixer.name, vol);
            }
        }
        if (buttonAction.ToString() == "ChangeMenu")
        {
            GetComponent<TextMesh>().color = new Color(1, 0.75f, 0);
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
