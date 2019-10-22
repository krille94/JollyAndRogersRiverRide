using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WhirlpoolScript : MonoBehaviour
{
    public bool active = true;
    public int spinSpeed = 42000;

    public float dizzyDuration = 3;
    private float dizzyTimer = 0;
    private bool dizzy = false;

    void OnTriggerEnter(Collider other)
    {
        if (active)
        {
            if (other.gameObject.tag == "Player")
            {
                active = false;
                dizzy = true;
                Rigidbody rigidbody = other.gameObject.GetComponent<Rigidbody>();
                rigidbody.AddTorque(rigidbody.transform.up * spinSpeed);

                GameObject.Find("PlayerOneSpot").GetComponent<Paddling>().SetCanControl(false);
                GameObject.Find("PlayerTwoSpot").GetComponent<Paddling>().SetCanControl(false);
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(dizzy)
        {
            dizzyTimer += Time.deltaTime;

            if (dizzyTimer>=dizzyDuration)
            {
                dizzy = false;
                active = true;
                GameObject.Find("PlayerOneSpot").GetComponent<Paddling>().SetCanControl(true);
                GameObject.Find("PlayerTwoSpot").GetComponent<Paddling>().SetCanControl(true);

            }
        }
    }
}
