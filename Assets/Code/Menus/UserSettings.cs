﻿using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using UnityEngine;
using UnityEngine.Audio;

public static class UserSettings
{
    private static float musicVolume;
    private static float sfxVolume;
    private static bool reversedControls;
    private static int controlScheme;

    public static void ReadSettings()
    {
        musicVolume = PlayerPrefs.GetFloat("Music");
        sfxVolume = PlayerPrefs.GetFloat("Sound Effects");
        reversedControls = (PlayerPrefs.GetInt("Reversed Controls") == 1);
        controlScheme = PlayerPrefs.GetInt("Control Scheme");
        if (controlScheme == 0) SetInt("Control Scheme", 1);

        AudioMixer mixer;
        mixer = Resources.Load("AudioMixers/Music") as AudioMixer;
        mixer.SetFloat("volume", musicVolume);

        mixer = Resources.Load("AudioMixers/Sound Effects") as AudioMixer;
        mixer.SetFloat("volume", sfxVolume);

    }

    public static float GetVolume(string soundType)
    {
        if (soundType == "Music")
            return musicVolume;
        else
            return sfxVolume;
    }

    public static bool GetReversedControls()
    {
        return reversedControls;
    }

    public static int GetControlScheme()
    {
        return controlScheme;
    }

    public static void SetFloat(string name, float value)
    {
        PlayerPrefs.SetFloat(name, value);
    }

    public static void SetInt(string name, int value)
    {
        PlayerPrefs.SetInt(name, value);
    }
}
