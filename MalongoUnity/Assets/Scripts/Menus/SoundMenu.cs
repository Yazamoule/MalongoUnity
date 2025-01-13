using UnityEngine;
using UnityEngine.UI;
using FMOD.Studio;

public class SoundMenu : MonoBehaviour
{
    public Slider masterVolumeSlider;
    public Slider musicVolumeSlider;
    public Slider sfxVolumeSlider;

    private void Start()
    {
        // Set initial volumes (assuming volumes are normalized to a range of 0 to 1)
        SetBusVolume("bus:/", masterVolumeSlider.value);
        SetBusVolume("bus:/Music", musicVolumeSlider.value);
        SetBusVolume("bus:/SFX", sfxVolumeSlider.value);

        // Add listeners to the sliders
        masterVolumeSlider.onValueChanged.AddListener(delegate { OnVolumeSliderChanged("bus:/", masterVolumeSlider.value); });
        musicVolumeSlider.onValueChanged.AddListener(delegate { OnVolumeSliderChanged("bus:/Music", musicVolumeSlider.value); });
        sfxVolumeSlider.onValueChanged.AddListener(delegate { OnVolumeSliderChanged("bus:/SFX", sfxVolumeSlider.value); });
    }

    // Function to set the volume of an FMOD bus
    private void SetBusVolume(string busPath, float volume)
    {
        FMOD.Studio.Bus bus = FMODUnity.RuntimeManager.GetBus(busPath);
        bus.setVolume(volume);
    }

    // Event handler for when a volume slider is changed
    private void OnVolumeSliderChanged(string busPath, float volume)
    {
        SetBusVolume(busPath, volume);
    }
}