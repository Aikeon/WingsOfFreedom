using System.Collections;
using System.Collections.Generic;
using System;
using sound;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;

public class AudioManager : MonoBehaviour
{
    public String StartSound = "";

    public bool StartSoundPlaying = false;

    [System.Serializable]
    public class Sound
    {
        public string name;
        public AudioClip clip;
        
        [Range(0f, 1f)]
        public float volume;

        [Range(0.1f, 3f)]
        public float pitch;

        [System.NonSerialized]
        public AudioSource source;

        [System.NonSerialized]
        public CopyTransform copyTransform;

        [System.NonSerialized]
        public GameObject sourceObject;

        public bool loop;

    }

    public Sound[] sounds;

    public void Awake()
    {
        foreach (Sound s in sounds)
        {
            var sound = new GameObject(s.name, typeof(AudioSource), typeof(CopyTransform));
            sound.transform.SetParent(this.transform, false);
            s.copyTransform = sound.GetComponent<CopyTransform>();
            s.source = sound.GetComponent<AudioSource>();
            s.sourceObject = sound;
            s.source.clip = s.clip;
            s.source.volume = s.volume;
            s.source.pitch = s.pitch;
            s.source.loop = s.loop;
        }
    }


    public void Mute(string name)
    {
        Sound s = Array.Find(sounds, sound => sound.name == name);

        if (s == null)
        {
            Debug.LogWarning("Sound : " + name + " not found");
            return;
        }

        s.source.mute = !s.source.mute;
    }

    public void Play(string name)
    {
        Sound s = Array.Find(sounds, sound => sound.name == name);

        if (s == null)
        {
            Debug.LogWarning("Sound : " + name + " not found");
            return;
        }

        Debug.Log("son " + name + " joué");
        s.source.Play();

    }

    public void PlayAtPosition(string name, Vector3 position)
    {
        Sound s = Array.Find(sounds, sound => sound.name == name);

        if (s == null)
        {
            Debug.LogWarning("Sound : " + name + " not found");
            return;
        }

        AudioSource.PlayClipAtPoint(s.clip, position, s.volume);
    }

    public void Pause(string name)
    {
        Sound s = Array.Find(sounds, sound => sound.name == name);

        if (s == null)
        {
            Debug.LogWarning("Sound : " + name + " not found");
            return;
        }


        s.source.Pause();

    }

    public void Stop(string name)
    {
        Sound s = Array.Find(sounds, sound => sound.name == name);

        if (s == null)
        {
            Debug.LogWarning("Sound : " + name + " not found");
            return;
        }


        s.source.Stop();

    }

    public void Start()
    {
        if (StartSoundPlaying == true)
        {
            Play(StartSound);
        }
    }

}