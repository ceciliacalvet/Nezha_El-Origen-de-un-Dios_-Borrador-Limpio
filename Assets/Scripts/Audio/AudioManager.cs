using UnityEngine;

#nullable enable

[DisallowMultipleComponent]
public class AudioManager : MonoBehaviour
{
    public static AudioManager? Instance { get; private set; }

    [Header("Background Music")]
    [Tooltip("Background music clip to play when the scene starts.")]
    public AudioClip? backgroundMusic;

    [Tooltip("Play background music automatically on start.")]
    public bool playOnAwake = true;

    [Tooltip("Loop the background music clip.")]
    public bool loopMusic = true;

    [Range(0f, 1f)]
    [Tooltip("Volume for background music.")]
    public float musicVolume = 0.75f;

    [Header("Sound Effects")]
    [Range(0f, 1f)]
    [Tooltip("Default volume for one-shot sound effects.")]
    public float sfxVolume = 1f;

    private AudioSource? musicSource;
    private AudioSource? sfxSource;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
        InitializeSources();
    }

    private void Start()
    {
        if (playOnAwake && backgroundMusic != null)
        {
            PlayMusic(backgroundMusic, loopMusic);
        }
    }

    private void InitializeSources()
    {
        if (musicSource == null)
        {
            musicSource = gameObject.AddComponent<AudioSource>();
            musicSource.playOnAwake = false;
            musicSource.loop = loopMusic;
            musicSource.volume = musicVolume;
        }

        if (sfxSource == null)
        {
            sfxSource = gameObject.AddComponent<AudioSource>();
            sfxSource.playOnAwake = false;
            sfxSource.loop = false;
            sfxSource.volume = sfxVolume;
        }
    }

    public void PlayMusic(AudioClip clip, bool loop = true)
    {
        if (musicSource == null)
            InitializeSources();

        if (musicSource == null)
            return;

        musicSource.clip = clip;
        musicSource.loop = loop;
        musicSource.volume = musicVolume;
        musicSource.Play();
    }

    public void StopMusic()
    {
        if (musicSource != null && musicSource.isPlaying)
            musicSource.Stop();
    }

    public void PauseMusic()
    {
        if (musicSource != null && musicSource.isPlaying)
            musicSource.Pause();
    }

    public void ResumeMusic()
    {
        if (musicSource != null && musicSource.clip != null && !musicSource.isPlaying)
            musicSource.UnPause();
    }

    public void PlaySfx(AudioClip clip, float volumeScale = 1f)
    {
        if (sfxSource == null)
            InitializeSources();

        if (sfxSource == null)
            return;

        sfxSource.PlayOneShot(clip, Mathf.Clamp01(sfxVolume * volumeScale));
    }
}
