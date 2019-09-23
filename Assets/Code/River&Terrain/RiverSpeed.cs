using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RiverSpeed : MonoBehaviour
{
    public int minimumSpeed;
    public Vector3 movementDirection;

    private Rigidbody playerRigidbody;
    
    public RiverObject river;

    // Start is called before the first frame update
    void Start()
    {
        if(playerRigidbody == null)
        playerRigidbody = GameObject.FindGameObjectWithTag("Player").GetComponent<Rigidbody>();

        Vector3 movement = movementDirection;
        movement *= (minimumSpeed);

        //Use rb.velocity to set a specific speed
        playerRigidbody.velocity = movement;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        UpdateRigidBody(playerRigidbody);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
            return;
        if (other.tag == "Item")
            return;

        UpdateRigidBody(other.GetComponent<Rigidbody>());
    }

    void UpdateRigidBody(Rigidbody body)
    {
        RiverNode node = river.GetNodeFromPosition(playerRigidbody.transform.position);

        Vector3 movement = node.flowDirection;
        movement *= (minimumSpeed);

        //Use rb.AddForce to gradually increase or decrease speed
        //   Giving it 0.1f leeway so that the boat won't start going back and forth between 24.97f and 25.002f
        if (playerRigidbody.velocity.x < minimumSpeed - 0.1f)
            playerRigidbody.AddForce(movement * (minimumSpeed * Time.deltaTime));
        else if (playerRigidbody.velocity.x > minimumSpeed + 0.1f)
            playerRigidbody.AddForce(-movement * (minimumSpeed * Time.deltaTime));
        else
            playerRigidbody.velocity = movement;
    }
}