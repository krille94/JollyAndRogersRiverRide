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
    [SerializeField] PickUpTrigger trigger = null;

    public float popSize = 2;
    public float popAnimSpeed = 1;
    public float popWaitSpeed = 0.1f;
    float popWait = 0.0f;
    Vector3 normalScale;
    Color normalColor;

    // Start is called before the first frame update
    void Start()
    {
        //float windowWidth = (float)(Screen.width * 3)/(float)(Screen.height * 4);
        //timerText.transform.localPosition = new Vector3(-1+windowWidth,1.8f, 4);
        normalScale = timerText.transform.localScale;
        normalColor = timerText.GetComponent<TextMesh>().color;
        trigger.onLowerTime += LowerTime;

        minutes = 0;
        seconds = 0;
        timerIncrease = 0;
        SetTimerText();
    }

    public void LowerTime(int amount)
    {
        timerText.transform.localScale=normalScale*popSize;
        timerText.GetComponent<TextMesh>().color = Color.white;

        PlayerData.score -= amount;
        seconds -= amount;

        if(seconds<0)
        {
            if (minutes > 0)
            {
                minutes -= 1;
                seconds += 60;
            }
            else
                seconds = 0;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (!GameController.isPlaying)
            return;

        if(timerText.transform.localScale.x>normalScale.x)
        {
            if (popWait < popWaitSpeed)
                popWait += Time.deltaTime;
            else
            {
                timerText.transform.localScale -= ((normalScale*popSize)-normalScale) * (Time.deltaTime * popAnimSpeed);
                timerText.GetComponent<TextMesh>().color += ((normalColor-Color.white) * (Time.deltaTime / popAnimSpeed)); ;

                if (timerText.transform.localScale.x < normalScale.x)
                {
                    timerText.transform.localScale = normalScale;
                    timerText.GetComponent<TextMesh>().color = normalColor;
                    popWait = 0;
                }
            }
        }

        if (minutes < 99 || seconds < 59)
        {
            timerIncrease += Time.deltaTime;

            while (timerIncrease > 1)
            {
                PlayerData.score++;
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
        

        timerText.GetComponent<TextMesh>().text = newText;
    }
}
