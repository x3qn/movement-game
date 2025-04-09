using UnityEditor.Search;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class AudioManagerScript : MonoBehaviour
{
    [SerializeField] private Slider masterVolumeSlider;
    [SerializeField] private Slider musicVolumeSlider;
    [SerializeField] private Slider sfxVolumeSlider;
    [SerializeField] private AudioSettings audioSettings;
    [SerializeField] private AudioMixer audioMixer;
    void Start()
    {
        masterVolumeSlider.value = audioSettings.masterVolume;
        OnSliderChange(0);
        musicVolumeSlider.value = audioSettings.musicVolume;
        OnSliderChange(1);
        sfxVolumeSlider.value = audioSettings.sfxVolume;
        OnSliderChange(2);
    }

    public void OnSliderChange(int id)
    {
        float volume;

        switch(id)
        {
            case 0:
                volume = masterVolumeSlider.value;
                audioSettings.masterVolume = volume;
                audioMixer.SetFloat("MasterVolume", volume);
                break;
            case 1:
                volume = musicVolumeSlider.value;
                audioSettings.musicVolume = volume;
                audioMixer.SetFloat("MusicVolume", volume);
                break;
            case 2:
                volume = sfxVolumeSlider.value;
                audioSettings.sfxVolume = volume;
                audioMixer.SetFloat("SFXVolume", volume);
                break;
        }

    }
}