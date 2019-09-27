using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class CameraController : MonoBehaviour
{
    public GameObject boat;

    public Vector3 offset;
    public float viewRot;

    public float speed = 0.25f;

    public Vector3 targetPosition;

    private void Start()
    {
        transform.position = boat.transform.position + offset;
    }

    void Update()
    {
        transform.position = Vector3.MoveTowards(transform.position, boat.transform.position + offset, Time.deltaTime * speed * Vector3.Distance(transform.position, boat.transform.position));
        //transform.position = Vector3.Lerp(targetPosition, transform.position, Time.deltaTime);

        transform.LookAt(boat.transform.position + new Vector3(0, viewRot, 0));
    }
}
