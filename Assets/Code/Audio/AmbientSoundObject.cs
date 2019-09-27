﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

[ExecuteInEditMode]
public class AmbientSoundObject : MonoBehaviour
{
    private AmbientSoundsController controller;

    public float triggerRange = 1;

    private new AudioSource audio;

    [HideInInspector]public Vector3 triggerRangeHandle;

    [SerializeField] AudioClip audioClip = null;
    [SerializeField] AudioMixer audioMixer = null;

    private void Start()
    {
        controller = AmbientSoundsController.controller;

        audio = GetComponent<AudioSource>();
        if (audio == null)
            audio = gameObject.AddComponent<AudioSource>();

        audio.clip = audioClip;
        if (audioMixer != null)
            audio.outputAudioMixerGroup = audioMixer.outputAudioMixerGroup;
    }

    private void Update()
    {
        if (controller == null)
            return;
        
        if(Vector3.Distance(transform.position, controller.playerBoat.transform.position) <= triggerRange)
        {
            if (!audio.isPlaying)
                audio.Play();
        }
        else
        {
            if (audio.isPlaying)
                audio.Stop();
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(transform.position, triggerRange);
    }


}
