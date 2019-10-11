using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TimerScript : MonoBehaviour
{
    public GameObject timerText;
    float minutes = 0;
    float seconds = 0;
    float timerIncrease = 0;

    // Start is called before the first frame update
    void Start()
    {
        float windowWidth = (float)(Screen.width * 3)/(float)(Screen.height * 4);
        timerText.transform.localPosition = new Vector3(-1+windowWidth,1.8f, 4);

        minutes = 0;
        seconds = 0;
        timerIncrease = 0;
        SetTimerText();
    }

    // Update is called once per frame
    void Update()
    {
        if (minutes < 99 || seconds < 59)
        {
            timerIncrease += Time.deltaTime;

            while (timerIncrease > 1)
            {
                PlayerData.timeTaken++;
                seconds += 1;
                timerIncrease -= 1;
                if (seconds >= 60)
                {
                    minutes += 1;
                    seconds -= 60;
                }
            }
        }
        SetTimerText();
    }

    void SetTimerText()
    {
        string newText="";
        if (minutes < 10)
            newText += "0" + minutes.ToString();
        else
            newText += minutes.ToString();

        if (seconds < 10)
            newText += ":0" + seconds.ToString();
        else
            newText += ":" + seconds.ToString();
        

        timerText.GetComponent<TextMesh>().text = "Timer: " + newText;
    }
}
