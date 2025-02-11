using UnityEngine;
using FMODUnity;

public class GlobalAudioEvents : MonoBehaviour
{

    [field: Header("Ambience")]
    [field: SerializeField] public EventReference ambience { get; private set; }

    [field: Header("Music")]
    [field: SerializeField] public EventReference music { get; private set; }

    private void Awake()
    {
        GameManager.Instance.auEvents = this;
    }
}
