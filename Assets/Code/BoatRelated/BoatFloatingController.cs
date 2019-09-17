using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoatFloatingController : MonoBehaviour
{
    public float bouance = 100;

    private Collider collider;

    private void Start()
    {
        collider = GetComponent<Collider>();
    }

    private void OnTriggerStay(Collider other)
    {
        float distance = collider.bounds.SqrDistance(other.transform.position);
        other.attachedRigidbody.AddForce(Vector3.up * Time.fixedDeltaTime * bouance * distance);

        //Debug.Log(other.name + " is colliding with water surface");
    }
}
