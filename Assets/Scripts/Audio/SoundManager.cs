using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

[AddComponentMenu("Sound/Sound Manager")]
public class SoundManager : MonoBehaviour
{
    public static SoundManager instance = null;
    
    public AudioSourceController audioControllerPrefab;
    public Queue<AudioSourceController> audioControllers = new Queue<AudioSourceController>();
    public int sfxObjectPoolSize;    

    [SerializeField] private AudioClip mainMenuMusic;
    [SerializeField] private AudioClip gameMusic;

    [SerializeField] Sound[] sounds;

    private AudioSource musicAudioSource;
    [Range(0.0f, 1.0f)] private float currentSfxVolume;
    [Range(0.0f, 1.0f)] private float currentMusicVolume;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }
        DontDestroyOnLoad(gameObject);
    }

    void Start()
    {
        musicAudioSource = GetComponent<AudioSource>();

        currentMusicVolume = PlayerPrefs.GetFloat("MusicVolume", 1.0f);
        musicAudioSource.volume = currentMusicVolume;

        for (int i = 0; i < sfxObjectPoolSize; i++)
        {
            AudioSourceController controller = Instantiate(audioControllerPrefab, transform);
            audioControllers.Enqueue(controller);
        }

        currentSfxVolume = PlayerPrefs.GetFloat("SfxVolume", 1.0f);
        SetSFXVolume(currentSfxVolume);


        if (SceneManager.GetActiveScene().name == "MainMenu")
        {
            PlayMusic(true);
        }
        else
        {
            PlayMusic(false);
        }

        SceneManager.activeSceneChanged += OnSceneChange;

    }

    private void OnSceneChange(Scene oldScene, Scene newScene)
    {
        if (SceneManager.GetActiveScene().buildIndex == 0)
        {
            PlayMusic(true);
        }
        else 
        {
            if (musicAudioSource.clip != gameMusic)
            {
                PlayMusic(false);
            }
        }
    }

    private void OnDestroy()
    {
        SceneManager.activeSceneChanged -= OnSceneChange;
    }

    public void PlaySFX(string soundName)
    {
        for (int i = 0; i < sounds.Length; i++)
        {
            if (sounds[i].name == soundName)
            {
                sounds[i].Play(audioControllers.Dequeue());
                return;
            }
        }
    }

    public void PlaySFX(AudioClip clip)
    {
        audioControllers.Dequeue().Play(clip,currentSfxVolume, 1.0f);
    }

    public void PlaySFX(AudioClip clip, float volumeScaler, float pitch)
    {
        audioControllers.Dequeue().Play(clip, currentSfxVolume * volumeScaler, pitch);
    }

    public float SetMusicVolume(float value)
    {
        currentMusicVolume += value;
        if (currentMusicVolume > 1.0f)
        {
            currentMusicVolume = 1.0f;
        }
        else if (currentMusicVolume < 0.0f)
        {
            currentMusicVolume = 0.0f;
        }

        print(currentMusicVolume);
        PlayerPrefs.SetFloat("MusicVolume", currentMusicVolume);
        musicAudioSource.volume = currentMusicVolume;
        return currentMusicVolume;
    }

    public float SetSFXVolume(float value)
    {
        currentSfxVolume += value;

        if (currentSfxVolume > 1.0f)
        {
            currentSfxVolume = 1.0f;
        }
        else if (currentSfxVolume < 0.0f)
        {
            currentSfxVolume = 0.0f;
        }

        PlayerPrefs.SetFloat("SfxVolume", currentSfxVolume);
        foreach (Sound sound in sounds)
        {
            sound.volume = currentSfxVolume;
        }

        return currentSfxVolume;
    }

    public void PlayMusic(bool isMainMenu)
    {
        if (isMainMenu)
        {
            if (mainMenuMusic != null)
            {
                musicAudioSource.clip = mainMenuMusic;
                musicAudioSource.Play();
            }
        }
        else
        {
            if (gameMusic != null)
            {
                musicAudioSource.clip = gameMusic;
                musicAudioSource.Play();
            }
        }
    }

    public void StopMusic()
    {
        if (musicAudioSource.isPlaying)
        {
            musicAudioSource.Stop();
        }
    }
}

[System.Serializable]
public class Sound
{
    public string name;
    public AudioClip clip;
    [Range(-3.0f, 3.0f)] public float pitch = 1;
    [Range(0.0f,1.0f)]public float volume = 1;

    public void Play(AudioSourceController source)
    {
        if (clip == null || source == null)
        {
            return;
        }

        source.Play(clip,volume,pitch);
    }


    /**
 * Creates a sub clip from an audio clip based off of the start time
 * and the stop time. The new clip will have the same frequency as
 * the original.
 */
    private AudioClip MakeSubclip(AudioClip clip, float start, float stop)
    {
        /* Create a new audio clip */
        int frequency = clip.frequency;
        float timeLength = stop - start;
        int samplesLength = (int)(frequency * timeLength);
        AudioClip newClip = AudioClip.Create(clip.name + "-sub", samplesLength, 1, frequency, false);
        /* Create a temporary buffer for the samples */
        float[] data = new float[samplesLength];
        /* Get the data from the original clip */
        clip.GetData(data, (int)(frequency * start));
        /* Transfer the data to the new clip */
        newClip.SetData(data, 0);
        /* Return the sub clip */
        return newClip;
    }
}