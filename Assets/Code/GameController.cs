using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    public static GameController instance;

    private bool is_dead;

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

    public void OnCompletedLevel()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(3);
    }

    public void OnDeath()
    {
        is_dead = true;
        GameObject.FindGameObjectWithTag("Player").SetActive(false);
        YourScore.ResetScore();
    }

    private void Update()
    {
        if (is_dead)
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                is_dead = false;
                UnityEngine.SceneManagement.SceneManager.LoadScene(3);
            }
        }

    }
}
