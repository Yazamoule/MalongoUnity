using UnityEngine;

public class TestForFmodOneShotSound : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        GameManager.Instance.aum.PlayOneShot(LevelManager.Instance.auEvents.coinCollected, this.transform.position);
    }
}
