using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloatingObject : MonoBehaviour
{
    [Header("Constants")]
    protected Rigidbody body;
    public Rigidbody GetRigidbody() { return body; }

    protected RiverController river;
    protected RiverNode closestNode;
    protected RiverNode lastNode;
    public struct FloatingObjectNodes
    {
        public FloatingObjectNodes(RiverNode c, RiverNode l) { closest = c; last = l; }
        public RiverNode closest; public RiverNode last;
    }
    public FloatingObjectNodes GetNodes() { return new FloatingObjectNodes(closestNode,lastNode); }
    public void UpdateNodes (RiverNode c, RiverNode l)
    {
        closestNode = c;
        lastNode = l;
    }

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
            observer.SendMessage("OnDestroyed", this);
        }
    }
}
