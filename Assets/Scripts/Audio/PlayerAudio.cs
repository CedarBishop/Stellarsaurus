using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioClip))]
public class PlayerAudio : MonoBehaviour
{
    AudioSource audioSource;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    public void PlaySFX (AudioClip clip)
    {
        audioSource.clip = clip;
        audioSource.Play();
    }
}
