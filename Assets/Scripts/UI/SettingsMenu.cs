using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SettingsMenuu : MonoBehaviour
{
    public Scrollbar volumeScrollbar;
    public Scrollbar brightnessScrollbar;

    void Start()
    {
        // Add listeners to handle slider value changes
        volumeScrollbar.onValueChanged.AddListener(SetVolume);
        brightnessScrollbar.onValueChanged.AddListener(SetBrightness);

        // Optionally, load and set saved values for sliders
        volumeScrollbar.value = PlayerPrefs.GetFloat("Volume", 0.5f);
        brightnessScrollbar.value = PlayerPrefs.GetFloat("Brightness", 0.5f);
    }

    public void SetVolume(float volume)
    {
        // Here you can set the game volume using the volume parameter
        // Example: AudioListener.volume = volume;
        AudioListener.volume = volume;
        PlayerPrefs.SetFloat("Volume", volume);
    }

    public void SetBrightness(float brightness)
    {
        // Here you can set the game brightness using the brightness parameter
        // This example assumes you have a method to set brightness
        // Example: RenderSettings.ambientLight = Color.white * brightness;
        RenderSettings.ambientLight = Color.white * brightness;
        PlayerPrefs.SetFloat("Brightness", brightness);
    }

    
}
