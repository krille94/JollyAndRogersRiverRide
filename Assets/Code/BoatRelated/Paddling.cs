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
        if(onLeftSide)
        {
            return leftSideImpactPoint.position;
        }
       
        if(onRightSide)
        {
            return rightSideImpactPoint.position;
        }

        return Vector3.zero;
    }
}

public class Paddling : MonoBehaviour
{
    bool Paddled = false;
    bool rotateLeft=false;
    bool rotateRight=false;
    float rotation;
    public float rotateDegrees = 25;

    float timer;
    public float MovementTime = 2;
    public float speed;
    [SerializeField] KeyCode keyLeft, keyRight;

    [SerializeField] Rigidbody rigidbody;
    [SerializeField] Transform boathParentTransform;

    [SerializeField] Transform oarTransformLeft;
    [SerializeField] Transform oarTransformImpactPointLeft;

    [SerializeField] Transform oarTransformRight;
    [SerializeField] Transform oarTransformImpactPointRight;

    public Oar oar;
    
    void Update()
    {

        Vector3 impactPoint = Vector3.zero;

        if (Input.GetKey(keyLeft))
        {
            if (!oar.onLeftSide)
                oar.SetLeftSide();
            else
            {
                impactPoint = oar.Paddle();
            }
        }

        if (Input.GetKey(keyRight))
        {
            if (!oar.onRightSide)
                oar.SetRightSide();
            else
            {
                impactPoint = oar.Paddle();
            }
        }

        if(impactPoint != Vector3.zero)
        {
            rigidbody.AddForceAtPosition(rigidbody.transform.forward * speed, impactPoint);
        }

        /*
        if (Paddled == false)
        {
            if (Input.GetKey(keyLeft))
            {
                rotateLeft = true;
                rotation = rotateDegrees / MovementTime;
                Paddled = true;

                oarTransformLeft.Rotate(Vector3.right * 90);
            }
            else if (Input.GetKey(keyRight))
            {
                rotateRight = true;
                rotation = rotateDegrees / MovementTime;
                rotation = -rotation;
                Paddled = true;

                oarTransformRight.Rotate(Vector3.left * 90);
            }
        }

        if (Paddled == true)
        {
            timer += Time.deltaTime;
            
            if(timer>=MovementTime)
            {
                rotateLeft = false;
                rotateRight = false;
                Paddled = false;
                timer = 0;
            }
            
//            // Slow rotation as the movement gets closer to finishing - tweaking/feel thing for later
//            else if(timer>=MovementTime*0.75f)
//            {
//                rotation -= Time.deltaTime;
//            }

            float moveForward = 1;
            Vector3 movement = new Vector3(0.0f, 0.0f, moveForward);
            
            if (Paddled)
                boathParentTransform.Translate(movement * speed * Time.deltaTime);

            boathParentTransform.Rotate(0, rotation*Time.deltaTime, 0);
        }
        */
    }
}
