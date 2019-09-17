using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerScript : MonoBehaviour
{
    public Text countText;
    // Start is called before the first frame update
    void Start()
    {

        CountText();
    }

    // Update is called once per frame
    void Update()
    {

        if (Input.GetKeyDown(KeyCode.W))
        {
            YourScore.score++;
            CountText();
        }

    }

    void CountText()
    {
        countText.text = "Count: " + YourScore.score.ToString();
    }
}
