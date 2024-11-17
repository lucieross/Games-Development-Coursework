using System;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;

    public Sound[] sounds;

    void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
            return;
        }
        else
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }

        // Initialize all sounds in the array
        foreach (Sound s in sounds)
        {
            if (s.clip == null)
            {
                Debug.LogError($"Sound clip for {s.name} is missing.");
                continue; // Skip initialization if clip is null
            }

            s.source = gameObject.AddComponent<AudioSource>();
            s.source.clip = s.clip;
            s.source.volume = s.volume;
            s.source.pitch = s.pitch;
            s.source.loop = s.loop;

            // Assign the AudioSource to the appropriate AudioMixerGroup (if it exists)
            if (s.mixer != null)
            {
                s.source.outputAudioMixerGroup = s.mixer;
            }
            else
            {
                Debug.LogWarning($"AudioMixerGroup for {s.name} is not assigned.");
            }
        }
    }

    public void Play(string sound)
    {
        Sound s = Array.Find(sounds, item => item.name == sound);
        if (s == null)
        {
            Debug.LogWarning($"Sound '{sound}' not found in the sounds array.");
            return; // Exit early if sound is not found
        }

        if (s.source == null)
        {
            Debug.LogError($"AudioSource for sound '{sound}' is not initialized.");
            return; // Exit early if the AudioSource is not initialized
        }

        Debug.Log($"Playing sound: {sound}");
        s.source.Play();
    }

    public void Stop(string sound)
    {
        Sound s = Array.Find(sounds, item => item.name == sound);
        if (s != null && s.source != null)
        {
            s.source.Stop();
        }
    }
}
