using UnityEngine;
using UnityEngine.UI;

public class VolumeController : MonoBehaviour
{
    [Header("UI Reference")]
    public Slider volumeSlider;

    [Header("Music Reference")]
    public AudioSource bgMusicSource;

    void Start()
    {
        // 1. Load the last saved volume (Default to 0.5 so it's not silent)
        float savedVolume = PlayerPrefs.GetFloat("MusicVolume", 0.5f);

        if (volumeSlider != null)
        {
            volumeSlider.value = savedVolume;
            // This links the slider to the code automatically
            volumeSlider.onValueChanged.AddListener(SetVolume);
        }

        // 2. FORCE THE SYNC IMMEDIATELY
        SetVolume(savedVolume);
    }

    public void SetVolume(float value)
    {
        // Find every AudioSource in the current scene
        AudioSource[] allSources = Object.FindObjectsByType<AudioSource>(FindObjectsSortMode.None);

        foreach (AudioSource source in allSources)
        {
            // Only affect objects with "Music", "BG", or "yes_D" in their name
            if (source.gameObject.name.Contains("Music") ||
                source.gameObject.name.Contains("BG") ||
                source.gameObject.name.Contains("yes_D"))
            {
                source.volume = value;

                // HARD MUTE at zero (Physical "Off" switch)
                if (value <= 0.01f)
                    source.mute = true;
                else
                    source.mute = false;
            }
        }

        // Save the setting
        PlayerPrefs.SetFloat("MusicVolume", value);
        PlayerPrefs.Save();
    }
}
