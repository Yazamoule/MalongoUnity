using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;

public class LevelAudioEvents : MonoBehaviour
{


    [field: Header("Player SFX")]
    [field: SerializeField] public EventReference playerFootsteps { get; private set; }

    [field: Header("Coin SFX")]
    [field: SerializeField] public EventReference coinCollected { get; private set; }
    [field: SerializeField] public EventReference coinIdle { get; private set; }

    private void Awake()
    {
        LevelManager.Instance.auEvents = this;
    }
}
