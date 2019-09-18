using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseButtons : MonoBehaviour
{
    public bool isResume;
    public bool isOptions;
    public bool isQuit;
    public AudioSource audioSource;
    [SerializeField] private GameObject pausePanel = null;

    void OnMouseUp()
    {
        if (isResume)
        {
            Time.timeScale = 1;
            pausePanel.SetActive(false);
        }

        if (isOptions) { }

        if (isQuit)
        {
            SceneManager.LoadScene(0);
        }
    }
}
