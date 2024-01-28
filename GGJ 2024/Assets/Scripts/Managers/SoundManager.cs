using UnityEngine;
using System;

public class SoundManager : MonoBehaviour
{
    private Sound _currrentMainSound;
    [SerializeField] private Sound[] _sounds;

    private void Awake()
    {
        foreach(var sound in _sounds)
        {
            sound.AudioSrc = gameObject.AddComponent<AudioSource>();
            sound.AudioSrc.clip = sound.AudioClip;
            sound.AudioSrc.loop = sound.Loop;
            sound.AudioSrc.volume = sound.Volume;
            sound.AudioSrc.pitch = sound.Pitch;
        }
    }

    public void PlaySound(string name)
    {
        Sound mySound = Array.Find(_sounds, sound => sound.Name == name);
        if(mySound != null)
        {
            mySound.AudioSrc.Play();
        }
    }

    public void PlayMainSound(string name)
    {
        Sound mySound = Array.Find(_sounds, sound => sound.Name == name);
        if (mySound != null)
        {
            StopMainSound(name);
            mySound.AudioSrc.Play();
            _currrentMainSound = mySound;
        }
    }

    public void StopSound(string name)
    {
        Sound mySound = Array.Find(_sounds, sound => sound.Name == name);
        if (mySound != null)
        {
            mySound.AudioSrc.Stop();
        }
    }

    public void StopMainSound(string name)
    {
        if (_currrentMainSound != null)
        {
            _currrentMainSound.AudioSrc.Stop();
        }
    }
}
