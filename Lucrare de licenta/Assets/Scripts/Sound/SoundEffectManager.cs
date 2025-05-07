using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SoundEffectManager : MonoBehaviour
{
    private static SoundEffectManager Instance;

    private AudioSource audioSource;
    private SoundEffectLibrary soundEffectLibrary;
    [SerializeField] private Slider sfxSlider;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            audioSource = GetComponent<AudioSource>();
            soundEffectLibrary = GetComponent<SoundEffectLibrary>();
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public static void Play(string soundName)
    {
        AudioClip audioClip = Instance.soundEffectLibrary.GetRandomClip(soundName);
        if(audioClip != null)
        {
            Instance.audioSource.PlayOneShot(audioClip);
        }
    }

    void Start()
    {
        float savedVolume = PlayerPrefs.GetFloat("SFXVolume", 0.5f);
        audioSource.volume = savedVolume;

        if (sfxSlider != null)
        {
            sfxSlider.value = savedVolume;
            sfxSlider.onValueChanged.AddListener(delegate { OnValueChanged(); });
        }
    }

    public static void SetVolume(float volume)
    {
        Instance.audioSource.volume = volume;
        PlayerPrefs.SetFloat("SFXVolume", volume);
        PlayerPrefs.Save();
    }

    public void OnValueChanged()
    {
        SetVolume(sfxSlider.value);
    }
}
