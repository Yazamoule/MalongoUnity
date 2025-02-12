using UnityEngine;
using FMODUnity;

public class TestForFmodOneShotSound : MonoBehaviour
{
    [field: SerializeField] public EventReference soundForOnTriggerEnter; 

    private void Start()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        RuntimeManager.PlayOneShotAttached(soundForOnTriggerEnter, gameObject);
    }

    private void OnDestroy()
    {
        
    }
}