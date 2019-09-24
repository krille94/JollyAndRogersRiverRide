﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class BoatDamageController : MonoBehaviour
{
    public int MaxHull = 1000;
    private float hull;

    public UnityEvent onDeath;

    [SerializeField] PickUpTrigger trigger = null;
    public delegate void OnDamageRecived(float value, Vector3 point);
    public OnDamageRecived onDamaged;

    public ParticleSystem onDamagedParticlePrefab;

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
    }

    // Update is called once per frame
    void Update()
    {
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.transform.tag == "Item")
            return;

        hull -= collision.relativeVelocity.magnitude;
        
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

    private void OnGUI()
    {
        GUI.Box(new Rect(0, 0, 100, 25), "HULL: " + hull.ToString("F0") + " / " + MaxHull.ToString());
    }
}
