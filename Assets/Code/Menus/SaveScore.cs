using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using UnityEngine;

public static class SaveScore
{
    public static List<Highscore> savedGames = new List<Highscore>();

    public static int BestScore;
    private static string filename = "Highscore.gd";

    public static void Save()
    {
        if(savedGames.Count==0)
        {
            for (int i = 0; i < 10; i++)
                SetDefaultHighscores(i);
        }
        //savedGames.Add(Highscore.current);
        BinaryFormatter bf = new BinaryFormatter();
        //FileStream file = File.Create(Path.Combine(Application.persistentDataPath, "Highscore.gd"));
        FileStream file = File.Create(filename);
        bf.Serialize(file, SaveScore.savedGames);
        //bf.Serialize(file, YourScore);
        file.Close();
    }

    public static void Load()
    {
        //if (File.Exists(Path.Combine(Application.persistentDataPath, "Highscore.gd")))

        if (File.Exists(filename))
        {
            BinaryFormatter bf = new BinaryFormatter();
            //FileStream file = File.Open(Path.Combine(Application.persistentDataPath, "Highscore.gd"), FileMode.Open);
            FileStream file = File.Open(filename, FileMode.Open);
            SaveScore.savedGames = (List<Highscore>)bf.Deserialize(file);
            //YourScore = (int)bf.Deserialize(file);
            file.Close();
        }
        else
        {
            for (int i = 0; i < 10; i++)
                SetDefaultHighscores(i);
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Create(filename);
            bf.Serialize(file, SaveScore.savedGames);
            file.Close();
        }

        //HighScore.score = YourScore;
    }

    public static void Reset()
    {
        savedGames.Clear();
        for (int i = 0; i < 10; i++)
            SetDefaultHighscores(i);
        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Create(filename);
        bf.Serialize(file, SaveScore.savedGames);
        file.Close();
    }

    private static void SetDefaultHighscores(int i)
    {
        Highscore nScore = new Highscore();


        if (GameObject.Find("Default Scores"))
        {
            DefaultHighscores dh = GameObject.Find("Default Scores").GetComponent<DefaultHighscores>();

            if (dh.defaultSaves.Count > i)
            {
                nScore.score = dh.defaultSaves[i].score;
                nScore.name = dh.defaultSaves[i].name;
                SaveScore.savedGames.Add(nScore);
                return;
            }
        }

        {
            nScore.score = 90 + (i * 10);

            if (i == 9) nScore.name = "haha I suck";
            else if (i == 8) nScore.name = "placeholder guy 14";
            else if (i == 7) nScore.name = "It's fine";
            else if (i == 6) nScore.name = "WELL... IT'S THERE NOW";
            else if (i == 5) nScore.name = "wait was caps planned?";
            else if (i == 4) nScore.name = "TEXT LIMIT BABY!!!!!!!!!!!!!!!";
            else if (i == 3) nScore.name = "River Something";
            else if (i == 2) nScore.name = "River Ride?";
            else if (i == 1) nScore.name = "River Run";
            else if (i == 0) nScore.name = "Jolly & Roger";
        }

        SaveScore.savedGames.Add(nScore);
    }
}
