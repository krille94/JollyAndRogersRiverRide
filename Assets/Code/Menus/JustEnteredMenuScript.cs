using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JustEnteredMenuScript : MonoBehaviour
{
    private GameObject MainMenu;
    private GameObject CharacterSelectMenu;
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
        HighscoreMenu = GameObject.Find("Highscore Menu");
        OptionsMenu = GameObject.Find("Options Menu");
        CreditsMenu = GameObject.Find("Credits Menu");
    }

    private void StartOnSubmenu(string menu)
    {
        MainMenu.SetActive(false);
        CharacterSelectMenu.SetActive(false);
        HighscoreMenu.SetActive(false);
        OptionsMenu.SetActive(false);
        CreditsMenu.SetActive(false);

        if (menu == "Main Menu") MainMenu.SetActive(true);
        if (menu == "Character Select Menu") CharacterSelectMenu.SetActive(true);
        if (menu == "Highscore Menu") HighscoreMenu.SetActive(true);
        if (menu == "Options Menu") OptionsMenu.SetActive(true);
        if (menu == "Credits Menu") CreditsMenu.SetActive(true);
    }
}
