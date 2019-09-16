using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoatFloatingController : MonoBehaviour
{
    public float bouance = 100;

    private void OnTriggerStay(Collider other)
    {
        other.attachedRigidbody.AddForce(Vector3.up * Time.fixedDeltaTime * bouance, ForceMode.Acceleration);

        //Debug.Log(other.name + " is colliding with water surface");
    }
}
