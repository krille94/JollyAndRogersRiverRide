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

    private Quaternion targetRotation;

    private RiverNode targetNode;

    [SerializeField] private bool newTarget = true;

    Skybox skybox;
    private void Start()
    {
        skybox = GetComponent<Skybox>();
    }

    void Update()
    {
        if(skybox != null)
        {
            skybox.material.SetFloat("_Rotation", Time.time);
        }

        //Gets target position
        RiverNode node = riverController.riverAsset.GetNodeFromPosition(boat.position);
        if(node != targetNode)
        {
            targetNode = node;

            targetPosition = targetNode.centerVector + offsetPosition;
            Quaternion temp = transform.rotation;
            transform.LookAt(targetPosition);
            targetRotation = Quaternion.Euler(45 + offsetAngle, transform.rotation.eulerAngles.y, transform.rotation.eulerAngles.z);
            transform.rotation = temp;

            newTarget = true;
        }

        if(newTarget)
        {
            //changes location
            transform.position = Vector3.Lerp(transform.position, targetPosition, Time.deltaTime * translationSpeed);

            //changes rotation
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * rotationSpeed);

            //post changes rotation angle
            //transform.rotation = Quaternion.Euler(45 + offsetAngle, transform.rotation.eulerAngles.y, transform.rotation.eulerAngles.z);

            if (Vector3.Distance(transform.position, new Vector3(targetPosition.x, transform.position.y, targetPosition.z)) < Mathf.Abs(offsetPosition.z))
                newTarget = false;
        }
    }
}
