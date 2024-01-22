using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayAudioShot : MonoBehaviour
{
    [SerializeField] AudioClip soundEffect;
    [SerializeField] float volume;
    [SerializeField] Vector2 pitch;
    [SerializeField] float trim;
    AudioSource audioSource;

    void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }

    public void PlayAudio()
    {
        float[] samples = new float[soundEffect.samples * soundEffect.channels];
        soundEffect.GetData(samples, 0);

        int trimValue = (int)Mathf.Lerp(0, samples.Length, trim);
        float[] newSamples = new float[samples.Length - trimValue];
        for (int i = 0; i < newSamples.Length; i++)
        {
            newSamples[i] = samples[i + trimValue];
        }

        AudioClip newClip = AudioClip.Create("Temp", soundEffect.samples, soundEffect.channels, soundEffect.frequency, false);

        newClip.SetData(newSamples, 0);

        float newPitch = Random.Range(pitch.x, pitch.y);
        audioSource.pitch = newPitch;
        audioSource.PlayOneShot(newClip, volume);
        audioSource.time = trim;
    }
}