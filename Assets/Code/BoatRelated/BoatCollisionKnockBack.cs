using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoatCollisionKnockBack : MonoBehaviour
{
    public float force = 100;

    private Rigidbody body;
    private RiverController river;

    public bool isCollided = true;
    private GameObject otherCollider;

    private void Start()
    {
        body = GetComponent<Rigidbody>();

        river = RiverController.instance;

        if (body == null || river == null)
            this.enabled = false;
    }

    private void Update()
    {
        if (isCollided == false)
            return;

        bool stillClose = false;
        Collider[] hits = Physics.OverlapSphere(transform.position, 2.5f);
        for (int i = 0; i < hits.Length; i++)
        {
            if(hits[i].gameObject == otherCollider)
            {
                stillClose = true;
            }
        }
        if (stillClose == false)
            isCollided = false;
    }

    private void OnCollisionStay(Collision collision)
    {
        //Vector3 target = river.riverAsset.GetNodeFromPosition(river.transform.position, body.transform.position).centerVector;
        if (collision.gameObject.tag == "River")
            return;

        Vector3 target = collision.GetContact(0).point;
        target = target - body.transform.position;
        target.y = 0;
        body.AddForce(-target.normalized * force);

        otherCollider = collision.gameObject;
        isCollided = true;
    }
}
