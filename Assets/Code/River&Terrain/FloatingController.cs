using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloatingController : MonoBehaviour
{
    public enum SystemTypes { Arcade, Physics}
    public SystemTypes usedSystemType;

    public float arcadeBouance = 100;
    public float physicsBouance = 75000;

    private MeshFilter meshF;

    private void Start()
    {
        meshF = GetComponent<MeshFilter>();
    }

    private void OnTriggerStay(Collider other)
    {
        if (usedSystemType == SystemTypes.Arcade)
        {
            if (other.GetComponent<Rigidbody>())
            {
                //  if(other.GetComponent<Rigidbody>().velocity.magnitude <= 1)
                //  {
                int targetIndex = 0;
                int currentIndex = 0;
                float closestDist = Mathf.Infinity;
                foreach (Vector3 vert in meshF.sharedMesh.vertices)
                {
                    float dist = Vector3.Distance(other.attachedRigidbody.transform.position, vert);
                    if (dist < closestDist)
                    {
                        closestDist = dist;
                        targetIndex = currentIndex;
                    }
                    currentIndex++;
                }



                float targetSurfacingHeight = meshF.sharedMesh.vertices[targetIndex].y;
                targetSurfacingHeight += other.attachedRigidbody.mass / arcadeBouance;
                Vector3 targetVector = new Vector3(other.attachedRigidbody.transform.position.x, targetSurfacingHeight, other.attachedRigidbody.transform.position.z);
                other.attachedRigidbody.transform.position = Vector3.Lerp(other.transform.position, targetVector, Time.fixedDeltaTime);

                Debug.DrawLine(other.attachedRigidbody.transform.position, targetVector);
                return;
                // }
            }
        }

        if (usedSystemType == SystemTypes.Physics)
        {
            float distance = 1;// collider.bounds.SqrDistance(other.transform.position);
            distance = Mathf.Abs(other.transform.position.y - transform.position.y);

            other.attachedRigidbody.AddForce(Vector3.up * (physicsBouance / other.attachedRigidbody.mass) * distance * Time.fixedDeltaTime);

            Debug.DrawRay(other.transform.position, Vector3.up * ((physicsBouance / other.attachedRigidbody.mass) * distance) / 2500, Color.blue);
            //Debug.Log(other.name + " is colliding with water surface");
        }
    }
    
    private void OnTriggerEnter(Collider other)
    {
        if (usedSystemType != SystemTypes.Physics)
            return;

        if(other.GetComponent<Rigidbody>())
            other.GetComponent<Rigidbody>().drag = 2;
    }

    private void OnTriggerExit(Collider other)
    {
        if (usedSystemType != SystemTypes.Physics)
            return;

        if (other.GetComponent<Rigidbody>())
            other.GetComponent<Rigidbody>().drag = 1;
    }
}
