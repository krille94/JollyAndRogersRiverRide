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

    private void StartOnSubmenu(string menu)
    {
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
