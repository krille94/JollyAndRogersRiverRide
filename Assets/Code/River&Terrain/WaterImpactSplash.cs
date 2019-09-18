using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterImpactSplash : MonoBehaviour
{
    [SerializeField] ParticleSystem onImpactEffect;
    [SerializeField] AudioSource onImpactSound;
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
            return;

        ParticleSystem particle = Instantiate(onImpactEffect, other.transform.position, Quaternion.identity) as ParticleSystem;
        Destroy(particle.gameObject, particle.duration);

        AudioSource audio = Instantiate(onImpactSound, other.transform.position, Quaternion.identity) as AudioSource;
        Destroy(audio.gameObject, audio.clip.length);
    }
}
