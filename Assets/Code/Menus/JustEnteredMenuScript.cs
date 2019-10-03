using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JustEnteredMenuScript : MonoBehaviour
{
    private GameObject MainMenu;
    private GameObject CharacterSelectMenu;
    private GameObject HowToPlayMenu;
    private GameObject HighscoreMenu;
    private GameObject OptionsMenu;
    private GameObject CreditsMenu;

    // Start is called before the first frame update
    void Start()
    {
        CollectMenus();
        StartOnSubmenu(StartOnMenu.MoveToMenu);
    }

    private void CollectMenus()
    {
        MainMenu = GameObject.Find("Main Menu");
        CharacterSelectMenu = GameObject.Find("Character Select Menu");
        HowToPlayMenu = GameObject.Find("How To Play Menu");
        HighscoreMenu = GameObject.Find("Highscore Menu");
        OptionsMenu = GameObject.Find("Options Menu");
        CreditsMenu = GameObject.Find("Credits Menu");
    }

    private void SetOptionValues()
    {
        UserSettings.ReadSettings();

        GameObject option = GameObject.Find("Music Volume");
        if(UserSettings.GetVolume("Music")==-80)
            option.GetComponent<TextMesh>().text = "Music: OFF";
        option = GameObject.Find("SFX Volume");
        if (UserSettings.GetVolume("SFX") == -80)
            option.GetComponent<TextMesh>().text = "SFX: OFF";

        option = GameObject.Find("Hold Paddle");
        if (UserSettings.GetAutoPaddle() == true)
            option.GetComponent<TextMesh>().text = "Hold to row: ON";
        option = GameObject.Find("Control Scheme");
        if (UserSettings.GetControlScheme() == 2)
            option.GetComponent<TextMesh>().text = "One Paddle";
    }

    private void StartOnSubmenu(string menu)
    {
        SetOptionValues();

        if (menu != "Main Menu") GameObject.Find("Logo").SetActive(false);

        if (menu == "Main Menu") MainMenu.SetActive(true);
        else MainMenu.SetActive(false);

        if (menu == "Character Select Menu") CharacterSelectMenu.SetActive(true);
        else CharacterSelectMenu.SetActive(false);

        if (menu == "Highscore Menu") HighscoreMenu.SetActive(true);
        else HighscoreMenu.SetActive(false);

        if (menu == "Options Menu") OptionsMenu.SetActive(true);
        else OptionsMenu.SetActive(false);

        if (menu == "Credits Menu") CreditsMenu.SetActive(true);
        else CreditsMenu.SetActive(false);

        if (menu == "How To Play Menu") HowToPlayMenu.SetActive(true);
        else HowToPlayMenu.SetActive(false);
    }
}
