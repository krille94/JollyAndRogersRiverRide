using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PickUpTrigger : MonoBehaviour
{
    public delegate void OnPickUp(int value);
    public OnPickUp onPickUp;

    /*public void OnCollisionEnter(Collision collision)
    {
        if (collision.transform.tag == "Item")
        {
            Destroy(collision.transform.gameObject);
            onPickUp(1);
        }
    }*/
    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Item")
        {
            other.gameObject.SetActive(false);
            Destroy(other.gameObject);
            onPickUp(1);
        }
    }
}
