using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseControls : MonoBehaviour
{
    [SerializeField] private GameObject pausePanel = null;

    private GameObject main, options, howtoplay;
    // Start is called before the first frame update
    void Start()
    {
        UserSettings.ReadSettings();

        GameObject temp = GameObject.Find("Music Button");
        if (UserSettings.GetVolume("Music") == -80)
            temp.GetComponent<TextMesh>().text = "Music: OFF";
        temp = GameObject.Find("SFX Button");
        if (UserSettings.GetVolume("SFX") == -80)
            temp.GetComponent<TextMesh>().text = "SFX: OFF";

        main=GameObject.Find("Pause Main Menu");
        options=GameObject.Find("Pause Options Menu");
        howtoplay=GameObject.Find("Pause How To Play Menu");
        if (options == null)
            Debug.LogWarning("'Options' Menu Missing");
        else
            options.SetActive(false);
        if(howtoplay == null)
            Debug.LogWarning("'HowToPlay' Menu Missing");
        else
            howtoplay.SetActive(false);
        if(pausePanel == null)
            Debug.LogWarning("'PausePanel' Menu Missing");
        else
            pausePanel.SetActive(false);

        ContinueGame();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            // Edit this later to check if player is dead or not rather than if player exists
            if (!pausePanel.activeInHierarchy)
            {
                PauseGame();
            }
            else if (pausePanel.activeInHierarchy)
            {
                ContinueGame();
            }
        }
    }

    private void PauseGame()
    {
        Time.timeScale = 0;
        pausePanel.SetActive(true);
        //Disable scripts that still work while timescale is set to 0
    }
    private void ContinueGame()
    {
        Time.timeScale = 1;

        main.SetActive(true);
        options.SetActive(false);
        howtoplay.SetActive(false);

        pausePanel.SetActive(false);
        //enable the scripts again
    }
}
