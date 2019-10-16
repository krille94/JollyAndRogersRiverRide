using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrazyTreeRotation : MonoBehaviour
{
    [SerializeField] float spinSpeed = 10;

    void Update()
    {
        transform.Rotate(Vector3.up * spinSpeed);
    }
}
