using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RiverSpeed : MonoBehaviour
{
    public int minimumSpeed;
    public Vector3 movementDirection;

    public Rigidbody rb;

    // Start is called before the first frame update
    void Start()
    {
        if(rb == null)
        rb = GetComponent<Rigidbody>();

        Vector3 movement = movementDirection;
        movement *= (minimumSpeed);

        //Use rb.velocity to set a specific speed
        rb.velocity = movement;
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 movement = movementDirection;
        movement *= (minimumSpeed);

        //Use rb.AddForce to gradually increase or decrease speed
        //   Giving it 0.1f leeway so that the boat won't start going back and forth between 24.97f and 25.002f
        if (rb.velocity.x < minimumSpeed - 0.1f)
            rb.AddForce(movement * (minimumSpeed * Time.deltaTime));
        else if (rb.velocity.x > minimumSpeed+0.1f)
            rb.AddForce(-movement * (minimumSpeed * Time.deltaTime));
        else
            rb.velocity = movement;
    }
}