using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoatCollisionKnockBack : MonoBehaviour
{
    public float force = 100;

    private Rigidbody body;
    private RiverController river;

    private void Start()
    {
        body = GetComponent<Rigidbody>();

        river = RiverController.instance;

        if (body == null || river == null)
            this.enabled = false;
    }

    private void OnCollisionStay(Collision collision)
    {
        //Vector3 target = river.riverAsset.GetNodeFromPosition(river.transform.position, body.transform.position).centerVector;

        Vector3 target = collision.GetContact(0).point;
        target = target - body.transform.position;
        target.y = 0;
        body.AddForce(-target.normalized * force);

    }
}
