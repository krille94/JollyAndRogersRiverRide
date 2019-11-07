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
    private bool startedGame=false;

    public Vector3 portraitDistance;

    new AudioSource audioP1;
    new AudioSource audioP2;
    [SerializeField] AudioClip[] onCharacterClip;
    [SerializeField] AudioClip changingCharacterClip;

    [SerializeField] float jollyPitch = 2.5f;
    [SerializeField] float rogerPitch = 0.75f;

    private void OnDisable()
    {
        startedGame = false;
        onReturn = false;
        StartGameButton.SetActive(false);
        Player1Chosen = false;
        Player1Icon.SetActive(true);
        Player1Text.SetActive(false);
        Player2Chosen = false;
        Player2Icon.SetActive(true);
        Player2Text.SetActive(false);

        while(Player1Pos>0)
        {
            Player1Icon.transform.localPosition -= portraitDistance;
            Player1Text.transform.localPosition -= portraitDistance;
            Player1Pos--;
        }
        while (Player2Pos > 0)
        {
            Player2Icon.transform.localPosition -= portraitDistance;
            Player2Text.transform.localPosition -= portraitDistance;
            Player2Pos--;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        startedGame = false;
        if (gameObject.GetComponent<AudioSource>())
            audioP1 = gameObject.GetComponent<AudioSource>();
        else
        {
            audioP1 = gameObject.AddComponent<AudioSource>();
            audioP2 = gameObject.AddComponent<AudioSource>();
        }
        AudioMixer mix = Resources.Load("AudioMixers/Sound Effects") as AudioMixer;
        audioP1.outputAudioMixerGroup = mix.FindMatchingGroups("Master")[0];
        audioP2.outputAudioMixerGroup = mix.FindMatchingGroups("Master")[0];

        audioP2.pitch = jollyPitch;
        audioP1.pitch = rogerPitch;


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
                Player1Pos += i;
            }
            if (PlayerData.player2Character == tempName.ToString())
            {
                Player2Icon.transform.position += portraitDistance * i;
                Player2Text.transform.position += portraitDistance * i;
                Player2Pos += i;
            }
        }
    }

    public void SetStartedGame(bool onoff)
    {
        startedGame = onoff;
    }

    // Update is called once per frame
    void Update()
    {
        if (startedGame)
            return;

        if (Input.GetButtonUp("Player_One_Pause") || Input.GetButtonUp("Player_Two_Pause"))
        {
            if (Player1Chosen && Player2Chosen)
            {
                StartGameButton.GetComponent<MenuButtons>().PressButton();
                return;
            }
        }

        if (ReturnButton.GetComponent<MouseHover>().GetSelected()&&!StartGameButton.activeInHierarchy)
        {
            ReturnButton.GetComponent<MouseHover>().SetSelected(false);
            ReturnButton.GetComponent<TextMesh>().color = Color.white;
        }

        if (Input.GetAxis("Player_One_Joystick_Vertical") > 0 || Input.GetButtonUp("Player_One_Menu_Down"))
        {
            if (!Player1Chosen)
            {
                onReturn = true;
                ReturnButton.GetComponent<TextMesh>().color = Color.white;
                Player1Icon.SetActive(false);
            }
        }
        else if (Input.GetAxis("Player_One_Joystick_Vertical") < 0 || Input.GetButtonUp("Player_One_Menu_Up"))
        {
            if (onReturn)
            {
                onReturn = false;
                ReturnButton.GetComponent<TextMesh>().color = new Color(1, 0.75f, 0);
                Player1Icon.SetActive(true);
            }
        }

        if (Input.GetButtonUp("Player_One_Paddle_Forward"))
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
                            audioP1.pitch = rogerPitch;
                        else
                            audioP1.pitch = jollyPitch;

                        audioP1.PlayOneShot(onCharacterClip[Player1Pos]);


                        if (Player1Chosen && Player2Chosen)
                        {
                            StartGameButton.SetActive(true);
                            ReturnButton.GetComponent<TextMesh>().color = new Color(0.85f, 0.65f, 0);
                        }
                    }
                }
            }
            else
            {
                ReturnButton.GetComponent<TextMesh>().color = new Color(1, 0.75f, 0);
                ReturnButton.GetComponent<MenuButtons>().PressButton();
            }
        }
        else if (Input.GetButtonUp("Player_One_Paddle_Back"))
        {
            if (Player1Chosen == true)
            {
                StartGameButton.SetActive(false);
                Player1Chosen = false;
                Player1Icon.SetActive(true);
                Player1Text.SetActive(false);
            }
            else
            {
                ReturnButton.GetComponent<TextMesh>().color = new Color(1, 0.75f, 0);
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
                    audioP1.pitch = 1;
                    audioP1.PlayOneShot(changingCharacterClip);
                }
            }
            else if (Input.GetButtonDown("Player_One_Paddle_Left")|| Input.GetAxis("Player_One_Joystick_Horizontal")<0)
            {
                if (Player1Pos > 0)
                {
                    Player1Icon.transform.localPosition -= portraitDistance;
                    Player1Text.transform.localPosition -= portraitDistance;
                    Player1Pos--;
                    audioP1.pitch = 1;
                    audioP1.PlayOneShot(changingCharacterClip);
                }
            }
        }


        if (Input.GetButtonDown("Player_Two_Paddle_Forward"))
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
                        audioP2.pitch = rogerPitch;
                    else
                        audioP2.pitch = jollyPitch;

                    audioP2.PlayOneShot(onCharacterClip[Player2Pos]);

                    if (Player1Chosen && Player2Chosen)
                    {
                        StartGameButton.SetActive(true);
                        ReturnButton.GetComponent<TextMesh>().color = new Color(0.85f, 0.65f, 0);
                    }
                }
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
            if (Input.GetButtonDown("Player_Two_Paddle_Right")|| Input.GetAxis("Player_Two_Joystick_Horizontal")>0)
            {
                if (Player2Pos + 1 < AmountOfChars)
                {
                    Player2Icon.transform.localPosition += portraitDistance;
                    Player2Text.transform.localPosition += portraitDistance;
                    Player2Pos++;
                    audioP2.pitch = 1;
                    audioP2.PlayOneShot(changingCharacterClip);
                }
            }
            else if (Input.GetButtonDown("Player_Two_Paddle_Left")|| Input.GetAxis("Player_Two_Joystick_Horizontal")<0)
            {
                if (Player2Pos > 0)
                {
                    Player2Icon.transform.localPosition -= portraitDistance;
                    Player2Text.transform.localPosition -= portraitDistance;
                    Player2Pos--;
                    audioP2.pitch = 1;
                    audioP2.PlayOneShot(changingCharacterClip);
                }
            }
        }
    }
}
