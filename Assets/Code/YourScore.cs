using UnityEngine;
using System.Collections;

[System.Serializable]
public class YourScore : MonoBehaviour
{

    //public static Highscore current;
    public static string playerName = "";
    public static int score = 0;

    public YourScore()
    {
        playerName = "";
        score = 0;
    }
}