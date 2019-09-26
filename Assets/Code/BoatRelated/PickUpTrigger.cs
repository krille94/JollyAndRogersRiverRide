using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PickUpTrigger : MonoBehaviour
{
    public delegate void OnPickUpGold(int value);
    public OnPickUpGold onPickUpGold;

    public delegate void OnPickUpBucket(int value);
    public OnPickUpBucket onPickUpBucket;

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Item")
        {
            ItemType type = other.gameObject.GetComponent<ItemType>();
            other.gameObject.SetActive(false);
            
            if(type.itemType.ToString()=="Gold")
                onPickUpGold(type.itemValue);
            else if (type.itemType.ToString() == "Bucket")
                onPickUpBucket(type.itemValue);

            Destroy(other.gameObject);
        }
    }
}
