using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
public struct CameraPositions
{
    public CameraPositions(Vector3 pos, Quaternion rot, float t) { position = pos; rotation = rot; duration = t; }
    public Vector3 position;
    public Quaternion rotation;
    public float duration;
}

public class MenuCameraFlyby : MonoBehaviour
{
    public CameraPositions[] positionPoints;
    public void AddCameraPosition (Vector3 pos, Quaternion rot, float timeToPlay)
    {
        CameraPositions[] oldPos = positionPoints;
        List<CameraPositions> newPos = new List<CameraPositions>();
        for (int i = 0; i < oldPos.Length; i++)
        {
            newPos.Add(oldPos[i]);
        }
        newPos.Add(new CameraPositions(pos,rot, timeToPlay));
        positionPoints = newPos.ToArray();
    }
    public void SaveCameraPosition (int index, Vector3 pos, Quaternion rot, float timeToPlay)
    {
        for (int i = 0; i < positionPoints.Length; i++)
        {
            if(index == i)
            {
                positionPoints[i] = new CameraPositions(pos, rot, timeToPlay);
            }
        }
    }
    public void RemoveCameraPosition (int index)
    {
        CameraPositions[] oldPos = positionPoints;
        List<CameraPositions> newPos = new List<CameraPositions>();
        for (int i = 0; i < oldPos.Length; i++)
        {
            if (i != index)
                newPos.Add(oldPos[i]);
        }
        positionPoints = newPos.ToArray();
    }

    [SerializeField] float translateSpeed = 3.14f;
    [SerializeField] float rotateSpeed = 3.14f;
    [SerializeField] int positionIndex = 0;
    [SerializeField] float stopingDistance = 0.1f;

    private bool isPlaying = false;
    public void Play()
    {
        transform.parent.gameObject.SetActive(true);
        isPlaying = true;
        positionIndex = 0;
        oriPos = cam.transform.position = positionPoints[positionIndex].position;
        oriRot = cam.transform.rotation = positionPoints[positionIndex].rotation;
    }
    private float timeSpent;
    public Camera cam;
    public void ResetToStart ()
    {
        isPlaying = false;
        positionIndex = 0;
        cam.transform.position = positionPoints[positionIndex].position;
        cam.transform.rotation = positionPoints[positionIndex].rotation;
    }

    [SerializeField] bool playOnAwake;
    [SerializeField] bool loop;
    [SerializeField,Tooltip("This Whill Only Trigger If Looping Is False")] UnityEvent onCompleted;

    private Vector3 oriPos;
    private Quaternion oriRot;

    private void Start()
    {
        if(cam == null)
        {
            Debug.LogWarning("No Camera Is Assigned To The MenuFlyby Script");
            this.enabled = false;
        }
        else
        {
            cam.transform.position = positionPoints[0].position;
            cam.transform.rotation = positionPoints[0].rotation;
        }

        if (playOnAwake)
            isPlaying = true;
    }

    private void Update()
    {
        if (isPlaying == false)
            return;

        timeSpent += Time.deltaTime;

        cam.transform.position = Vector3.Slerp(oriPos, positionPoints[positionIndex].position, timeSpent / positionPoints[positionIndex].duration);
        cam.transform.rotation = Quaternion.Slerp(oriRot, positionPoints[positionIndex].rotation, timeSpent / positionPoints[positionIndex].duration);

        if(Vector3.Distance(cam.transform.position, positionPoints[positionIndex].position) <= stopingDistance)
        {
            if(timeSpent >= positionPoints[positionIndex].duration)
            {
                oriPos = positionPoints[positionIndex].position;
                oriRot = positionPoints[positionIndex].rotation;
                timeSpent = 0;
                positionIndex++;
                if (loop)
                {
                    if (positionIndex >= positionPoints.Length)
                        positionIndex = 0;
                }
                else
                {
                    if (positionIndex >= positionPoints.Length)
                    {
                        onCompleted.Invoke();
                        this.enabled = false;
                        isPlaying = false;
                    }
                }
            }
        }


    }
}
