using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VolumeSlider : MonoBehaviour
{
    private enum VolumeType {
        MASTER,
        MUSIC,
        AMBIENCE,
        SFX
    }

    [Header("Type")]
    [SerializeField] private VolumeType volumeType;

    AudioManager aum;

    private Slider volumeSlider;

    private void Awake()
    {
        volumeSlider = this.GetComponentInChildren<Slider>();
    }

    private void Start()
    {
        aum = GameManager.Instance.aum;
    }

    private void Update()
    {
        switch (volumeType)
        {
            case VolumeType.MASTER:
                volumeSlider.value = aum.masterVolume;
                break;
            case VolumeType.MUSIC:
                volumeSlider.value = aum.musicVolume;
                break;
            case VolumeType.AMBIENCE:
                volumeSlider.value = aum.ambienceVolume;
                break;
            case VolumeType.SFX:
                volumeSlider.value = aum.SFXVolume;
                break;
            default:
                Debug.LogWarning("Volume Type not supported: " + volumeType);
                break;
        }
    }

    public void OnSliderValueChanged()
    {
        switch (volumeType)
        {
            case VolumeType.MASTER:
                aum.masterVolume = volumeSlider.value;
                break;
            case VolumeType.MUSIC:
                aum.musicVolume = volumeSlider.value;
                break;
            case VolumeType.AMBIENCE:
                aum.ambienceVolume = volumeSlider.value;
                break;
            case VolumeType.SFX:
                aum.SFXVolume = volumeSlider.value;
                break;
            default:
                Debug.LogWarning("Volume Type not supported: " + volumeType);
                break;
        }
    }
}
