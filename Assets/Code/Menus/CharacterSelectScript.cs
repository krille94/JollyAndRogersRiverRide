using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterSelectScript : MonoBehaviour
{
    private enum CharNames { Jolly, Roger };

    public GameObject Player1Icon;
    public GameObject Player2Icon;
    public GameObject Player1Text;
    public GameObject Player2Text;

    bool Player1Chosen=false;
    bool Player2Chosen=false;

    int Player1Pos;
    int Player2Pos;

    int AmountOfChars;

    Vector3 portraitDistance;

    // Start is called before the first frame update
    void Start()
    {
        Player1Pos = 0;
        Player2Pos = 0;
        AmountOfChars = 2;

        portraitDistance=new Vector3(6,0,0);

    }

    // Update is called once per frame
    void Update()
    {

        if (Input.GetButtonDown("Player_One_Paddle_Forward"))
        {
            if (!Player2Chosen || Player1Pos != Player2Pos)
            {
                CharNames name = (CharNames)Player1Pos;
                Debug.LogWarning("Player 1 chose "+name.ToString());
                PlayerData.player1Character = name.ToString();

                Player1Chosen = true;
                Player1Icon.SetActive(false);
                Player1Text.SetActive(true);
            }
        }
        else if (Input.GetButtonDown("Player_One_Paddle_Back"))
        {
            Player1Chosen = false;
            Player1Icon.SetActive(true);
            Player1Text.SetActive(false);
        }
        else if(!Player1Chosen)
        {
            if (Input.GetButtonDown("Player_One_Paddle_Right"))
            {
                if (Player1Pos + 1 < AmountOfChars)
                {
                    Player1Icon.transform.position += portraitDistance;
                    Player1Text.transform.position += portraitDistance;
                    Player1Pos++;
                }
            }
            else if (Input.GetButtonDown("Player_One_Paddle_Left"))
            {
                if (Player1Pos > 0)
                {
                    Player1Icon.transform.position -= portraitDistance;
                    Player1Text.transform.position -= portraitDistance;
                    Player1Pos--;
                }
            }
        }


        if (Input.GetButtonDown("Player_Two_Paddle_Forward"))
        {
            if (!Player1Chosen || Player1Pos != Player2Pos)
            {
                CharNames name = (CharNames)Player2Pos;
                Debug.LogWarning("Player 2 chose " + name.ToString());
                PlayerData.player2Character = name.ToString();

                Player2Chosen = true;
                Player2Icon.SetActive(false);
                Player2Text.SetActive(true);
            }
        }
        else if (Input.GetButtonDown("Player_Two_Paddle_Back"))
        {
            Player2Chosen = false;
            Player2Icon.SetActive(true);
            Player2Text.SetActive(false);
        }
        else if (!Player2Chosen)
        {
            if (Input.GetButtonDown("Player_Two_Paddle_Right"))
            {
                if (Player2Pos + 1 < AmountOfChars)
                {
                    Player2Icon.transform.position += portraitDistance;
                    Player2Text.transform.position += portraitDistance;
                    Player2Pos++;
                }
            }
            else if (Input.GetButtonDown("Player_Two_Paddle_Left"))
            {
                if (Player2Pos > 0)
                {
                    Player2Icon.transform.position -= portraitDistance;
                    Player2Text.transform.position -= portraitDistance;
                    Player2Pos--;
                }
            }
        }
    }
}
