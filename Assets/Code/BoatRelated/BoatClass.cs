using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class BoatClass : MonoBehaviour
{
    [Header("Constants")]
    private Rigidbody body;
    private RiverController river;

    private RiverNode closestNode;

    [Header("Collision")]
    public float collisionKnockback = 100;
    public bool isCollided = true;
    private GameObject otherCollider;
    
    [Header("Counter Mesiures For Going Reverse River Direction")]
    public float counterMesiuresStrenght = 10;
    /// <summary>
    /// Inspector ReadOnly
    /// </summary>
    [SerializeField] private bool isApplyingCountermesiureForce;
    private int lastPassedNodeIndex = 0;

    [Header("Damage Controller")]
    public int MaxHull = 10;
    private float hull;
    public float InvincibilityFrames = 3;
    private float InvincibilityTimer = 0;
    private bool invincible = false;
    public UnityEvent onDeath;
    [SerializeField] PickUpTrigger trigger = null;
    public delegate void OnDamageRecived(float value, Vector3 point);
    public OnDamageRecived onDamaged;
    public ParticleSystem onDamagedParticlePrefab;
    private GameObject WaterLevel = null;

    #region Damage Functions
    public void OnDeath()
    {
        GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>().OnDeath();
    }
    public void RecoverHull(int amount)
    {
        hull += amount;
        if (hull > MaxHull)
            hull = MaxHull;
        SetWaterInBoat();
    }
    private void SetWaterInBoat()
    {
        if (hull == MaxHull)
        {
            if (WaterLevel != null)
                WaterLevel.SetActive(false);
        }
        else
        {
            if (WaterLevel == null)
            {
                // Rewrite this once we have the proper dimensions and such
                GameObject newObj;
                newObj = GameObject.CreatePrimitive(PrimitiveType.Plane);
                Destroy(newObj.GetComponent<MeshCollider>());
                //newObj.GetComponent<MeshCollider>().convex = true;
                newObj.name = "Boat Water";
                newObj.transform.parent = gameObject.transform;

                newObj.transform.localScale = new Vector3(0.3f, 1, 0.45f);
                newObj.transform.localEulerAngles = new Vector3(0, 0, 0);

                Material newMat = Resources.Load("Materials/SimpleWater", typeof(Material)) as Material;
                if (newMat != null)
                    newObj.GetComponent<MeshRenderer>().material = newMat;
                else
                    Debug.LogWarning("Error: Material not found");

                WaterLevel = newObj;
            }
            else if (!WaterLevel.activeInHierarchy)
                WaterLevel.SetActive(true);

            float waterHeight;
            waterHeight = (hull - 1) / (MaxHull - 1);
            WaterLevel.transform.localPosition = new Vector3(0, 1 + (0.7f * (1 - waterHeight)), 0.4f);
        }
    }
    #endregion

    void Start()
    {
        body = GetComponent<Rigidbody>();

        river = RiverController.instance;
        if (river == null)
        {
            river = (RiverController)GameObject.FindGameObjectWithTag("River").GetComponent<RiverController>();
        }

        if (body == null || river == null)
            this.enabled = false;

        hull = MaxHull;
        trigger.onPickUpBucket += RecoverHull;
    }

    void Update()
    {
        //Knockback
        if (isCollided == false)
            return;

        bool stillClose = false;
        Collider[] hits = Physics.OverlapSphere(transform.position, 2.5f);
        for (int i = 0; i < hits.Length; i++)
        {
            if (hits[i].gameObject == otherCollider)
            {
                stillClose = true;
            }
        }
        if (stillClose == false)
            isCollided = false;

        //Counter Mesiures
        closestNode = river.riverAsset.GetNodeFromPosition(transform.position);
        if (closestNode.index > lastPassedNodeIndex)
        {
            lastPassedNodeIndex = closestNode.index;
        }

        if (closestNode.index < lastPassedNodeIndex)
        {
            GetComponent<Rigidbody>().AddForce(river.riverAsset.GetNodeFromIndex(closestNode.index).flowDirection * counterMesiuresStrenght * Time.deltaTime * (lastPassedNodeIndex - closestNode.index));
            isApplyingCountermesiureForce = true;
        }
        else
        {
            isApplyingCountermesiureForce = false;
        }

        //Damage Controll
        if (invincible)
        {
            InvincibilityTimer += Time.deltaTime;
            if (InvincibilityTimer >= InvincibilityFrames)
            {
                InvincibilityTimer = 0;
                invincible = false;
            }
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.transform.tag == "Item")
            return;

        if (!invincible)
        {
            //hull -= collision.relativeVelocity.magnitude;
            hull--;
            SetWaterInBoat();
            invincible = true;
        }

        foreach (ContactPoint contact in collision.contacts)
        {
            onDamaged(collision.relativeVelocity.magnitude, contact.point);

            ParticleSystem particle = Instantiate(onDamagedParticlePrefab, contact.point, Quaternion.identity) as ParticleSystem;
            Destroy(particle.gameObject, particle.main.duration);
        }

        if (hull <= 0)
        {
            Time.timeScale = 0;
            OnDeath();
            onDeath.Invoke();
        }

        //Debug.LogWarning(collision.gameObject.name);
    }

    private void OnCollisionStay(Collision collision)
    {
        //Vector3 target = river.riverAsset.GetNodeFromPosition(river.transform.position, body.transform.position).centerVector;
        if (collision.gameObject.tag == "River")
            return;

        Vector3 target = collision.GetContact(0).point;
        target = target - body.transform.position;
        target.y = 0;
        body.AddForce(-target.normalized * counterMesiuresStrenght);

        otherCollider = collision.gameObject;
        isCollided = true;
    }

    private void OnGUI()
    {
        GUI.Box(new Rect(0, 0, 100, 25), "HULL: " + hull.ToString("F0") + " / " + MaxHull.ToString());
        if (invincible)
            GUI.Box(new Rect(0, 25, 100, 25), "Invincible");
    }
}
