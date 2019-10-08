using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoostData : MonoBehaviour
{
    public int strength;

    private void OnTriggerStay(Collider other)
    {
        if (other.tag == "Player")
            other.GetComponent<Rigidbody>().AddForce(transform.forward * strength);
    }
}
