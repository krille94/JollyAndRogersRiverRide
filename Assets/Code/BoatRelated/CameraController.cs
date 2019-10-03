using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class CameraController : MonoBehaviour
{
    public RiverController riverController;
    public Transform boat;

    public Vector3 offset;
    public float viewRot;

    public float speed = 0.25f;

    public Vector3 targetPosition;

    void Update()
    {
        RiverNode node = riverController.riverAsset.GetNodeFromPosition(boat.position);

        targetPosition = node.centerVector + offset;

        transform.position = Vector3.Lerp(transform.position, targetPosition, Time.deltaTime * speed);

        transform.LookAt(boat);
    }
}
