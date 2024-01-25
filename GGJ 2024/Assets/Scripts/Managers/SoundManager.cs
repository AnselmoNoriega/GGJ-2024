using UnityEngine;
using System;

public class SoundManager : MonoBehaviour
{
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
}
