using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class HighScoreList : MonoBehaviour
{
    [SerializeField] private GameObject listView = null;
    public GameObject YourScoreText;
    public GameObject ScoreListText;
    public GameObject NameListText;
    public GameObject RestartGame;

    private int YourPlacement=0;

    void OnEnable()
    {
        listView.SetActive(false);
        SaveScore.Load();

        if (YourScore.playedGame)
        {
            YourScore.CalculateScore();
            SortScores();
            CountText();
        }
        else
        {
            RestartGame.SetActive(false);
            ListScores();
            ListNames();
            listView.SetActive(true);
            CountText();

        }
    }

    void SortScores()
    {
        List<Highscore> oldScores = SaveScore.savedGames;
        int newScore = YourScore.score;
        YourPlacement = 0;
        foreach (Highscore g in SaveScore.savedGames)
        {
            if (newScore >= g.score)
            {
                break;
            }
            YourPlacement++;
        }

        Highscore nScore = new Highscore();
        nScore.name = "Player";
        nScore.score = YourScore.score;
        SaveScore.savedGames.Insert(YourPlacement, nScore);
        while (SaveScore.savedGames.Count > 10)
            SaveScore.savedGames.RemoveAt(10);
        SaveScore.Save();

        if (YourPlacement >= 10)
        {
            ListScores();
            ListNames();
            listView.SetActive(true);
        }
    }

    // Update is called once per frame
    void Update()
    {
        /*if (listView.activeInHierarchy)
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                SceneManager.LoadScene(1);
            }
        }
        else*/
        if (!listView.activeInHierarchy)
        {
            if (Input.GetKeyDown(KeyCode.Return))
            {
                if (YourScore.playerName == "") YourScore.playerName = "Player";

                SaveScore.savedGames[YourPlacement].name = YourScore.playerName;
                SaveScore.Save();
                ListScores();
                ListNames();
                listView.SetActive(true);
                YourScore.playedGame = false;
                CountText();
            }
            else if (Input.anyKeyDown)
            {
                AddLetterToName();
                CountText();
            }
        }

    }

    void ListScores()
    {
        string allScores=" ";
        int i = 0;
        foreach (Highscore g in SaveScore.savedGames)
        {
            if (i == YourPlacement)
                allScores += "<color=red>" + (g.score.ToString() + "</color>\n ");
            else
                allScores += (g.score.ToString() + "\n ");
            i++;
        }

        ScoreListText.GetComponent<TextMesh>().text = allScores;
    }

    void ListNames()
    {
        string allScores = " ";
        int i = 1;
        foreach (Highscore g in SaveScore.savedGames)
        {
            if (i < 10) allScores += "  "+i.ToString()+". ";
            else allScores += i.ToString() + ". ";

            if (i == YourPlacement+1)
                allScores += "<color=red>" + (g.name + "</color>\n ");
            else
                allScores += (g.name + "\n ");
            i++;
        }

        NameListText.GetComponent<TextMesh>().text = allScores;
    }

    void CountText()
    {
        if (!listView.activeInHierarchy)
        {
            string setText = "Your score was: " + YourScore.score.ToString() + "\nEnter your name:\n" + (YourPlacement + 1).ToString() + ". " + YourScore.playerName;
            YourScoreText.GetComponent<TextMesh>().text = setText;
        }
        else YourScoreText.GetComponent<TextMesh>().text = " ";
    }

    void AddLetterToName()
    {
        string newLetter = "none";
             if (Input.GetKeyDown(KeyCode.A)) newLetter = "a";
        else if (Input.GetKeyDown(KeyCode.B)) newLetter = "b";
        else if (Input.GetKeyDown(KeyCode.C)) newLetter = "c";
        else if (Input.GetKeyDown(KeyCode.D)) newLetter = "d";
        else if (Input.GetKeyDown(KeyCode.E)) newLetter = "e";
        else if (Input.GetKeyDown(KeyCode.F)) newLetter = "f";
        else if (Input.GetKeyDown(KeyCode.G)) newLetter = "g";
        else if (Input.GetKeyDown(KeyCode.H)) newLetter = "h";
        else if (Input.GetKeyDown(KeyCode.I)) newLetter = "i";
        else if (Input.GetKeyDown(KeyCode.J)) newLetter = "j";
        else if (Input.GetKeyDown(KeyCode.K)) newLetter = "k";
        else if (Input.GetKeyDown(KeyCode.L)) newLetter = "l";
        else if (Input.GetKeyDown(KeyCode.M)) newLetter = "m";
        else if (Input.GetKeyDown(KeyCode.N)) newLetter = "n";
        else if (Input.GetKeyDown(KeyCode.O)) newLetter = "o";
        else if (Input.GetKeyDown(KeyCode.P)) newLetter = "p";
        else if (Input.GetKeyDown(KeyCode.Q)) newLetter = "q";
        else if (Input.GetKeyDown(KeyCode.R)) newLetter = "r";
        else if (Input.GetKeyDown(KeyCode.S)) newLetter = "s";
        else if (Input.GetKeyDown(KeyCode.T)) newLetter = "t";
        else if (Input.GetKeyDown(KeyCode.U)) newLetter = "u";
        else if (Input.GetKeyDown(KeyCode.V)) newLetter = "v";
        else if (Input.GetKeyDown(KeyCode.W)) newLetter = "w";
        else if (Input.GetKeyDown(KeyCode.X)) newLetter = "x";
        else if (Input.GetKeyDown(KeyCode.Y)) newLetter = "y";
        else if (Input.GetKeyDown(KeyCode.Z)) newLetter = "z";

        else if (Input.GetKeyDown(KeyCode.Alpha1)) newLetter = "1";
        else if (Input.GetKeyDown(KeyCode.Alpha2)) newLetter = "2";
        else if (Input.GetKeyDown(KeyCode.Alpha3)) newLetter = "3";
        else if (Input.GetKeyDown(KeyCode.Alpha4)) newLetter = "4";
        else if (Input.GetKeyDown(KeyCode.Alpha5)) newLetter = "5";
        else if (Input.GetKeyDown(KeyCode.Alpha6)) newLetter = "6";
        else if (Input.GetKeyDown(KeyCode.Alpha7)) newLetter = "7";
        else if (Input.GetKeyDown(KeyCode.Alpha8)) newLetter = "8";
        else if (Input.GetKeyDown(KeyCode.Alpha9)) newLetter = "9";
        else if (Input.GetKeyDown(KeyCode.Alpha0)) newLetter = "0";

        else if (Input.GetKeyDown(KeyCode.Space)) newLetter = " ";
        else if (Input.GetKeyDown(KeyCode.Period)) newLetter = ".";
        else if (Input.GetKeyDown(KeyCode.Equals)) newLetter = "+";

        if (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift))
        {
            if (newLetter=="1") newLetter = "!";
            else if (newLetter == "+") newLetter = "?";
            else if (newLetter == "6") newLetter = "&";
            else newLetter = newLetter.ToUpper();
        }

        if (Input.GetKeyDown(KeyCode.Backspace))
        {
            if(YourScore.playerName.Length>0)
                YourScore.playerName = YourScore.playerName.Substring(0,YourScore.playerName.Length-1);
        }

        if (newLetter!="none"&&newLetter!="NONE")
        {
            if(YourScore.playerName.Length<21)
                YourScore.playerName += newLetter;
        }
    }
}
