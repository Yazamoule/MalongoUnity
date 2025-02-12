using UnityEngine;
using FMODUnity;

public class TestForFmodOneShotSound : MonoBehaviour
{

    private StudioEventEmitter emitter;

    LevelAudioEvents levelAudioEvents;

    private void Start()
    {
        levelAudioEvents = LevelManager.Instance.auEvents;

        emitter = GameManager.Instance.aum.InitializeEventEmitter(levelAudioEvents.coinIdle, this.gameObject);
        emitter.Play();
    }

    private void OnTriggerEnter(Collider other)
    {
        GameManager.Instance.aum.PlayOneShot(levelAudioEvents.coinCollected, this.transform.position);
    }

    private void OnDestroy()
    {
        emitter.Stop();
    }
}
