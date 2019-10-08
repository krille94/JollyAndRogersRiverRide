using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public RiverController riverController;
    public Transform boat;

    public Vector3 offset;
    public float viewRot;

    public float speed = 0.25f;
    public float rotationSpeed = 200f;

    public Vector3 targetPosition;
    public Vector3 targetRotation;
    public Vector3 boatOffset;

    private void Start()
    {
        transform.rotation = boat.transform.rotation;
    }

    void Update()
    {
        RiverNode node = riverController.riverAsset.GetNodeFromPosition(boat.position);

        //Quaternion flowRot = Quaternion.Euler(targetRotation.x, targetRotation.y, targetRotation.z);
        //Vector3 myVector = Vector3.one;

        Quaternion rotation = Quaternion.Euler(targetRotation.x, targetRotation.y, targetRotation.z);
        boatOffset = rotation * offset;

        targetPosition = boat.position + boatOffset;
        targetRotation = node.flowDirection;

        transform.position = targetPosition;
        // Rotate by node direction around Y pos

        //Vector3 newRot = Vector3.RotateTowards(transform.forward, targetRotation, 10, 0.0f);
        //transform.rotation = Quaternion.Euler(newRot);
        transform.Rotate(new Vector3(-viewRot, 0, 0));
        //transform.rotation = Quaternion.LookRotation(node.flowDirection, Vector3.up);
        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(node.flowDirection), Time.deltaTime);
        transform.Rotate(new Vector3(viewRot,0,0));

        //transform.rotation = Quaternion.Euler(targetRotation.x, targetRotation.y, targetRotation.z);
        //if (transform.rotation != Quaternion.Euler(targetRotation))
        //transform.position = Vector3.Lerp(transform.position, targetPosition, Time.deltaTime * speed);

        /// OLD CODE
        /// 
        //targetPosition = node.centerVector + offset;

        //transform.position = Vector3.Lerp(transform.position, targetPosition, Time.deltaTime * speed);

        //transform.LookAt(boat);
    }
}
