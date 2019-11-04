using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
public class GameController : MonoBehaviour
{
    public static GameController instance;
    public static bool isPlaying=false;

    private bool clear_game=false;

    public UnityEvent onPlay;
    public UnityEvent onReset;
    public UnityEvent onComplete;
    public UnityEvent onQuitToMainMenu;

    private void Start()
    {
        if(instance != null)
        {
            Destroy(gameObject);
            return;
        }
        instance = this;
        DontDestroyOnLoad(gameObject);

        Time.timeScale = 1;
    }

    public bool GetClearGame() { return clear_game; }

    public void OnStartLevel ()
    {
        if(!isPlaying)
            onPlay.Invoke();
        else
        {
            UnityEngine.SceneManagement.SceneManager.LoadScene(1);
            onReset.Invoke();
        }
    }

    public void OnVictory()
    {
        if (!clear_game)
        {
            Debug.Log("OnVictory");
            clear_game = true;
        }
    }

    public void OnCompletedLevel()
    {
        if (isPlaying)
        {
            clear_game = false;
            isPlaying = false;
            Debug.Log("OnCompletedLevel");
            StartOnMenu.MoveToMenu = "Highscore Menu";
            onComplete.Invoke();
            UnityEngine.SceneManagement.SceneManager.LoadScene(1);
        }
    }

    public void OnQuitToMenu()
    {
        isPlaying = false;
        StartOnMenu.MoveToMenu = "Main Menu";
        onQuitToMainMenu.Invoke();
        UnityEngine.SceneManagement.SceneManager.LoadScene(1);
    }

    public void OnStartPlaying()
    {
        isPlaying = true;
    }

    private void Update()
    {
    }
}
