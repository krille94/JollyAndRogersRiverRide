﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloatingObject : MonoBehaviour
{
    [Header("Constants")]
    protected Rigidbody body;
    public Rigidbody GetRigidbody() { return body; }

    protected RiverController river;
    [SerializeField]
    protected RiverNode closestNode;
    [SerializeField]
    protected RiverNode lastNode;
    public struct FloatingObjectNodes
    {
        public FloatingObjectNodes(RiverNode c, RiverNode l) { closest = c; last = l; }
        public RiverNode closest; public RiverNode last;
    }
    public FloatingObjectNodes GetNodes() { return new FloatingObjectNodes(closestNode,lastNode); }
    public void UpdateNodes (RiverNode closest)
    {
        if(lastNode.centerVector == Vector3.zero|| closestNode.centerVector == Vector3.zero)
        {
            closestNode = closest;
            lastNode = closest;
            return;
        }

        if(closest.index > closestNode.index)
        {
            lastNode = closestNode;
        }

        //Is using index more efficient? All nodes have index 0, so comparing indexes won't work for now
        //if(closest.index != closestNode.index)
        if (closest != closestNode)
        {
            lastNode = closestNode;
            closestNode = closest;
            nodeProgressClosestDist = Vector3.Distance(transform.position, closestNode.centerVector);

            if (trackNodeProgress)
            {
                nodeProgress = Vector3.Distance(transform.position, closestNode.centerVector);
                if (nodeProgress <= nodeProgressClosestDist)
                {
                    nodeProgressClosestDist = nodeProgress;
                    reverseProgress = false;
                }
                else
                {
                    reverseProgress = true;
                }
            }
        }
        else if(closest.index == lastNode.index)
        {
            if (trackNodeProgress)
            {
                nodeProgress = Vector3.Distance(transform.position, closestNode.centerVector);
                if (nodeProgress >= nodeProgressClosestDist)
                {
                    nodeProgressClosestDist = nodeProgress;
                    reverseProgress = false;
                }
                else
                {
                    reverseProgress = true;
                }
            }
        }
    }

    public bool trackNodeProgress = true;
    [HideInInspector] public float nodeProgress;
    [HideInInspector] public float nodeProgressClosestDist;
    [HideInInspector] public bool reverseProgress;

    public List<GameObject> observers = new List<GameObject>();

    void Start()
    {
        Initialize();
    }

    protected virtual void Initialize()
    {
        body = GetComponent<Rigidbody>();

        river = RiverController.instance;
        if (river == null)
        {
            river = (RiverController)GameObject.FindGameObjectWithTag("River").GetComponent<RiverController>();
        }

        if (body == null || river == null)
            this.enabled = false;
    }

    private void OnDestroy()
    {
        foreach(GameObject observer in observers)
        {
            if (observer == null)
                continue;
            observer.SendMessage("OnDestroyed", this);
        }
    }
}
