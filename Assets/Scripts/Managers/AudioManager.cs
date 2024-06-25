using System;
using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;

    [Header("Sounds")]
    [SerializeField] Audio[] musicSounds, sfxSounds;

    [Header("AudioSources")]
    public AudioSource mainAudioSource, musicAudioSource, sfxAudiosource;

    [Header("Audio Mixer")]
    public AudioMixer audioMixer;
    public string masterVolumeParam = "MasterVolume";
    public string musicVolumeParam = "MusicVolume";
    public string sfxVolumeParam = "SFXVolume";

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        PlayMusic("Main");
    }

    public void PlayMusic(string name)
    {
        Audio audios = Array.Find(musicSounds, x => x.name == name);

        if (audios == null)
        {
            Debug.Log("Sound not found");
        }
        else
        {
            musicAudioSource.clip = audios.clip;
            musicAudioSource.Play();
        }
    }
}
