using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

[System.Serializable]
public struct HitPoint
{
    public Vector3 hitPoint;
    public Vector3 hitNormal;

    public Vector3 direction;

    public Vector3 oppositePoint;

    public HitPoint(Vector3 pos, Vector3 normal, Vector3 opposite, RiverSpeedController controller)
    {
        int maxForce = 10;

        hitPoint = pos;

        hitNormal = new Vector3(normal.x, 0, normal.z);
        
        direction = (hitNormal + controller.defaultMovementDirection) * controller.minimumSpeed;
        if (direction.magnitude > maxForce)
        {
            direction.Normalize();
            direction = direction * maxForce;
        }

        hitNormal.Normalize();

        oppositePoint = opposite;
    }
}

public class RiverSpeedController : MonoBehaviour
{
    public int minimumSpeed = 1;
    public Vector3 defaultMovementDirection;

    [HideInInspector] public int baseLenght = 11;
    [HideInInspector] public int resolution = 3;

    public List<HitPoint> hitPoints = new List<HitPoint>();

    private List<Vector3> nextIterationStartPoints = new List<Vector3>();

    //Un-serialized this temporarily because it caused an annoying warning
    /*[SerializeField] */Rigidbody boat;

    private void Update()
    {
        //AddForce_Simple();
    }

    private void AddForce_Simple ()
    {
        Vector3 averagedForce = Vector3.zero;
        int forceCount = 0;
        foreach (HitPoint p in hitPoints)
        {
            float distance = Vector3.Distance(p.hitPoint, boat.position);
            if (distance < 50)
            {
                averagedForce += p.direction * (50 - distance);
                forceCount++;
            }
        }

        averagedForce = averagedForce / forceCount;

        boat.AddForce(averagedForce, ForceMode.Force);
    }

    public void Calculate ()
    {
        int startTime_mili = System.DateTime.Now.Millisecond;
        int startTime_sec = System.DateTime.Now.Second;
        float startTime = (float)startTime_sec + (float)((float)startTime_mili / 60);
        //Debug.Log("Starts Calculation: " + startTime.ToString());

        hitPoints.Clear();
        for (int i = 0; i < baseLenght * resolution; i++)
        {
            Vector3 start = new Vector3(-50 + (i * resolution), 1, -250);
            CheckForward(start);
        }

        while(nextIterationStartPoints.Count > 0)
        {
            CheckForward(nextIterationStartPoints[0]);
            nextIterationStartPoints.RemoveAt(0);
        }

        startTime_mili = System.DateTime.Now.Millisecond;
        startTime_sec = System.DateTime.Now.Second;
        float endTime = (float)startTime_sec + (float)((float)startTime_mili / 60);
        //Debug.Log("Ends Calculation: " + endTime.ToString() + "\nDuration: " + (endTime - startTime).ToString());
        Debug.Log("Duration: " + (endTime - startTime).ToString("F4") + " millisec");
    }

    private void CheckForward (Vector3 start)
    {
        RaycastHit hit;
        if (Physics.Raycast(start, Vector3.forward, out hit))
        {
            if (!hit.transform.GetComponent<MeshCollider>())
                return;

            Vector3 opposite = hit.point + (Vector3.forward * 50);
            RaycastHit colliderHit;
            if (Physics.Raycast(opposite, Vector3.back, out colliderHit))
            {
                opposite = colliderHit.point;
                if (!nextIterationStartPoints.Contains(opposite))
                {
                    bool toCloseForWhileLoop = false;
                    for (int i = 0; i < nextIterationStartPoints.Count; i++)
                    {
                        if(Vector3.Distance(nextIterationStartPoints[i], opposite) < 1)
                        {
                            toCloseForWhileLoop = true;
                        }
                    }
                    if(!toCloseForWhileLoop)
                        nextIterationStartPoints.Add(opposite);
                }
            }

            hitPoints.Add(new HitPoint(hit.point, hit.normal, opposite, this));
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        for (int i = 0; i < baseLenght * resolution; i++)
        {
            Gizmos.DrawSphere(new Vector3(-50 + (i * resolution), 1, -250), resolution);
            Gizmos.DrawRay(new Vector3(-50 + (i * resolution), 1, -250), Vector3.forward * 10);
        }
        
        if(hitPoints.Count > 0)
        {
            
            for (int i = 0; i < hitPoints.Count; i++)
            {   //Hit Point
                Gizmos.color = Color.red;
                Gizmos.DrawSphere(hitPoints[i].hitPoint, resolution);
                Gizmos.DrawRay(hitPoints[i].hitPoint, hitPoints[i].hitNormal * 10);

                //New Flow Direction
                Gizmos.color = Color.green;
                Gizmos.DrawRay(hitPoints[i].hitPoint, hitPoints[i].direction * 5);

                //Line To Opposite Side Of HitPoint
                //Gizmos.color = Color.blue;
                //Gizmos.DrawLine(hitPoints[i].hitPoint, hitPoints[i].oppositePoint);

                //Start From Opposite Side
                Gizmos.color = Color.yellow;
                Gizmos.DrawSphere(hitPoints[i].oppositePoint, resolution);
            }
        }
    }
}
