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
        float distance = 1;// collider.bounds.SqrDistance(other.transform.position);
        other.attachedRigidbody.AddForce(Vector3.up * (bouance / other.attachedRigidbody.mass) * distance * Time.fixedDeltaTime);

        //Debug.Log(other.name + " is colliding with water surface");
    }
}
