using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using UnityEngine;
using UnityEngine.Audio;

public static class UserSettings
{
    private static float musicVolume;
    private static float sfxVolume;

    public static void ReadSettings()
    {
        musicVolume = PlayerPrefs.GetFloat("Music");
        sfxVolume = PlayerPrefs.GetFloat("Sound Effects");

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

    public static void SaveFloat(string name, float value)
    {
        PlayerPrefs.SetFloat(name, value);
    }
}
