using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public RiverController riverController;
    public FloatingObject boat;

    public Vector3 offsetPosition;
    public float offsetAngle;

    public float translationSpeed = 0.25f;
    public float rotationSpeed = 0.5f;

    private Vector3 targetPosition;

    private Vector3 targetRotationVector;
    private Quaternion targetRotation;

    /// <summary>
    /// Consider this as the cameras current node, stuff happens fast.
    /// </summary>
    public RiverNode targetNode;

    [SerializeField] private bool newTarget = true;

    private Vector3 boatOffset;

    Skybox skybox;

    [Header("Shake Cam")]
    public float shakeDuration=1.5f;
    float shakeTimer = 0;
    float oneShake = 0;
    public float timePerShake = 0.2f;
    bool shaking = false;
    Vector3 shakeDir = Vector3.zero;
    public float shakeIntensity=5;
    Vector3 shakeOffset = Vector3.zero;

    private void Start()
    {
        shakeTimer = shakeDuration;
        skybox = GetComponent<Skybox>();
        targetNode = riverController.riverAsset.GetNodeFromPosition(boat.transform.position);
        transform.rotation = boat.transform.rotation;
        targetRotationVector = transform.rotation.eulerAngles;
    }

    public void StartShakeCam()
    {
        shakeTimer = 0;
        oneShake = 0;
        shaking = true;
        shakeDir.x = Random.Range(-1.0f, 1.0f);
        shakeDir.y = Random.Range(-1.0f, 1.0f);
    }

    void ShakeCam()
    {
        oneShake += Time.deltaTime;
        shakeTimer += Time.deltaTime;
        if (oneShake >= timePerShake)
        {
            oneShake = 0;
            shakeDir.x = Random.Range(-1.0f, 1.0f);
            shakeDir.y = Random.Range(-1.0f, 1.0f);
        }

        if (oneShake<= timePerShake/2)  shakeOffset = shakeDir * (oneShake * shakeIntensity);
        else                            shakeOffset = shakeDir * ((timePerShake - oneShake) * shakeIntensity);


        if (shakeTimer >= shakeDuration)
        {
            shaking = false;
            shakeOffset = Vector3.zero;
        }
    }

    private float lastFrameProgress;

    void Update()
    {
        if(skybox != null)
        {
            skybox.material.SetFloat("_Rotation", Time.time);
        }

        if(shaking)
        { ShakeCam(); }

        if (boat.reverseProgress)
        {
            transform.position = boat.transform.position + boatOffset + shakeOffset;
            return;
        }
        
        RiverNode boatNode = boat.GetNodes().closest;

        if (boatNode != targetNode)
        {
            var heading = targetNode.centerVector - boatNode.centerVector;
            var distance = heading.magnitude;
            var direction = heading / distance; // This is now the normalized direction.
            targetNode = boatNode;

            targetRotation = Quaternion.LookRotation(-direction, Vector3.up);
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
        //targetRotation = boat.rotation.eulerAngles;
        transform.Rotate(new Vector3(-offsetAngle, 0, 0));
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * rotationSpeed);

        transform.Rotate(new Vector3(offsetAngle, 0, 0));
        //Gets target position
        targetRotationVector = transform.rotation.eulerAngles;
        Quaternion boatOffsetRot = Quaternion.Euler(targetRotationVector.x, targetRotationVector.y, targetRotationVector.z);
        boatOffset = boatOffsetRot * offsetPosition;

        targetPosition = boat.transform.position + boatOffset + shakeOffset;
        transform.position = targetPosition;


    }
}