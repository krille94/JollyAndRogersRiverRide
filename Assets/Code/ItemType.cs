using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemType : MonoBehaviour
{
    public enum ItemTypes { Gold, Bucket }
    public ItemTypes itemType = 0;

    public int itemValue = 1;
}
