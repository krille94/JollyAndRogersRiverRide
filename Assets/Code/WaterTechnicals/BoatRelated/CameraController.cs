using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public RiverController riverController;
    public FloatingObject boat;

    public Vector3 offsetPosition;
    public float offsetAngle;
    public float slopeAngle=0;

    public float translationSpeed = 0.25f;
    public float rotationSpeed = 0.4f;
    public float blockRotationSpeed = 2;

    private Vector3 targetPosition;

    private Vector3 targetRotationVector;
    private Quaternion targetRotation;

    /// <summary>
    /// Consider this as the cameras current node, stuff happens fast.
    /// </summary>
    public RiverNode targetNode;
    public RiverNode oldTargetNode;

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

    bool blocked=false;
    bool blockedVertical = false;
    bool blockedHorizontal = false;
    public Vector3 offset = Vector3.zero;
    public float autoCorrectRadius=7;
    float oldNodeY=0;

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
            //return;
        }

        Vector3 heading;
        float distance;
        Vector3 direction; // This is the normalized direction.
        Vector3 basePos = transform.position;
        Vector3 adjustPos = Vector3.zero;
        heading = basePos - (boat.transform.position + new Vector3(0, 5, 0));
        distance = heading.magnitude;
        direction = heading / distance;

        RaycastHit hit;
        Debug.DrawRay(transform.position, -heading, Color.red);


        //Debug.DrawRay(transform.position - new Vector3(0, 5, 0), -heading - new Vector3(0, 5, 0), Color.red);
        //Debug.DrawRay(transform.position - new Vector3(0, -5, 0), -heading - new Vector3(0, -5, 0), Color.red);

        blocked = false;
        offset = Vector3.zero;
        bool centerBlocked = false;

        if (Physics.Raycast(transform.position, -heading, out hit, distance))
        {
            
            blockedVertical = false;
            blockedHorizontal = false;

            /*if (hit.collider.gameObject.tag=="River")
                blockedVertical = true;
            else */if (hit.collider.gameObject.tag == "Untagged")
                blockedHorizontal = true;

            if(blockedVertical||blockedHorizontal)
            {
                blocked = true;
                centerBlocked = true;
            }
        }
        else
        {
            adjustPos.Set(0, autoCorrectRadius, 0);
            Debug.DrawRay(transform.position - adjustPos, -heading+adjustPos, Color.red);
            adjustPos = transform.right * autoCorrectRadius;

            Debug.DrawRay(transform.position - adjustPos, -heading + adjustPos, Color.red);
            Debug.DrawRay(transform.position + adjustPos, -heading - adjustPos, Color.red);
            if (Physics.Raycast(transform.position + adjustPos, -heading, out hit, distance))
            {
                adjustPos = -adjustPos;
                if (hit.collider.gameObject.tag == "Untagged")
                {
                    blockedHorizontal = true;
                    blocked = true;
                }
            }
            if (Physics.Raycast(transform.position - adjustPos, -heading, out hit, distance))
            {
                if (hit.collider.gameObject.tag == "Untagged")
                {
                    if (blockedHorizontal == true)
                        blockedHorizontal = false;
                    else
                        blockedHorizontal = true;
                    blocked = true;
                }
            }
            else
            {
                adjustPos.y += 5;
                if (Physics.Raycast(transform.position - adjustPos, -heading + adjustPos, out hit, distance))
                {
                    if (hit.collider.gameObject.tag == "River")
                        blockedVertical = true;

                    if (blockedVertical || blockedHorizontal)
                        blocked = true;
                }
            }
        }

        if (blocked)
        {
            offset = FixBlockedCamera(basePos, adjustPos, blockedVertical, blockedHorizontal);
        }
        //Debug.Log(offset);

        RiverNode boatNode = boat.GetNodes().closest;

        if (boatNode != targetNode)
        {
            if (boatNode != oldTargetNode)
            {
                slopeAngle = targetNode.centerVector.y - boatNode.centerVector.y;
                if (boat.transform.position.y - boatNode.centerVector.y <= 0.5f) slopeAngle = 0;
                //Debug.Log(boat.transform.position.y + " " + boatNode.centerVector.y);
                heading = (targetNode.centerVector + offset + new Vector3(0, slopeAngle, 0)) - boatNode.centerVector;
                distance = heading.magnitude;
                direction = heading / distance; // This is now the normalized direction.

                oldTargetNode = targetNode;
                targetNode = boatNode;

                targetRotation = Quaternion.LookRotation(-direction, Vector3.up);

                newTarget = true;
            }
        }
        else
        {
            if (oldTargetNode.centerVector!=Vector3.zero)
            {
                heading = (oldTargetNode.centerVector + offset + new Vector3(0, slopeAngle, 0)) - targetNode.centerVector;
                distance = heading.magnitude;
                direction = heading / distance; // This is now the normalized direction.

                targetRotation = Quaternion.LookRotation(-direction, Vector3.up);

                if(offset==Vector3.zero&&slopeAngle==0)
                {
                    targetRotation.x = 0;
                    targetRotation.z = 0;
                }
            }

        }


        transform.Rotate(new Vector3(-offsetAngle, 0, 0));
        if(centerBlocked)
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * (rotationSpeed+blockRotationSpeed));
        else
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * rotationSpeed);

        transform.Rotate(new Vector3(offsetAngle, 0, 0));
        //Gets target position
        targetRotationVector = transform.rotation.eulerAngles;
        Quaternion boatOffsetRot = Quaternion.Euler(targetRotationVector.x, targetRotationVector.y, targetRotationVector.z);
        boatOffset = boatOffsetRot * offsetPosition;

        targetPosition = boat.transform.position + boatOffset + shakeOffset;
        transform.position = targetPosition;


    }

    private Vector3 FixBlockedCamera(Vector3 basePos, Vector3 adjustPos, bool blockedVertical, bool blockedHorizontal)
    {
        Vector3 heading;
        float distance;
        Vector3 direction; // This is now the normalized direction.
        Vector3 offset = Vector3.zero;

        heading = basePos - (boat.transform.position + new Vector3(0, 5, 0));
        distance = heading.magnitude;
        direction = heading / distance;

        RaycastHit hit;
        Vector3 moveCamera = basePos;
        moveCamera.y += 5;
        heading = moveCamera - (boat.transform.position + new Vector3(0, 5, 0));
        Debug.DrawRay(moveCamera, -heading, Color.blue);

        if (blockedVertical)
        {
            if (!Physics.Raycast(moveCamera, -heading+adjustPos, out hit, distance))
            {
                offset = adjustPos;
                offset.y += 5;
            }
        }
        /*
        if(blockedHorizontal)
        {
            moveCamera = Vector3.right*5;
            heading = moveCamera - (boat.transform.position + new Vector3(0, 5, 0));
            Debug.DrawRay(moveCamera, -heading, Color.green);
            if (!Physics.Raycast(moveCamera, -heading, out hit, distance))
            {
                offset.z += 50;
            }
            else
            {
                moveCamera = -moveCamera;
                heading = moveCamera - (boat.transform.position + new Vector3(0, 5, 0));
                Debug.DrawRay(moveCamera, -heading, Color.magenta);
                if (!Physics.Raycast(moveCamera, -heading, out hit, distance))
                {
                    offset.z += -50;
                }
            }
        }*/
        return offset;
    }

    private void OnGUI()
    {
        //GUI.Box(new Rect(0, 0, 100, 25), "HULL: " + hull.ToString("F0") + " / " + MaxHull.ToString());
        //if (blocked)
            //GUI.Box(new Rect(0, 50, 100, 25), "Boat obscured");
    }
}