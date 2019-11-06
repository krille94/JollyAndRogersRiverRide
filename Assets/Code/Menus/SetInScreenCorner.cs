using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetInScreenCorner : MonoBehaviour
{
    public float offsetX;
    public float offsetZ;

    // Start is called before the first frame update
    void Start()
    {
        float windowWidth = (float)(Screen.width * 3) / (float)(Screen.height * 4);
        transform.localPosition = new Vector3(offsetX * windowWidth, transform.localPosition.y, offsetZ);

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
