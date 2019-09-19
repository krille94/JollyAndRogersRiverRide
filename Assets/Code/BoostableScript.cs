using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoostableScript : MonoBehaviour
{
    private Rigidbody rigidBody;
    // Start is called before the first frame update
    void Start()
    {
        if (rigidBody == null)
            rigidBody = GetComponent<Rigidbody>();
    }

    void OnTriggerStay(Collider other)
    {
        if (other.gameObject.transform.tag == "Boost")
        {
            int streamStrength = other.gameObject.GetComponent<BoostData>().strength;
            Vector3 movementDirection = other.gameObject.transform.forward;

            rigidBody.velocity += (movementDirection * (streamStrength * Time.deltaTime));
        }
    }
}
