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

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = Vector3.Lerp(boat.transform.position + offset, transform.position, Time.deltaTime * speed);

        transform.LookAt(boat.transform.position + new Vector3(0, viewRot, 0));
    }
}
