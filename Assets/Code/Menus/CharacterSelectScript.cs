﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterSelectScript : MonoBehaviour
{
    // Swapped name spots due to character boat spots being switched
    private enum CharNames { Roger, Jolly };

    public GameObject Player1Icon;
    public GameObject Player2Icon;
    public GameObject Player1Text;
    public GameObject Player2Text;

    public GameObject StartGameButton;

    bool Player1Chosen=false;
    bool Player2Chosen=false;

    int Player1Pos;
    int Player2Pos;

    int AmountOfChars;

    Vector3 portraitDistance;

    new AudioSource audio;
    [SerializeField] AudioClip[] onCharacterClip;

    private void OnDisable()
    {
        StartGameButton.SetActive(false);
        Player1Chosen = false;
        Player1Icon.SetActive(true);
        Player1Text.SetActive(false);
        Player2Chosen = false;
        Player2Icon.SetActive(true);
        Player2Text.SetActive(false);
    }

    // Start is called before the first frame update
    void Start()
    {
        if (gameObject.GetComponent<AudioSource>())
            audio = gameObject.GetComponent<AudioSource>();
        else
            audio = gameObject.AddComponent<AudioSource>();

        Player1Pos = 0;
        Player2Pos = 0;
        AmountOfChars = 2;

        portraitDistance=new Vector3(6,0,0);

        CharNames tempName;
        for(int i=0; i<AmountOfChars; i++)
        {
            tempName = (CharNames)i;
            if(PlayerData.player1Character == tempName.ToString())
            {
                Player1Icon.transform.position += portraitDistance*i;
                Player1Text.transform.position += portraitDistance*i;
                Player1Pos+=i;
            }
            if (PlayerData.player2Character == tempName.ToString())
            {
                Player2Icon.transform.position += portraitDistance * i;
                Player2Text.transform.position += portraitDistance * i;
                Player2Pos += i;
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Player_One_Paddle_Forward"))
        {
            if (!Player2Chosen || Player1Pos != Player2Pos)
            {
                CharNames name = (CharNames)Player1Pos;
                PlayerData.player1Character = name.ToString();

                Player1Chosen = true;
                Player1Icon.SetActive(false);
                Player1Text.SetActive(true);

                if (Player1Pos == 0)
                    audio.pitch = 2.5f;
                else
                    audio.pitch = 0.75f;
                audio.PlayOneShot(onCharacterClip[Player1Pos]);

                if (Player1Chosen && Player2Chosen)
                    StartGameButton.SetActive(true);
            }
        }
        else if (Input.GetButtonDown("Player_One_Paddle_Back"))
        {
            StartGameButton.SetActive(false);
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
                PlayerData.player2Character = name.ToString();

                Player2Chosen = true;
                Player2Icon.SetActive(false);
                Player2Text.SetActive(true);

                if (Player2Pos == 0)
                    audio.pitch = 2.5f;
                else
                    audio.pitch = 0.75f;
                audio.PlayOneShot(onCharacterClip[Player2Pos]);

                if (Player1Chosen && Player2Chosen)
                    StartGameButton.SetActive(true);
            }
        }
        else if (Input.GetButtonDown("Player_Two_Paddle_Back"))
        {
            StartGameButton.SetActive(false);
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
