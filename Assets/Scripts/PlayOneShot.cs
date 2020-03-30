using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonClick : MonoBehaviour
{
    public AudioClip ClickSound;                //button click sound

    private AudioSource audioSource;            //audiosource in the scene handling the sound

    private void Start()
    {
        // to find an audiosource in the scene
        if (audioSource == null)
            audioSource = FindObjectOfType<AudioSource>();
    }

    //plays a sound once
    public void PlaySound()
    {
        audioSource.PlayOneShot(ClickSound);
    }

}