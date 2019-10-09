using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public RiverController riverController;
    public Transform boat;

    public Vector3 offsetPosition;
    public float offsetAngle;

    public float translationSpeed = 0.25f;
    public float rotationSpeed = 0.5f;

    private Vector3 targetPosition;

    public Vector3 targetRotation;

    private RiverNode targetNode;

    [SerializeField] private bool newTarget = true;

    public Vector3 boatOffset;

    Skybox skybox;

    private void Start()
    {
        skybox = GetComponent<Skybox>();
        targetNode = riverController.riverAsset.GetNodeFromPosition(boat.position);
        transform.rotation = boat.transform.rotation;
        targetRotation = targetNode.centerVector;
    }

    void Update()
    {
        if(skybox != null)
        {
            skybox.material.SetFloat("_Rotation", Time.time);
        }

        RiverNode node = riverController.riverAsset.GetNodeFromPosition(boat.position);

        
        if (node != targetNode)
        {
            var heading = targetNode.centerVector - node.centerVector;
            var distance = heading.magnitude;
            var direction = heading / distance; // This is now the normalized direction.
            targetRotation = direction;
            targetNode = node;
            //transform.Rotate(new Vector3(0, targetRotation.x, 0));
            //targetPosition = targetNode.centerVector + offsetPosition;
            //Quaternion temp = transform.rotation;
            //transform.LookAt(targetPosition);
            //targetRotation = new Vector3(45 + offsetAngle, transform.rotation.eulerAngles.y, transform.rotation.eulerAngles.z);
            //transform.rotation = temp;
            newTarget = true;
        }

        //if(newTarget)
        {
            //changes location
            //transform.position = Vector3.Lerp(transform.position, targetPosition, Time.deltaTime * translationSpeed);

            //changes rotation
            //transform.Rotate(new Vector3(-offsetAngle, 0, 0));
            //Quaternion trueRotation = Quaternion.AngleAxis(targetRotation, Vector3.up);
            //transform.rotation = Quaternion.Slerp(transform.rotation, trueRotation, Time.deltaTime * rotationSpeed);
            //transform.Rotate(new Vector3(offsetAngle, 0, 0));

            //post changes rotation angle
            //transform.rotation = Quaternion.Euler(45 + offsetAngle, transform.rotation.eulerAngles.y, transform.rotation.eulerAngles.z);

            //if (Vector3.Distance(transform.position, new Vector3(targetPosition.x, transform.position.y, targetPosition.z)) < Mathf.Abs(offsetPosition.z))
            //    newTarget = false;
        }
        targetRotation = boat.rotation.eulerAngles;
        transform.rotation = Quaternion.Euler(targetRotation);
        transform.Rotate(new Vector3(offsetAngle, 0, 0));
        //Gets target position
        Quaternion boatOffsetRot = Quaternion.Euler(targetRotation.x, targetRotation.y, targetRotation.z);
        boatOffset = boatOffsetRot * offsetPosition;

        targetPosition = boat.position + boatOffset;
        transform.position = targetPosition;


    }
}