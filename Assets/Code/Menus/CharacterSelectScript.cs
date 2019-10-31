using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class CharacterSelectScript : MonoBehaviour
{
    // Swapped name spots due to character boat spots being switched
    private enum CharNames { Roger, Jolly };

    public GameObject Player1Icon;
    public GameObject Player2Icon;
    public GameObject Player1Text;
    public GameObject Player2Text;

    public GameObject StartGameButton;
    public GameObject ReturnButton;

    bool Player1Chosen =false;
    bool Player2Chosen=false;
    bool onReturn = false;

    int Player1Pos;
    int Player2Pos;

    int AmountOfChars;

    public Vector3 portraitDistance;

    new AudioSource audioRoger;
    new AudioSource audioJolly;
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
            audioRoger = gameObject.GetComponent<AudioSource>();
        else
        {
            audioRoger = gameObject.AddComponent<AudioSource>();
            audioJolly = gameObject.AddComponent<AudioSource>();
        }
        AudioMixer mix = Resources.Load("AudioMixers/Sound Effects") as AudioMixer;
        audioRoger.outputAudioMixerGroup = mix.FindMatchingGroups("Master")[0];
        audioJolly.outputAudioMixerGroup = mix.FindMatchingGroups("Master")[0];

        audioJolly.pitch = 2.5f;
        audioRoger.pitch = 0.75f;


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
        if (ReturnButton.GetComponent<MouseHover>().GetSelected())
        {
            ReturnButton.GetComponent<MouseHover>().SetSelected(false);
            ReturnButton.GetComponent<TextMesh>().color = Color.white;
        }

        if (Input.GetAxis("Player_One_Joystick_Vertical") > 0)
        {
            if (!Player1Chosen)
            {
                onReturn = true;
                ReturnButton.GetComponent<TextMesh>().color = Color.white;
                Player1Icon.SetActive(false);
            }
        }
        else if (Input.GetAxis("Player_One_Joystick_Vertical") < 0)
        {
            if (onReturn)
            {
                onReturn = false;
                ReturnButton.GetComponent<TextMesh>().color = new Color(1, 0.75f, 0);
                Player1Icon.SetActive(true);
            }
        }

        if (Input.GetButtonDown("Player_One_Paddle_Back"))
        {
            if (!onReturn)
            {
                if (Player1Chosen == false)
                {
                    if (!Player2Chosen || Player1Pos != Player2Pos)
                    {
                        CharNames name = (CharNames)Player1Pos;
                        PlayerData.player1Character = name.ToString();

                        Player1Chosen = true;
                        Player1Icon.SetActive(false);
                        Player1Text.SetActive(true);

                        if (Player1Pos == 0)
                            audioJolly.PlayOneShot(onCharacterClip[Player1Pos]);
                        else
                            audioRoger.PlayOneShot(onCharacterClip[Player1Pos]);


                        if (Player1Chosen && Player2Chosen)
                        {
                            StartGameButton.SetActive(true);
                            ReturnButton.GetComponent<TextMesh>().color = new Color(0.85f, 0.65f, 0);
                        }
                    }
                }
                else
                {
                    StartGameButton.SetActive(false);
                    ReturnButton.GetComponent<TextMesh>().color = new Color(1, 0.75f, 0);
                    Player1Chosen = false;
                    Player1Icon.SetActive(true);
                    Player1Text.SetActive(false);
                }
            }/*
        }
        else if (Input.GetButtonUp("Player_One_Paddle_Back"))
        {
            if (Player1Chosen == true)
            {
                StartGameButton.SetActive(false);
                Player1Chosen = false;
                Player1Icon.SetActive(true);
                Player1Text.SetActive(false);
            }*/
            else if(onReturn)
            {
                ReturnButton.GetComponent<MenuButtons>().PressButton();
            }
        }
        else if(!Player1Chosen&&!onReturn)
        {
            if (Input.GetButtonDown("Player_One_Paddle_Right")|| Input.GetAxis("Player_One_Joystick_Horizontal")>0)
            {
                if (Player1Pos + 1 < AmountOfChars)
                {
                    Player1Icon.transform.localPosition += portraitDistance;
                    Player1Text.transform.localPosition += portraitDistance;
                    Player1Pos++;
                }
            }
            else if (Input.GetButtonDown("Player_One_Paddle_Left")|| Input.GetAxis("Player_One_Joystick_Horizontal")<0)
            {
                if (Player1Pos > 0)
                {
                    Player1Icon.transform.localPosition -= portraitDistance;
                    Player1Text.transform.localPosition -= portraitDistance;
                    Player1Pos--;
                }
            }
        }

        if(Input.GetButtonUp("Player_One_Pause") /*|| Input.GetButtonUp("Player_Two_Pause")*/)
        {
            if(Player1Chosen&&Player2Chosen)
                StartGameButton.GetComponent<MenuButtons>().PressButton();
        }

        if (Input.GetButtonDown("Player_Two_Paddle_Back"))
        {
            if (!Player2Chosen)
            {
                if (!Player1Chosen || Player1Pos != Player2Pos)
                {
                    CharNames name = (CharNames)Player2Pos;
                    PlayerData.player2Character = name.ToString();

                    Player2Chosen = true;
                    Player2Icon.SetActive(false);
                    Player2Text.SetActive(true);

                    if (Player2Pos == 0)
                        audioJolly.PlayOneShot(onCharacterClip[Player2Pos]);
                    else
                        audioRoger.PlayOneShot(onCharacterClip[Player2Pos]);

                    if (Player1Chosen && Player2Chosen)
                    {
                        StartGameButton.SetActive(true);
                        ReturnButton.GetComponent<TextMesh>().color = new Color(0.85f, 0.65f, 0);
                    }
                }
            }
            else
            {
                ReturnButton.GetComponent<TextMesh>().color = new Color(1, 0.75f, 0);
                StartGameButton.SetActive(false);
                Player2Chosen = false;
                Player2Icon.SetActive(true);
                Player2Text.SetActive(false);
            }
        }/*
        else if (Input.GetButtonDown("Player_Two_Paddle_Back"))
        {
            StartGameButton.SetActive(false);
            Player2Chosen = false;
            Player2Icon.SetActive(true);
            Player2Text.SetActive(false);
        }*/
        else if (!Player2Chosen)
        {
            if (Input.GetButtonDown("Player_Two_Paddle_Right")|| Input.GetAxis("Player_Two_Joystick_Horizontal")>0)
            {
                if (Player2Pos + 1 < AmountOfChars)
                {
                    Player2Icon.transform.localPosition += portraitDistance;
                    Player2Text.transform.localPosition += portraitDistance;
                    Player2Pos++;
                }
            }
            else if (Input.GetButtonDown("Player_Two_Paddle_Left")|| Input.GetAxis("Player_Two_Joystick_Horizontal")<0)
            {
                if (Player2Pos > 0)
                {
                    Player2Icon.transform.localPosition -= portraitDistance;
                    Player2Text.transform.localPosition -= portraitDistance;
                    Player2Pos--;
                }
            }
        }
    }
}
