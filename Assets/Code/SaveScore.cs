using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using UnityEngine;

public static class SaveScore
{
    public static List<Highscore> savedGames = new List<Highscore>();

    public static int BestScore;

    public static void Save()
    {
        if(savedGames.Count==0)
        {
            Highscore nScore = new Highscore();
            nScore.name = "Player";
            nScore.score = YourScore.score;
            SaveScore.savedGames.Add(nScore);
        }
        //savedGames.Add(Highscore.current);
        BinaryFormatter bf = new BinaryFormatter();
        //FileStream file = File.Create(Path.Combine(Application.persistentDataPath, "Highscore.gd"));
        FileStream file = File.Create("Highscore.gd");
        bf.Serialize(file, SaveScore.savedGames);
        //bf.Serialize(file, YourScore);
        file.Close();
    }

    public static void Load()
    {
        //if (File.Exists(Path.Combine(Application.persistentDataPath, "Highscore.gd")))

        if (File.Exists("Highscore.gd"))
        {
            BinaryFormatter bf = new BinaryFormatter();
            //FileStream file = File.Open(Path.Combine(Application.persistentDataPath, "Highscore.gd"), FileMode.Open);
            FileStream file = File.Open("Highscore.gd", FileMode.Open);
            SaveScore.savedGames = (List<Highscore>)bf.Deserialize(file);
            //YourScore = (int)bf.Deserialize(file);
            file.Close();
        }
        else
        {
            Highscore nScore = new Highscore();
            nScore.name = "Default";
            nScore.score = YourScore.score;
            SaveScore.savedGames.Add(nScore);
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Create("Highscore.gd");
            bf.Serialize(file, SaveScore.savedGames);
            file.Close();
        }

        //HighScore.score = YourScore;
    }
}
