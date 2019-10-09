using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicController : MonoBehaviour
{
    public AudioClip menuBackground;
    public AudioClip gameBackground;

    private AudioSource source;

    void Start()
    {
        DontDestroyOnLoad(gameObject);
        source = GetComponent<AudioSource>();

        source.clip = menuBackground;
    }

    void Update()
    {
        if (source.isPlaying == false)
            source.Play();
    }
}
