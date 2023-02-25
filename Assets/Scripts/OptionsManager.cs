using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;


public class OptionsManager : MonoBehaviour
{
    public AudioMixer audioMixer;
    public Slider UiSlider;
    public Slider SfxSlider;
    public Slider MusicSlider;
    private float defaultUiValue = -10f;
    private float defaultMusicValue = -20f;
    private float defaultSfxValue = -20f;

    private void Start() {
        audioMixer.SetFloat("UiVolume", PlayerPrefs.GetFloat("UiVolume"));
        audioMixer.SetFloat("SfxVolume", PlayerPrefs.GetFloat("SfxVolume"));
        audioMixer.SetFloat("MusicVolume", PlayerPrefs.GetFloat("MusicVolume"));

    }

    public void SetUiVolume(float volume){
        audioMixer.SetFloat("UiVolume", volume);
        PlayerPrefs.SetFloat("UiVolume", volume);
    }

    public void SetSfxVolume(float volume){
        audioMixer.SetFloat("SfxVolume", volume);
        PlayerPrefs.SetFloat("SfxVolume", volume);
    }

    public void SetMusicVolume(float volume){
        audioMixer.SetFloat("MusicVolume", volume);
        PlayerPrefs.SetFloat("MusicVolume", volume);
    }

    public void RefreshSliders(){
        UiSlider.value = PlayerPrefs.GetFloat("UiVolume");
        SfxSlider.value = PlayerPrefs.GetFloat("SfxVolume");
        MusicSlider.value = PlayerPrefs.GetFloat("MusicVolume");
    }

    public void ResetPrefs(){
        PlayerPrefs.DeleteAll();
        SetUiVolume(defaultUiValue);
        SetSfxVolume(defaultSfxValue);
        SetMusicVolume(defaultMusicValue);
        RefreshSliders();
    }
}
