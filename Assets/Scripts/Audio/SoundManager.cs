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
    [SerializeField] private AudioClip transistionMusic;

    [SerializeField] Sound[] sounds;
    [SerializeField] Sound[] exclusiveSounds;
    private AudioSource musicAudioSource;
    [Range(0.0f, 1.0f)] private float currentSfxVolume;
    [Range(0.0f, 1.0f)] private float currentMusicVolume;

    private bool isMainMenu;

    const float VOLUME_SCALER = 0.5f;

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

        currentMusicVolume = PlayerPrefs.GetFloat("MusicVolume", 0.5f);
        musicAudioSource.volume = currentMusicVolume * VOLUME_SCALER;

        for (int i = 0; i < sfxObjectPoolSize; i++)
        {
            AudioSourceController controller = Instantiate(audioControllerPrefab, transform);
            audioControllers.Enqueue(controller);
        }

        for (int i = 0; i < exclusiveSounds.Length; i++)
        {
            GameObject _go = new GameObject("Sound_" + i + "_" + exclusiveSounds[i].name);
            _go.transform.parent = transform;
            exclusiveSounds[i].audioSource = _go.AddComponent<AudioSource>();
        }
        
        currentSfxVolume = PlayerPrefs.GetFloat("SfxVolume", 0.5f);
        SetSFXVolume(currentSfxVolume);

        if (SceneManager.GetActiveScene().buildIndex == 0)
        {
            PlayMusic(true);
            isMainMenu = true;
        }
        else
        {
            PlayMusic(false);
            isMainMenu = false;
        }

        SceneManager.activeSceneChanged += OnSceneChange;
    }

    private void OnSceneChange(Scene oldScene, Scene newScene)
    {
        if (SceneManager.GetActiveScene().buildIndex == 0)
        {
            PlayMusic(true);
            isMainMenu = true;
        }
        else 
        {
            if (isMainMenu)
            {
                StartCoroutine("TransistionToGameMusic");
                isMainMenu = false;
            }
            else
            {
                if (musicAudioSource.clip != gameMusic)
                {
                    PlayMusic(false);
                    isMainMenu = false;
                }
            }
           
        }
    }

    IEnumerator TransistionToGameMusic ()
    {
        float delay = 0;
        if (transistionMusic != null)
        {
            delay = transistionMusic.length;
            musicAudioSource.clip = transistionMusic;
            musicAudioSource.Play();
        }
        yield return new WaitForSeconds(delay);
        PlayMusic(false);
    }

    private void OnDestroy()
    {
        SceneManager.activeSceneChanged -= OnSceneChange;
    }

    public AudioSourceController PlaySFX(string soundName)
    {
        for (int i = 0; i < sounds.Length; i++)
        {
            if (sounds[i].name == soundName)
            {
                if (sounds[i].clip == null)
                {
                    return null;
                }
                AudioSourceController controller = audioControllers.Dequeue();
                sounds[i].Play(controller, currentSfxVolume * VOLUME_SCALER);
                CheckOutOfSources();
                return controller;
            }
        }
        return null;
    }

    public void PlaySFX(AudioClip clip)
    {
        PlaySFX(clip,currentSfxVolume , 1.0f);
    }

    public void PlaySFX(AudioClip clip, float volumeScaler, float pitch)
    {
        audioControllers.Dequeue().Play(clip, currentSfxVolume * volumeScaler * VOLUME_SCALER, pitch);
        CheckOutOfSources();
    }

    public void PlayExclusiveSFX (string soundName)
    {
        for (int i = 0; i < exclusiveSounds.Length; i++)
        {
            if (exclusiveSounds[i].name == soundName)
            {
                exclusiveSounds[i].PlayExclusive(currentSfxVolume * VOLUME_SCALER);
                print("Play Exclusive SFX");
                return;
            }
        }
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
        musicAudioSource.volume = currentMusicVolume * VOLUME_SCALER;
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

    void CheckOutOfSources ()
    {
        if (audioControllers.Count <= 1)
        {
            Debug.LogError("Out of audio source controller, increase buffer size");
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
    public AudioSource audioSource;

    public void Play(AudioSourceController source, float currentSfx)
    {
        if (clip == null || source == null)
        {
            return;
        }

        source.Play(clip,volume * currentSfx,pitch);
    }

    public void PlayExclusive (float currentSfxVolume)
    {
        if (clip == null || audioSource == null)
        {
            Debug.Log("Clip or source is null");
            return;
        }
        Debug.Log("Play Exclusive" + clip.name);

        audioSource.clip = clip;
        audioSource.volume = volume * currentSfxVolume;
        audioSource.pitch = pitch;
        audioSource.Play();
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