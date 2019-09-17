using UnityEngine;
using System.Collections;

[System.Serializable]
public class YourScore : MonoBehaviour
{

    //public static Highscore current;
    public static string name;
    public static int score;

    public YourScore()
    {
        name = "Player";
        score = 0;
    }
}