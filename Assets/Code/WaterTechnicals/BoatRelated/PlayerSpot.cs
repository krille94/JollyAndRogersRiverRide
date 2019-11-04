using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSpot : MonoBehaviour
{
    public enum PlayerIndexTypes { Jolly, Roger }
    [Header("Player")]
    public PlayerIndexTypes playerIndex = 0;
    string player;
    private bool isPlaying = false;

    [Header("Force Values")]
    float turnForwardForce;
    float forwardForce;
    float backwardForce = 500;
    float turnBackwardForce;
    float paddleTime;

    [Header("Graphics")]
    [SerializeField] Animator animator;
    [SerializeField] bool tiltedBoat = false;
    float boatTiltAngle = 0.4f;

    [Header("System")]
    [SerializeField] private new Rigidbody rigidbody = null;

    [Header("Settings")]
    private bool CanControl = true;
    //private bool reverseControls = true;
    private bool autoPaddle = false;
    private float controlScheme;

    [Header("Charge Boost")]
    [SerializeField] float boostTurnMultiplier = 2;
    [SerializeField] float chargeTimerMax = 1;
    [SerializeField] float boostSidePushForce = 5000;
    [SerializeField] float boostTimerMax = 0.3f;
    [SerializeField] bool fullyChargedBoost = false;
    float chargeTimer = 0;
    float boostTimer = 0;
    float chargeForce = 0;
    bool chargingBoost = false;
    float sidePushForce = 0;

    [Header("Sounds")]
    [SerializeField] AudioSource audioSource;
    [SerializeField] AudioClip[] onPaddleSound;

    [Header("Private Values")]
    private bool isPaddling;
    private float paddlingTime;

    public void SetCanControl(bool truefalse)
    {
        CanControl = truefalse;

        if (CanControl == false)
        {

        }
    }

    public void SetSpeedValues(int damage)
    {
        if (SpeedValueManager.GetSpeedValues().Count > damage)
        {
            SpeedValue newValue = SpeedValueManager.GetSpeedValues()[damage];

            turnForwardForce = newValue.turnForwardForce;
            turnBackwardForce = newValue.turnBackwardForce;
            forwardForce = newValue.forwardForce;
            backwardForce = newValue.backwardForce;
            //maximumSpeed = newValue.maximumSpeed;
            sidePushForce = newValue.sidePushForce;
            paddleTime = newValue.paddleTime;
        }
    }

    private void Start()
    {
        SetSpeedValues(0);

        if (PlayerData.player1Character == playerIndex.ToString())
            player = "One";
        else if (PlayerData.player2Character == playerIndex.ToString())
            player = "Two";
        else if (playerIndex.ToString() == "Jolly") player = "One";
        else player = "Two";

        UserSettings.ReadSettings();
        controlScheme = UserSettings.GetControlScheme();
        if (UserSettings.GetAutoPaddle()) autoPaddle = true;
        //if (UserSettings.GetReversedControls()) reverseControls = true;
    }

    void Update()
    {
        if (!isPlaying)
        {
            if (GameController.isPlaying)
            {
                isPlaying = true;

                if (PlayerData.player1Character == playerIndex.ToString())
                    player = "One";
                else if (PlayerData.player2Character == playerIndex.ToString())
                    player = "Two";
                else if (playerIndex.ToString() == "Jolly") player = "One";
                else player = "Two";
            }
            else
                return;
        }

        if (GameController.instance.GetClearGame())
            return;

        #region Input
        bool rightKey;
        bool leftKey;
        bool holdingForwardKey;
        bool holdingBackKey;
        bool releasingForwardKey;
        bool releasingBackKey;

        {
            float joyStickDir = Input.GetAxis("Player_" + player + "_Joystick_Horizontal");
            if (Input.GetButton("Player_" + player + "_Paddle_Right") || joyStickDir > 0)
                rightKey = true;
            else
                rightKey = false;

            if (Input.GetButton("Player_" + player + "_Paddle_Left") || joyStickDir < 0)
                leftKey = true;
            else
                leftKey = false;
        }

        if (autoPaddle == true)
        {
            holdingForwardKey = false;
            holdingBackKey = false;
            if (Input.GetButton("Player_" + player + "_Paddle_Forward") || Input.GetButton("Player_" + player + "_Menu_Up"))
                releasingForwardKey = true;
            else
                releasingForwardKey = false;

            if (Input.GetButton("Player_" + player + "_Paddle_Back") || Input.GetButton("Player_" + player + "_Menu_Down"))
                releasingBackKey = true;
            else
                releasingBackKey = false;
        }
        else
        {
            holdingForwardKey = Input.GetButton("Player_" + player + "_Paddle_Forward");
            if (Input.GetButtonUp("Player_" + player + "_Paddle_Forward") || Input.GetButtonUp("Player_" + player + "_Menu_Up"))
                releasingForwardKey = true;
            else
                releasingForwardKey = false;

            if (!holdingForwardKey && !releasingForwardKey)
            {
                holdingBackKey = Input.GetButton("Player_" + player + "_Paddle_Back");
                if (Input.GetButtonUp("Player_" + player + "_Paddle_Back") || Input.GetButtonUp("Player_" + player + "_Menu_Down"))
                    releasingBackKey = true;
                else
                    releasingBackKey = false;
            }
            else
            {
                holdingBackKey = false;
                releasingBackKey = false;
            }
        }
        #endregion

        if (CanControl)
        {
            //if (controlScheme == 1)
            {
                if (leftKey)
                {
                    if (!tiltedBoat)
                    {
                        tiltedBoat = true;
                        PlayerData.boatTiltOffset = -boatTiltAngle;
                    }
                    else if (PlayerData.boatTiltOffset > 0)
                    {
                        PlayerData.boatTiltOffset = -boatTiltAngle;
                    }

                    rigidbody.AddTorque(rigidbody.transform.up * -turnForwardForce);

                    if (animator != null)
                        animator.SetFloat("LeaningDirection", 0.0f);
                }
                else if (rightKey)
                {
                    if (!tiltedBoat)
                    {
                        tiltedBoat = true;
                        PlayerData.boatTiltOffset = boatTiltAngle;
                    }
                    else if (PlayerData.boatTiltOffset < 0)
                    {
                        PlayerData.boatTiltOffset = boatTiltAngle;
                    }

                    rigidbody.AddTorque(rigidbody.transform.up * turnForwardForce);

                    if (animator != null)
                        animator.SetFloat("LeaningDirection", 1.0f);
                }
                else
                {
                    if (tiltedBoat)
                    {
                        tiltedBoat = false;
                        PlayerData.boatTilted = false;
                        PlayerData.boatTiltOffset = 0;
                    }

                    if (animator != null)
                        animator.SetFloat("LeaningDirection", 0.5f);
                }

                if (isPaddling)
                {
                    paddlingTime += Time.deltaTime;
                    if (paddlingTime >= paddleTime)
                    {
                        paddlingTime = 0;
                        isPaddling = false;
                    }
                }

                if (!isPaddling)
                {
                    if (holdingForwardKey || holdingBackKey)
                    {
                        chargingBoost = true;
                        if (chargeTimer < chargeTimerMax)
                        {
                            chargeTimer += Time.deltaTime;

                            if (chargeTimer >= chargeTimerMax)
                            {
                                chargeTimer = chargeTimerMax;
                                fullyChargedBoost = true;
                            }
                        }
                    }

                    if (releasingForwardKey)
                    {
                        audioSource.PlayOneShot(onPaddleSound[Random.Range(0,onPaddleSound.Length-1)]);
                        
                        isPaddling = true;

                        chargingBoost = false;
                        fullyChargedBoost = false;

                        if (fullyChargedBoost)
                        {
                            chargeForce = forwardForce;
                            rigidbody.AddForce(rigidbody.transform.forward * (turnForwardForce * boostTurnMultiplier));
                        }
                        else
                        {
                            chargeTimer = 0;
                            boostTimer = 0;
                            { rigidbody.AddForce(rigidbody.transform.forward * forwardForce); }
                        }
                    }
                    else if (releasingBackKey)
                    {
                        audioSource.PlayOneShot(onPaddleSound[Random.Range(0, onPaddleSound.Length - 1)]);

                        isPaddling = true;

                        chargingBoost = false;
                        fullyChargedBoost = false;

                        if (fullyChargedBoost)
                        {
                            chargeForce = backwardForce;
                            rigidbody.AddForce(-rigidbody.transform.forward * (turnBackwardForce * boostTurnMultiplier));
                        }
                        else
                        {
                            chargeTimer = 0;
                            boostTimer = 0;
                            rigidbody.AddForce(-rigidbody.transform.forward * backwardForce);
                        }
                    }
                }
            }
        }

        animator.SetBool("isPaddling", isPaddling);
    }
}
