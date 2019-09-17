using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class HighScoreList : MonoBehaviour
{
    public Text countText;
    public Text HighScoreText;
    // Start is called before the first frame update
    void Start()
    {
        SaveScore.Load();
        CountText();

        SortScores();
        ListScores();
    }

    void SortScores()
    {
        
        List<Highscore> oldScores = SaveScore.savedGames;
        int newScore = YourScore.score;
        int i = 0;
        foreach(Highscore g in SaveScore.savedGames)
        {
            if (newScore > g.score)
            {
                Highscore nScore = new Highscore();
                nScore.name = YourScore.name;
                nScore.score = YourScore.score;
                SaveScore.savedGames.Insert(i, nScore);
                while (SaveScore.savedGames.Count > 10)
                    SaveScore.savedGames.RemoveAt(10);
                SaveScore.Save();
                break;
            }
            i++;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            SceneManager.LoadScene(0);
        }

    }

    void ListScores()
    {
        string allScores=" ";
        foreach (Highscore g in SaveScore.savedGames)
            allScores += (g.name + "   " + g.score.ToString()+"\n ");

        HighScoreText.text = allScores;
    }

    void CountText()
    {
        countText.text = "Your score was: " + YourScore.score.ToString();
    }
}
