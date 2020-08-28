using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class AudioSourceController : MonoBehaviour
{
    private AudioSource source;

    private float duration;

    void Start()
    {
        source = GetComponent<AudioSource>();
        source.loop = false;
    }

    public void Play(AudioClip clip, float volume, float pitch)
    {
        source.clip = clip;
        source.volume = volume;
        source.pitch = pitch;
        source.Play();
        duration = clip.length;
        StartCoroutine("CoPlay");
    }

    public void StopPlaying ()
    {
        source.Stop();
        StopCoroutine("CoPlay");
        SoundManager.instance.audioControllers.Enqueue(this);
    }

    IEnumerator CoPlay ()
    {
        yield return new WaitForSeconds(duration);
        SoundManager.instance.audioControllers.Enqueue(this);
    }
}
