using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloatingController : MonoBehaviour
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
        distance = Mathf.Abs( other.transform.position.y - transform.position.y );

        other.attachedRigidbody.AddForce(Vector3.up * (bouance / other.attachedRigidbody.mass) * distance * Time.fixedDeltaTime);

        Debug.DrawRay(other.transform.position, Vector3.up * ((bouance / other.attachedRigidbody.mass) * distance) / 2500, Color.blue);
        //Debug.Log(other.name + " is colliding with water surface");
    }
}
