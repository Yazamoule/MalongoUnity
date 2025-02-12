using UnityEngine;

public struct OptionStruct
{
    //true = use The mouse to control the wingsuit
    //false = directili read input to control each axis
    public bool useMouseFlight;

    //player name used for hight Score
    public string playerName;

    public float mouseSensitivity;

    //Sound
    public float volumeMaster;
    public float volumeMusic;
    public float volumeAmbience;
    public float volumeSFX;
}
