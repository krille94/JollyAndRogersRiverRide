using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

//[ExecuteInEditMode]
public class AmbientSoundObject : MonoBehaviour
{
    private AmbientSoundsController controller;

    public float triggerRange = 1;

    public bool muteOnExit;
    private bool fading;

    private new AudioSource audioSource;

    [HideInInspector]public Vector3 triggerRangeHandle;

    [SerializeField] AudioClip[] audioClips = null;
    [SerializeField] AudioMixer audioMixer = null;

    private void Start()
    {
        controller = AmbientSoundsController.controller;

        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
            audioSource = gameObject.AddComponent<AudioSource>();

        if(audioClips.Length > 0)
            audioSource.clip = audioClips[0];

        if (audioMixer != null)
            audioSource.outputAudioMixerGroup = audioMixer.outputAudioMixerGroup;
    }

    private void Update()
    {
        if (controller == null)
            return;

        if (fading)
        {
            audioSource.volume -= Time.deltaTime;
            if (audioSource.volume <= 0)
            {
                fading = false;
                audioSource.volume = 1;
                audioSource.Stop();
            }
            return;
        }
        
        if(Vector3.Distance(transform.position, controller.playerBoat.transform.position) <= triggerRange)
        {
            if (!audioSource.isPlaying)
                audioSource.PlayOneShot(audioClips[Random.Range(0, audioClips.Length - 1)]);
        }
        else if(muteOnExit)
        {
            if (audioSource.isPlaying)
                fading = true;
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(transform.position, triggerRange);
    }


}
