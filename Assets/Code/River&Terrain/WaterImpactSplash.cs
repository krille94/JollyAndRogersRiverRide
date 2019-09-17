using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterImpactSplash : MonoBehaviour
{
    [SerializeField] ParticleSystem onImpactEffect;
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
            return;

        ParticleSystem particle = Instantiate(onImpactEffect, other.transform.position, Quaternion.identity) as ParticleSystem;
        Destroy(particle.gameObject, particle.duration);
    }
}
