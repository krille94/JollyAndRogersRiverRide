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
        Vector3 target = river.riverAsset.GetNodeFromPosition(river.transform.position, body.transform.position).centerVector;
        target = target - body.transform.position;
        body.AddForce(target.normalized * force);
    }
}
