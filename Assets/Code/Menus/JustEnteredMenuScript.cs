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
    private GameObject ResetMenu;
    private List<GameObject> options = new List<GameObject>();

    // Start is called before the first frame update
    void Start()
    {
        CollectMenus();

        StartOnSubmenu(StartOnMenu.MoveToMenu);
    }

    public void SetSubmenu()
    {
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
        ResetMenu = GameObject.Find("Reset Warning");

        options.Add(GameObject.Find("Music Volume"));
        options.Add(GameObject.Find("SFX Volume"));
        options.Add(GameObject.Find("Auto Paddle"));
    }

    private void SetOptionValues()
    {
        UserSettings.ReadSettings();

        if(UserSettings.GetVolume("Music")==-80)
            options[0].GetComponent<TextMesh>().text = "Music: OFF";
        else
            options[0].GetComponent<TextMesh>().text = "Music: ON";
        if (UserSettings.GetVolume("SFX") == -80)
            options[1].GetComponent<TextMesh>().text = "SFX: OFF";
        else
            options[1].GetComponent<TextMesh>().text = "SFX: ON";
        /*
        option = GameObject.Find("Reverse Controls");
        if (UserSettings.GetReversedControls() == true)
            option.GetComponent<TextMesh>().text = "Controls: reverse";
        option = GameObject.Find("Control Scheme");
        if (UserSettings.GetControlScheme() == 2)
            option.GetComponent<TextMesh>().text = "Old Control Scheme";*/
        if (UserSettings.GetAutoPaddle()==false)
            options[2].GetComponent<TextMesh>().text = "Auto Paddle: OFF";
        else
            options[2].GetComponent<TextMesh>().text = "Auto Paddle: ON";
    }

    private void StartOnSubmenu(string menu)
    {
        SetOptionValues();

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

        ResetMenu.SetActive(false);
    }
}
