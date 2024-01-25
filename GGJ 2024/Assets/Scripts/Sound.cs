using UnityEngine;

[System.Serializable]
public class Sound
{
    public string Name;
    public AudioClip AudioClip;
    public bool Loop;
    [Range(0, 1)]
    public float Volume;
    [Range(-3, 3)]
    public float Pitch;
    public AudioSource AudioSrc { get; set; }
}
