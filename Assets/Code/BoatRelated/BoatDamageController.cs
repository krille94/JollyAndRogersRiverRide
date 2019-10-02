using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class BoatDamageController : MonoBehaviour
{
    public int MaxHull = 10;
    private float hull;

    public float InvincibilityFrames = 3;
    private float InvincibilityTimer = 0;
    private bool invincible=false;

    public UnityEvent onDeath;

    [SerializeField] PickUpTrigger trigger = null;

    public delegate void OnDamageRecived(float value, Vector3 point);
    public OnDamageRecived onDamaged;

    public ParticleSystem onDamagedParticlePrefab;

    private GameObject WaterLevel = null;

    public void OnDeath()
    {
        GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>().OnDeath();
    }

    private void Start()
    {
        hull = MaxHull;
        trigger.onPickUpBucket += RecoverHull;
    }

    public void RecoverHull(int amount)
    {
        hull += amount;
        if (hull > MaxHull)
            hull = MaxHull;
        SetWaterInBoat();
    }

    // Update is called once per frame
    void Update()
    {
        if(invincible)
        {
            InvincibilityTimer += Time.deltaTime;
            if(InvincibilityTimer>=InvincibilityFrames)
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

        if(!invincible)
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
                newObj.GetComponent<MeshCollider>().convex = true;
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
            else if(!WaterLevel.activeInHierarchy)
                WaterLevel.SetActive(true);

            float waterHeight;
            waterHeight = (hull - 1) / (MaxHull - 1);
            WaterLevel.transform.localPosition = new Vector3(0, 1 + (0.7f * (1 - waterHeight)), 0.4f);
        }
    }

    private void OnGUI()
    {
        GUI.Box(new Rect(0, 0, 100, 25), "HULL: " + hull.ToString("F0") + " / " + MaxHull.ToString());
        if(invincible)
            GUI.Box(new Rect(0, 25, 100, 25), "Invincible");
    }
}
