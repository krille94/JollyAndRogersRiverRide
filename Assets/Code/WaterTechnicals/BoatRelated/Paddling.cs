﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Oar
{
    public Transform modelRoot;

    public bool onLeftSide;
    public bool onRightSide;

    public Transform leftSideImpactPoint;
    public Transform rightSideImpactPoint;

    public GameObject modelLeft;
    public GameObject modelRight;

    public bool isPaddling = false;
    public float paddlingTime = 0.5f;

    public void SetRightSide(bool inWater)
    {
        if (UserSettings.GetControlScheme() == 1)
        {
            if (!modelRight.activeInHierarchy)
                modelRight.SetActive(true);

            if (inWater)
            {
                modelRight.transform.localEulerAngles = new Vector3(0, 0, 35);
            }
            else
            {
                modelRight.transform.localEulerAngles = new Vector3(0, 0, 70);
            }
            onRightSide = inWater;
        }
        else if (UserSettings.GetControlScheme() == 2)
        {
            onRightSide = inWater;
            modelRight.SetActive(inWater);
            if (inWater)
            {
                onLeftSide = false;
                modelLeft.SetActive(false);
            }
        }
    }

    public void SetLeftSide(bool inWater)
    {
        if (UserSettings.GetControlScheme() == 1)
        {
            if (!modelLeft.activeInHierarchy)
                modelLeft.SetActive(true);

            if (inWater)
            {
                modelLeft.transform.localEulerAngles = new Vector3(0, 180, 35);
            }
            else
            {
                modelLeft.transform.localEulerAngles = new Vector3(0, 180, 70);
            }
            onLeftSide = inWater;
        }
        else if (UserSettings.GetControlScheme() == 2)
        {
            onLeftSide = inWater;
            modelLeft.SetActive(inWater);
            if (inWater)
            {
                onRightSide = false;
                modelRight.SetActive(false);
            }
        }
    }

    /// <summary>
    /// Returns a point where the paddle hits the water
    /// </summary>
    /// <returns></returns>
    public Vector3 Paddle(string whichway)
    {
        if (isPaddling)
            return Vector3.zero;
        isPaddling = true;

        if (onLeftSide && onRightSide)
        {
            //if (!modelLeft.GetComponent<Animation>().isPlaying)
                modelLeft.GetComponent<Animation>().Play("OarLeft"+whichway+"Animation");
            //if (!modelRight.GetComponent<Animation>().isPlaying)
                modelRight.GetComponent<Animation>().Play("OarRight" + whichway + "Animation");
            return (leftSideImpactPoint.position + rightSideImpactPoint.position)/2;
        }

        if (!onLeftSide && !onRightSide)
        {
            //modelLeft.GetComponent<Animation>().Play();
            //modelRight.GetComponent<Animation>().Play();
        }

        if (onLeftSide)
        {
            //if (!modelLeft.GetComponent<Animation>().isPlaying)
                modelLeft.GetComponent<Animation>().Play("OarLeft" + whichway + "Animation");
            return leftSideImpactPoint.position;
        }
       
        if(onRightSide)
        {
            //if (!modelRight.GetComponent<Animation>().isPlaying)
                modelRight.GetComponent<Animation>().Play("OarRight" + whichway + "Animation");
            return rightSideImpactPoint.position;
        }

        return Vector3.zero;
    }
}

public class Paddling : MonoBehaviour
{
    public enum PlayerIndexTypes { Jolly, Roger }
    public PlayerIndexTypes playerIndex = 0;
    string player;

    float turnForwardForce;
    float forwardForce;
    float backwardForce=500;
    float turnBackwardForce;
    [SerializeField] public float paddleTime;
    float maximumSpeed=30;

    //[SerializeField] public KeyCode keyLeft, keyRight;

    [SerializeField] private new Rigidbody rigidbody = null; 

    [SerializeField] private Oar oar = null;

    private Vector3 impactPoint;

    private bool CanControl = true;
    private bool autoPaddle = false;
    private bool reverseControls = true;

    [Header("Charge Boost")]
    public float boostTurnMultiplier = 2;
    public float chargeTimerMax = 1;
    float chargeTimer = 0;
    public float boostTimerMax = 0.3f;
    float boostTimer = 0;
    float chargeForce = 0;
    public bool fullyChargedBoost = false;
    bool chargingBoost = false;
    public float sidePushForce = 5000;

    public void SetCanControl(bool truefalse) { CanControl = truefalse; }

