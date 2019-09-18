using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterImpactSplash : MonoBehaviour
{
    [SerializeField] ParticleSystem onImpactEffect = null;
    [SerializeField] AudioSource onImpactSound = null;

    private Transform effectsPool;

    private void Start()
    {
        effectsPool = GameObject.Find("EffectsPool").transform;
        if(effectsPool == null)
        {
            effectsPool = new GameObject("EffectsPool").transform;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Boat"))
            return;

        ParticleSystem particle = Instantiate(onImpactEffect, other.transform.position, Quaternion.identity) as ParticleSystem;
        particle.transform.SetParent(effectsPool, true);
        Destroy(particle.gameObject, particle.main.duration);

        AudioSource audio = Instantiate(onImpactSound, other.transform.position, Quaternion.identity) as AudioSource;
        audio.transform.SetParent(effectsPool, true);
        Destroy(audio.gameObject, audio.clip.length);
    }
}
