using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BigWhirlwindScript : MonoBehaviour
{
    Transform rotationObj;

    [SerializeField]
    List<FloatingObject> observerdObjects = new List<FloatingObject>();

    public float centrificalForce = 10;
    public float inwardForce = 5;
    public float rotateAnimSpeed = -60;

    void Start()
    {
        GameObject obj = new GameObject("directionTransform");
        obj.transform.SetParent(transform);
        rotationObj = obj.transform;
    }

    void Update()
    {
        transform.Rotate(Vector3.up * rotateAnimSpeed * Time.deltaTime);

        if (!GameObject.Find("Pause Screen"))
        {
            for (int i = 0; i < observerdObjects.Count; i++)
            {
                rotationObj.position = observerdObjects[i].transform.position;
                rotationObj.LookAt(transform);

                observerdObjects[i].GetRigidbody().AddForce(((-observerdObjects[i].GetNodes().closest.finalFlowDirection * inwardForce) + (((rotationObj.right + rotationObj.forward) / 2) * centrificalForce) * Time.deltaTime));
            }
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.tag == "Player")
        {
            other.GetComponent<BoatClass>().UpdateDamage(SpeedValueManager.GetSpeedValues().Count - 1);
        }

        if (other.GetComponent<FloatingObject>())
        {
            if(observerdObjects.Contains(other.GetComponent<FloatingObject>()) == false)
            {
                other.GetComponent<FloatingObject>().observers.Add(gameObject);
                observerdObjects.Add(other.GetComponent<FloatingObject>());
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            other.GetComponent<BoatClass>().UpdateDamage();
        }

        if (other.GetComponent<FloatingObject>())
        {
            other.GetComponent<FloatingObject>().observers.Remove(gameObject);
            observerdObjects.Remove(other.GetComponent<FloatingObject>());
        }
    }

    public void OnDestroyed (FloatingObject obj)
    {
        observerdObjects.Remove(obj);
    }
}
