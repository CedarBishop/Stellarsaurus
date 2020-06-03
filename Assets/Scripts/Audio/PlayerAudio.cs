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
        SoundManager.instance.playerAudioList.Add(this);

        if (PlayerPrefs.HasKey("SfxVolume"))
        {
            audioSource.volume = PlayerPrefs.GetFloat("SfxVolume", 0.5f);
        }
    }

    private void OnDestroy()
    {
        SoundManager.instance.playerAudioList.Remove(this);
    }

    public void PlaySFX (AudioClip clip)
    {
        audioSource.clip = clip;
        audioSource.Play();
    }

    public void SetVolume (float value)
    {
        audioSource.volume = value; 
    }
}
