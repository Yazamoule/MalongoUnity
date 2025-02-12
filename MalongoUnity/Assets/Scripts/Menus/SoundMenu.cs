using UnityEngine;
using UnityEngine.UI;
using FMOD.Studio;

public class SoundMenu : MonoBehaviour
{
    GameManager gm;

    [SerializeField] Slider masterVolumeSlider;
    [SerializeField] Slider musicVolumeSlider;
    [SerializeField] Slider ambienceVolumeSlider;
    [SerializeField] Slider sfxVolumeSlider;

    private void Start()
    {
        gm = GameManager.Instance;

        masterVolumeSlider.value = gm.option.volumeMaster;
        musicVolumeSlider.value = gm.option.volumeMusic;
        sfxVolumeSlider.value = gm.option.volumeSFX;
        ambienceVolumeSlider.value = gm.option.volumeAmbience;

        // Add listeners to the sliders
        masterVolumeSlider.onValueChanged.AddListener(delegate { OnVolumeSliderChanged("bus:/", masterVolumeSlider.value); });
        musicVolumeSlider.onValueChanged.AddListener(delegate { OnVolumeSliderChanged("bus:/Music", musicVolumeSlider.value); });
        musicVolumeSlider.onValueChanged.AddListener(delegate { OnVolumeSliderChanged("bus:/Ambience", ambienceVolumeSlider.value); });
        sfxVolumeSlider.onValueChanged.AddListener(delegate { OnVolumeSliderChanged("bus:/SFX", sfxVolumeSlider.value); });


    }

    // Event handler for when a volume slider is changed
    private void OnVolumeSliderChanged(string busPath, float volume)
    {
        FMOD.Studio.Bus bus = FMODUnity.RuntimeManager.GetBus(busPath);
        bus.setVolume(volume);


    }

    public void Save()
    {
        gm.option.volumeMaster = masterVolumeSlider.value;
        gm.option.volumeMusic = musicVolumeSlider.value;
        gm.option.volumeSFX = sfxVolumeSlider.value;
        gm.option.volumeAmbience = ambienceVolumeSlider.value;

        gm.saveLoad.SaveOption();
    }
}