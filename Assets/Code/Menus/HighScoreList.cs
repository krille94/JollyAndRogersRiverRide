using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class HighScoreList : MonoBehaviour
{
    [SerializeField] private GameObject listView = null;
    [SerializeField] private GameObject NewScoreView = null;
    public GameObject YourScoreText;
    public GameObject ScoreListText;
    public GameObject NameListText;
    public GameObject RestartGame;

    [SerializeField] AudioClip youGotHighscoreClip;
    [SerializeField] AudioClip postGameHighscoreClip;
    [SerializeField] AudioClip typeNameClip;

    private int YourPlacement=0;
    private int ScoresPerText = 10;
    List<char> letters = new List<char>();
    private int currentLetter;
    bool canChangeCurrentLetter;

    new AudioSource audio;

    void OnEnable()
    {
        if (gameObject.GetComponent<AudioSource>())
            audio = gameObject.GetComponent<AudioSource>();
        else
            audio = gameObject.AddComponent<AudioSource>();

        AudioMixer mix = Resources.Load("AudioMixers/Sound Effects") as AudioMixer;
        audio.outputAudioMixerGroup = mix.FindMatchingGroups("Master")[0];

        NewScoreView.SetActive(true);
        listView.SetActive(false);
        SaveScore.Load();

        if (PlayerData.playedGame && PlayerData.score > 0)
        {
            ControllerLetterToName();
            PlayerData.CalculateScore();
            SortScores();
            CountText();
        }
        else
        {
            YourPlacement = 11;
            RestartGame.SetActive(false);
            ListScores();
            ListNames();
            NewScoreView.SetActive(false);
            listView.SetActive(true);
            CountText();

        }
    }

    void SortScores()
    {
        List<Highscore> oldScores = SaveScore.savedGames;
        int newScore = PlayerData.score;
        YourPlacement = 0;
        foreach (Highscore g in SaveScore.savedGames)
        {
            if (newScore <= g.score)
            {
                audio.PlayOneShot(youGotHighscoreClip);
                break;
            }
            YourPlacement++;
        }

        if (YourPlacement >= 10)
        {
            ListScores();
            ListNames();
            NewScoreView.SetActive(false);
            listView.SetActive(true);
            audio.PlayOneShot(postGameHighscoreClip);
        }
    }
    void SaveYourScore()
    { 
        Highscore nScore = new Highscore();
        nScore.name = "Player";
        nScore.score = PlayerData.score;
        SaveScore.savedGames.Insert(YourPlacement, nScore);
        while (SaveScore.savedGames.Count > 10)
            SaveScore.savedGames.RemoveAt(10);
        SaveScore.Save();
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
            if (Input.GetAxis("Player_One_Joystick_Vertical") > 0)
            {
                if (canChangeCurrentLetter)
                {
                    canChangeCurrentLetter = false;
                    currentLetter++;
                    if (currentLetter >= letters.Count) currentLetter = 0;
                }
            }
            else if (Input.GetAxis("Player_One_Joystick_Vertical") < 0)
            {
                if (canChangeCurrentLetter)
                {
                    canChangeCurrentLetter = false;
                    currentLetter--;
                    if (currentLetter < 0) currentLetter = letters.Count - 1;
                }
            }
            else
                canChangeCurrentLetter = true;

            if (Input.GetKeyDown(KeyCode.Return)|| Input.GetButtonUp("Player_One_Pause"))
            {
                if (PlayerData.playerName == "") PlayerData.playerName = "Player";

                SaveYourScore();

                SaveScore.savedGames[YourPlacement].name = PlayerData.playerName;
                SaveScore.Save();
                ListScores();
                ListNames();
                NewScoreView.SetActive(false);
                listView.SetActive(true);
                PlayerData.playedGame = false;
                audio.PlayOneShot(postGameHighscoreClip);
            }
            else if (Input.anyKeyDown)
            {
                AddLetterToName();
            }
            CountText();
        }

    }

    string ConvertToTime(int score)
    {
        string time = "";
        int minutes = 0;
        while(score>=60)
        {
            minutes++;
            score -= 60;
        }

        if (minutes < 10)
            time += "0";
        time += minutes.ToString() + ":";

        if (score < 10)
            time += "0";
        time += score.ToString();

        return time;
    }

    void ListScores()
    {
        string allScores=" ";
        int i = 0;
        foreach (Highscore g in SaveScore.savedGames)
        {
            string score = ConvertToTime(g.score);
            if (i == YourPlacement)
                allScores += "<color=red>" + score + "</color>\n ";
            else
                allScores += score + "\n ";
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
            if (i < 10) allScores += " "+i.ToString()+". ";
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
            string score = ConvertToTime(PlayerData.score);
            string setText = "Your score was: " + score + "\nEnter your name:\n" + (YourPlacement + 1).ToString() + ". " + PlayerData.playerName+letters[currentLetter];
            YourScoreText.GetComponent<TextMesh>().text = setText;
        }
        else
        {
            NewScoreView.SetActive(false);
        }
    }

    void ControllerLetterToName()
    {
        letters.Add('_');
        letters.Add('a');
        letters.Add('b');
        letters.Add('c');
        letters.Add('d');
        letters.Add('e');
        letters.Add('f');
        letters.Add('g');
        letters.Add('h');
        letters.Add('i');
        letters.Add('j');
        letters.Add('k');
        letters.Add('l');
        letters.Add('m');
        letters.Add('n');
        letters.Add('o');
        letters.Add('p');
        letters.Add('q');
        letters.Add('r');
        letters.Add('s');
        letters.Add('t');
        letters.Add('u');
        letters.Add('v');
        letters.Add('w');
        letters.Add('x');
        letters.Add('y');
        letters.Add('z');
        letters.Add('.');
        letters.Add('!');
        letters.Add('?');
        letters.Add('-');
        letters.Add('+');
        letters.Add('&');
        letters.Add('1');
        letters.Add('2');
        letters.Add('3');
        letters.Add('4');
        letters.Add('5');
        letters.Add('6');
        letters.Add('7');
        letters.Add('8');
        letters.Add('9');
        letters.Add('0');
        letters.Add(' ');
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

        else if (Input.GetButtonDown("Player_One_Paddle_Forward")) newLetter = letters[currentLetter].ToString();

        if (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift))
        {
            if (newLetter=="1") newLetter = "!";
            else if (newLetter == "+") newLetter = "?";
            else if (newLetter == "6") newLetter = "&";
            else newLetter = newLetter.ToUpper();
        }

        if (Input.GetKeyDown(KeyCode.Backspace)|| Input.GetButtonDown("Player_One_Paddle_Back"))
        {
            if(PlayerData.playerName.Length>0)
                PlayerData.playerName = PlayerData.playerName.Substring(0,PlayerData.playerName.Length-1);
        }

        if (newLetter!="none"&&newLetter!="NONE")
        {
            if(PlayerData.playerName.Length<21)
            {
                PlayerData.playerName += newLetter;
                currentLetter = 0;
                audio.PlayOneShot(typeNameClip);
            }
        }
    }
}
