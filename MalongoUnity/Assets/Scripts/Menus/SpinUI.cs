using UnityEngine;

public class SpinUI : MonoBehaviour
{
    [SerializeField] float rotationSpeed = 500f;

    void Update()
    {
        transform.Rotate(0, 0, - rotationSpeed * Time.unscaledDeltaTime);
    }
}

