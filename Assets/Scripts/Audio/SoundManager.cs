using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

[AddComponentMenu("Sound/Sound Manager")]
public class SoundManager : MonoBehaviour
{
    public static SoundManager instance = null;
    AudioSource musicAudioSource;
    List<AudioSource> sfx = new List<AudioSource>();

    [SerializeField] private AudioClip mainMenuMusic;
    [SerializeField] private AudioClip gameMusic;

    [SerializeField]
    Sound[] sounds;

    public List<PlayerAudio> playerAudioList = new List<PlayerAudio>();

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


        for (int i = 0; i < sounds.Length; i++)
        {
            GameObject _go = new GameObject("Sound_" + i + "_" + sounds[i].name);
            _go.transform.parent = transform;
            sounds[i].SetSource(_go.AddComponent<AudioSource>());
            sfx.Add(_go.GetComponent<AudioSource>());
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
                sounds[i].Play();
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
        foreach (AudioSource source in sfx)
        {
            source.volume = currentSfxVolume;
        }

        foreach (PlayerAudio audio in playerAudioList)
        {
            audio.SetVolume(currentSfxVolume);
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
    [HideInInspector] public AudioSource source;
    [Range(-3.0f, 3.0f)] public float pitch = 1;
    [Range(0.0f,1.0f)]public float volume = 1;


    public void SetSource(AudioSource _source)
    {
        source = _source;
        source.clip = clip;
        source.pitch = pitch;

    }



    public void Play()
    {
        if (clip == null || source == null)
        {
            return;
        }

        source.Play();

    }
}