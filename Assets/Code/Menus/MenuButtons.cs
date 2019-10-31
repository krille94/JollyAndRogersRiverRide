using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;

public class MenuButtons : MonoBehaviour
{
    [HideInInspector] public GameObject CurrentMenu;
    [HideInInspector] public GameObject NextMenu;

    public enum ButtonActions { None, StartGame, ResumeGame, Highscores, ChangeMenu, ChangeOptions, QuitToMainMenu, QuitApplication }
    [HideInInspector] public ButtonActions buttonAction = 0;

    public enum OptionTypes { None, Audio, Controls, ReverseControls, AutoPaddle }
    [HideInInspector] public OptionTypes optionType = 0;

    private void Start()
    {
    }

    private void OnMouseDown()
    {
        //if (GetComponent<AudioSource>())
        //    GetComponent<AudioSource>().Play();
    }

    void OnMouseUp()
    {
        PressButton();
    }

    public void PressButton()
    {
        if (GetComponent<AudioSource>())
            GetComponent<AudioSource>().Play();

        if (buttonAction.ToString()=="StartGame")
        {
            GameObject.Find("Character Select Script").GetComponent<CharacterSelectScript>().SetStartedGame(true);
            PlayerData.ResetScore();
            PlayerData.playedGame = true;
            Time.timeScale = 1;

            GameController.instance.OnStartLevel();
            //GameObject.Find("2D Menu Assets").SetActive(false);
            //SceneManager.LoadScene(2);
        }
        if (buttonAction.ToString() == "ResumeGame")
        {
            Time.timeScale = 1;
            CurrentMenu.SetActive(false);
        }
        if (buttonAction.ToString() == "Highscores")
        {
            if (PlayerData.playedGame) PlayerData.playedGame = false;
            //GetComponent<TextMesh>().color = new Color(1, 0.75f, 0);
            CurrentMenu.SetActive(false);
            NextMenu.SetActive(true);
        }
        if (buttonAction.ToString() == "ChangeOptions")
        {
            if (optionType.ToString() == "AutoPaddle")
            {
                if (UserSettings.GetAutoPaddle())
                {
                    UserSettings.SetInt("Auto Paddle", 0);
                    GetComponent<TextMesh>().text = "Auto Paddle: OFF";
                }
                else
                {
                    UserSettings.SetInt("Auto Paddle", 1);
                    GetComponent<TextMesh>().text = "Auto Paddle: ON";
                }
                UserSettings.ReadSettings();
            }
            if (optionType.ToString() == "ReverseControls")
            { 
                if (UserSettings.GetReversedControls())
                {
                    UserSettings.SetInt("Reversed Controls", 0);
                    GetComponent<TextMesh>().text = "Controls: normal";
                }
                else
                {
                    UserSettings.SetInt("Reversed Controls", 1);
                    GetComponent<TextMesh>().text = "Controls: reverse";
                }
                UserSettings.ReadSettings();
            }
            if (optionType.ToString() == "Controls")
            {
                if (UserSettings.GetControlScheme()==1)
                {
                    UserSettings.SetInt("Control Scheme", 2);
                    GetComponent<TextMesh>().text = "Old Control Scheme";
                }
                else
                {
                    UserSettings.SetInt("Control Scheme", 1);
                    GetComponent<TextMesh>().text = "New Control Scheme";
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
                    //AudioListener.volume = 0;
                    if (mixer.name == "Music")
                    {
                        vol = -15;
                        GetComponent<TextMesh>().text = "Music: ON";
                    }
                    else
                    {
                        vol = 0;
                        GetComponent<TextMesh>().text = "SFX: ON";
                    }
                    mixer.SetFloat("volume", vol);
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
            //GetComponent<TextMesh>().color = new Color(1, 0.75f, 0);
            CurrentMenu.SetActive(false);
            NextMenu.SetActive(true);
        }
        if (buttonAction.ToString() == "QuitToMainMenu")
        {
            PlayerData.ResetScore();
            StartOnMenu.MoveToMenu = "Main Menu";
            GameController.instance.OnQuitToMenu();
            SceneManager.LoadScene(1);
        }
        if (buttonAction.ToString() == "QuitApplication")
        {
            Application.Quit();
        }
    }
}
