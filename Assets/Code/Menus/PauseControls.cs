﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseControls : MonoBehaviour
{
    [SerializeField] private GameObject pausePanel = null;

    private GameObject main, options, howtoplay;
    // Start is called before the first frame update
    void Start()
    {
        main=GameObject.Find("Pause Main Menu");
        options=GameObject.Find("Pause Options Menu");
        howtoplay=GameObject.Find("Pause How To Play Menu");
        options.SetActive(false);
        howtoplay.SetActive(false);
        pausePanel.SetActive(false);
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