    private void Start()
    {
        SetSpeedValues(0);
        oar.SetLeftSide(false);
        oar.SetRightSide(false);

        if (PlayerData.player1Character == playerIndex.ToString())
            player = "One";
        else if (PlayerData.player2Character == playerIndex.ToString())
            player = "Two";
        else if (playerIndex.ToString() == "Jolly") player = "One";
        else player = "Two";
        //if (playerIndex.ToString()=="One") oar.SetLeftSide(true);
        //if (playerIndex.ToString() == "Two") oar.SetRightSide(true);

        UserSettings.ReadSettings();
        //if (UserSettings.GetAutoPaddle()) autoPaddle = true;
        if (UserSettings.GetReversedControls()) reverseControls = true;
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
            maximumSpeed = newValue.maximumSpeed;
        }
    }

    void Update()
    {
        impactPoint = Vector3.zero;

        if (fullyChargedBoost && !chargingBoost)
        {
            //if (rigidbody.velocity.magnitude < maximumSpeed)
            { rigidbody.AddForce(rigidbody.transform.forward * (chargeForce * (Time.deltaTime*boostTimerMax))); }

            boostTimer += Time.deltaTime;
            if (boostTimer >= boostTimerMax)
            {
                fullyChargedBoost = false;
                chargeTimer = 0;
                boostTimer = 0;
            }
        }

        if (oar.isPaddling)
        {
            oar.paddlingTime += Time.deltaTime;
            if (oar.paddlingTime >= paddleTime)
            {
                oar.paddlingTime = 0;
                oar.isPaddling = false;
            }

            return;
        }


        bool rightKey;
        bool leftKey;
        bool holdingForwardKey;
        bool holdingBackKey;
        bool releasingForwardKey;
        bool releasingBackKey;

        if (reverseControls==true)
        {
            float joyStickDir = Input.GetAxis("Player_" + player + "_Joystick_Movement");
            if (Input.GetButton("Player_" + player + "_Paddle_Right") || joyStickDir > 0)
                leftKey = true;
            else
                leftKey = false;

            if (Input.GetButton("Player_" + player + "_Paddle_Left") || joyStickDir < 0)
                rightKey = true;
            else
                rightKey = false;
        }
        else
        {
            float joyStickDir = Input.GetAxis("Player_" + player + "_Joystick_Movement");
            if (Input.GetButton("Player_" + player + "_Paddle_Right") || joyStickDir < 0)
                rightKey = true;
            else
                rightKey = false;

            if (Input.GetButton("Player_" + player + "_Paddle_Left") || joyStickDir > 0)
                leftKey = true;
            else
                leftKey = false;
        }

        holdingForwardKey = Input.GetButton("Player_" + player + "_Paddle_Forward");
        releasingForwardKey = Input.GetButtonUp("Player_" + player + "_Paddle_Forward");

        if(!holdingForwardKey&&!releasingForwardKey)
        {
            holdingBackKey = Input.GetButton("Player_" + player + "_Paddle_Back");
            releasingBackKey = Input.GetButtonUp("Player_" + player + "_Paddle_Back");
        }
        else
        {
            holdingBackKey = false;
            releasingBackKey = false;
        }

        /*if(autoPaddle==true)
        {
            forwardKey = Input.GetButton("Player_" + player + "_Paddle_Forward");
            backKey = Input.GetButton("Player_" + player + "_Paddle_Back");
        }
        else
        {
            forwardKey = Input.GetButtonUp("Player_" + player + "_Paddle_Forward");
            backKey = Input.GetButtonUp("Player_" + player + "_Paddle_Back");

            //if (forwardKey == true)
            //    Debug.Log("Forward key has been hit");
        }*/

        if (CanControl)
        {
            if (oar.onLeftSide || oar.onRightSide)
            {
                if(holdingForwardKey||holdingBackKey)
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
                    chargingBoost = false;

                    if (fullyChargedBoost)
                    {
                        impactPoint = oar.Paddle("Forward");
                        chargeForce = forwardForce;
                        if(oar.onLeftSide) rigidbody.AddRelativeForce (Vector3.left*sidePushForce);
                        else if (oar.onRightSide) rigidbody.AddRelativeForce(Vector3.right* sidePushForce);

                        rigidbody.AddForceAtPosition(rigidbody.transform.forward * (turnForwardForce*boostTurnMultiplier), impactPoint);
                    }
                    else
                    { 
                        chargeTimer = 0;
                        boostTimer = 0;

                        impactPoint = oar.Paddle("Forward");

                        if (rigidbody.velocity.magnitude < maximumSpeed)
                        {   rigidbody.AddForce(rigidbody.transform.forward * forwardForce); }
                        rigidbody.AddForceAtPosition(rigidbody.transform.forward * turnForwardForce, impactPoint);
                    }
                }
                else if (releasingBackKey)
                {
                    chargingBoost = false;

                    if (fullyChargedBoost)
                    {
                        impactPoint = oar.Paddle("Backward");
                        chargeForce = backwardForce;
                        rigidbody.AddForceAtPosition(-rigidbody.transform.forward * (turnBackwardForce*boostTurnMultiplier), impactPoint);
                    }
                    else
                    {
                        chargeTimer = 0;
                        boostTimer = 0;

                        impactPoint = oar.Paddle("Backward");

                        rigidbody.AddForce(-rigidbody.transform.forward * backwardForce);
                        rigidbody.AddForceAtPosition(-rigidbody.transform.forward * turnBackwardForce, impactPoint);
                    }
                }
            }

            if (leftKey)
            {
                if (!oar.onLeftSide)
                    oar.SetLeftSide(true);
            }
            else
            { 
                if (oar.onLeftSide)
                    oar.SetLeftSide(false);
            }

            if (rightKey)  
            {
                if (!oar.onRightSide)
                    oar.SetRightSide(true);
            }
            else
            {
                if (oar.onRightSide)
                    oar.SetRightSide(false);
            }
        }
    }

    private void FixedUpdate()
    {
        if (impactPoint != Vector3.zero)
        {
            //rigidbody.AddForceAtPosition(rigidbody.transform.forward * paddleForce, impactPoint);
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawSphere(oar.leftSideImpactPoint.position, 1);
        
        Gizmos.color = Color.red;
        Gizmos.DrawSphere(oar.rightSideImpactPoint.position, 1);

        Gizmos.color = Color.black;
        Gizmos.DrawRay(rigidbody.transform.position, rigidbody.transform.forward * 10);
    }
}
