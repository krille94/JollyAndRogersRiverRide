using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Paddling : MonoBehaviour
{
    public bool is_player1;
    public bool is_player2;
    bool Paddled = false;
    bool rotateLeft=false;
    bool rotateRight=false;
    float rotation;
    public float rotateDegrees = 25;

    float timer;
    public float MovementTime = 2;
    public float speed;
    private KeyCode oarKey, oarLeft, oarRight;

    private void Start()
    {
        if (is_player1)
        {
            oarLeft = KeyCode.A;
            oarRight = KeyCode.D;
        }
        else
        {
            oarLeft = KeyCode.LeftArrow;
            oarRight = KeyCode.RightArrow;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Paddled == false)
        {
            if (Input.GetKey(oarLeft))
            {
                rotateLeft = true;
                rotation = rotateDegrees / MovementTime;
                Paddled = true;
            }
            else if (Input.GetKey(oarRight))
            {
                rotateRight = true;
                rotation = rotateDegrees / MovementTime;
                rotation = -rotation;
                Paddled = true;
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
            /*
            // Slow rotation as the movement gets closer to finishing - tweaking/feel thing for later
            else if(timer>=MovementTime*0.75f)
            {
                rotation -= Time.deltaTime;
            }*/

            float moveForward = 1;
            Vector3 movement = new Vector3(0.0f, 0.0f, moveForward);

            if (Paddled)
                transform.Translate(movement * speed * Time.deltaTime);

            transform.Rotate(0, rotation*Time.deltaTime, 0);
        }
    }
}
