using System.Collections;
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
    public float paddlingTime = 0;
    
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
    public Vector3 Paddle()
    {
        if (isPaddling)
            return Vector3.zero;
        isPaddling = true;

        if (onLeftSide && onRightSide)
        {
            if (!modelLeft.GetComponent<Animation>().isPlaying)
                modelLeft.GetComponent<Animation>().Play();
            if (!modelRight.GetComponent<Animation>().isPlaying)
                modelRight.GetComponent<Animation>().Play();
            return (leftSideImpactPoint.position + rightSideImpactPoint.position)/2;
        }

        if (onLeftSide)
        {
            if (!modelLeft.GetComponent<Animation>().isPlaying)
                modelLeft.GetComponent<Animation>().Play();
            return leftSideImpactPoint.position;
        }
       
        if(onRightSide)
        {
            if (!modelRight.GetComponent<Animation>().isPlaying)
                modelRight.GetComponent<Animation>().Play();
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

    [SerializeField] public float paddleForce;
    [SerializeField] public float forwardForce;
    //[SerializeField] public KeyCode keyLeft, keyRight;

    [SerializeField] private new Rigidbody rigidbody = null; 

    [SerializeField] private Oar oar = null;

    private Vector3 impactPoint;

    private bool CanControl = true;
    private bool autoPaddle = false;

    public void SetCanControl(bool truefalse) { CanControl = truefalse; }

    private void Start()
    {
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

        if (UserSettings.GetAutoPaddle()) autoPaddle = true;
    }

    void Update()
    {
        impactPoint = Vector3.zero;

        if (oar.isPaddling)
        {
            oar.paddlingTime += Time.deltaTime;
            if (oar.paddlingTime >= 1)
            {
                oar.paddlingTime = 0;
                oar.isPaddling = false;
            }

            return;
        }

        bool rightKey = Input.GetButton("Player_"+player+"_Paddle_Right");
        bool leftKey = Input.GetButton("Player_"+ player+ "_Paddle_Left");
        bool forwardKey;
        bool backKey;

        if(autoPaddle==true)
        {
            forwardKey = Input.GetButton("Player_" + player + "_Paddle_Forward");
            backKey = Input.GetButton("Player_" + player + "_Paddle_Back");
        }
        else
        {
            forwardKey = Input.GetButtonUp("Player_" + player + "_Paddle_Forward");
            backKey = Input.GetButtonUp("Player_" + player + "_Paddle_Back");
        }

        if (CanControl)
        {
            if (forwardKey)
            {
                impactPoint = oar.Paddle();
                rigidbody.AddForce(rigidbody.transform.forward * forwardForce);
                rigidbody.AddForceAtPosition(rigidbody.transform.forward * paddleForce, impactPoint);
            }
            else if (backKey)
            {
                impactPoint = oar.Paddle();
                rigidbody.AddForceAtPosition(-rigidbody.transform.forward * paddleForce, impactPoint);
            }

            if (leftKey)
            {
                if (!oar.onLeftSide)
                    oar.SetLeftSide(true);
                /*else
                {
                    impactPoint = oar.Paddle();
                    rigidbody.AddForce(rigidbody.transform.forward * forwardForce);
                    rigidbody.AddForceAtPosition(rigidbody.transform.forward * paddleForce, impactPoint);
                }*/
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
                /*else
                {
                    impactPoint = oar.Paddle();
                    rigidbody.AddForce(rigidbody.transform.forward * forwardForce);
                    rigidbody.AddForceAtPosition(rigidbody.transform.forward * paddleForce, impactPoint);
                }*/
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
