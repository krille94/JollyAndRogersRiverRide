using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
public class GameController : MonoBehaviour
{
    public static GameController instance;
    public static bool isPlaying=false;

    private bool clear_game = false;
    private bool loading_game = false;
    private bool loading_to_menu = false;

    public UnityEvent onPlay;
    public UnityEvent onReset;
    public UnityEvent onComplete;
    public UnityEvent onQuitToMainMenu;
    public UnityEvent onLoadToMenu;

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
            LoadToMenu();
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
            loading_to_menu = true;
            Debug.Log("OnCompletedLevel");
            LoadToMenu();
            StartOnMenu.MoveToMenu = "Highscore Menu";
            onComplete.Invoke();
        }
    }

    public void OnQuitToMenu()
    {
        isPlaying = false;
        loading_to_menu = true;
        StartOnMenu.MoveToMenu = "Main Menu";
        LoadToMenu();
        onQuitToMainMenu.Invoke();
    }

    public void OnStartPlaying()
    {
        isPlaying = true;
    }

    public void LoadToMenu()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(2);
        loading_game = false;
    }

    private void Update()
    {
        if (!loading_to_menu)
            return;

        if(loading_game)
        {
            if (UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex == 1)
            {
                loading_to_menu = false;
                onLoadToMenu.Invoke();
            }
        }
        else
        {
            if (UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex == 2)
                loading_game = true;
        }
    }
}
