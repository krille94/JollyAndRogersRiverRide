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

    public void SetRightSide()
    {
        onLeftSide = false;
        onRightSide = true;
        modelRight.SetActive(true);
        modelLeft.SetActive(false);
    }

    public void SetLeftSide()
    {
        onLeftSide = true;
        onRightSide = false;
        modelLeft.SetActive(true);
        modelRight.SetActive(false);
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
    public enum PlayerIndexTypes { One, Two }
    public PlayerIndexTypes playerIndex = 0;

    [SerializeField] public float paddleForce;
    //[SerializeField] public KeyCode keyLeft, keyRight;

    [SerializeField] private new Rigidbody rigidbody = null; 

    [SerializeField] private Oar oar = null;

    private Vector3 impactPoint;
    
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

        bool rightKey = Input.GetButton("Player_"+((PlayerIndexTypes)playerIndex).ToString()+"_Paddle_Right");
        bool leftKey = Input.GetButton("Player_"+ ((PlayerIndexTypes)playerIndex).ToString()+ "_Paddle_Left");

        if (leftKey)
        {
            if (!oar.onLeftSide)
                oar.SetLeftSide();
            else
            {
                impactPoint = oar.Paddle();
                rigidbody.AddForceAtPosition(rigidbody.transform.forward * paddleForce, impactPoint);
            }
        }

        if (rightKey)
        {
            if (!oar.onRightSide)
                oar.SetRightSide();
            else
            {
                impactPoint = oar.Paddle();
                rigidbody.AddForceAtPosition(rigidbody.transform.forward * paddleForce, impactPoint);
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
