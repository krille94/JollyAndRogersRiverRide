using UnityEngine;
using System.Collections;

[System.Serializable]
public class YourScore : MonoBehaviour
{
    public static string playerName = "";
    public static int score = 0;
    public static int timeTaken = 6000; // Calculated in seconds
    public static float distanceTraveled = 0; // A value between 0 and 1, determining the distance in percent
    public static int bonusesPickedUp = 0; // Use the gold variable in GoldChestContainer! How to reach it?
    public static float damageTaken = 0; // Calculated in percentage, hull/maxhull

    public YourScore()
    {
    }

    public static void CalculateScore()
    {
        // How do we fairly calculate the time? Need to re-calculate once we know the median time for a run
        score = 10000 / timeTaken;
        score += (bonusesPickedUp * 10);

        if (distanceTraveled >= 1)
            score += 100;
        else
        {
            score = (int)(score * distanceTraveled);
        }
    }

    public static void ResetScore()
    {
        playerName = "";
        score = 0;
        timeTaken = 6000;
        distanceTraveled = 0;
        bonusesPickedUp = 0;
    }
}