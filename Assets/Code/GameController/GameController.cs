using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
public class GameController : MonoBehaviour
{
    public static GameController instance;
    public static bool isPlaying=false;

    private bool is_dead;

    public UnityEvent onPlay;
    public UnityEvent onComplete;

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

    public void OnStartLevel ()
    {
        onPlay.Invoke();
    }

    public void OnCompletedLevel()
    {
        Debug.Log("OnCompletedLevel");
        StartOnMenu.MoveToMenu = "Highscore Menu";
        UnityEngine.SceneManagement.SceneManager.LoadScene(2);
        onComplete.Invoke();
    }

    public void OnDeath()
    {
        is_dead = true;
        GameObject.FindGameObjectWithTag("Player").SetActive(false);
        PlayerData.ResetScore();
    }

    public void OnStartPlaying()
    {
        isPlaying = true;
    }

    private void Update()
    {
        if (is_dead)
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                is_dead = false;
                StartOnMenu.MoveToMenu = "Highscore Menu";
                UnityEngine.SceneManagement.SceneManager.LoadScene(1);
            }
        }

    }
}
