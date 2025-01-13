using UnityEngine;
using UnityEngine.UI;

public class MenuButton : MonoBehaviour
{

    [SerializeField] Canvas destinationCanva;
    Canvas parentCanva;

    Button button;
    
    void Start()
    {
        parentCanva = GetComponentInParent<Canvas>();
        button = GetComponent<Button>();

        button.onClick.AddListener(delegate { ChangeActiveCanvas(); });
    }


    private void ChangeActiveCanvas()
    {
        if (parentCanva == null || destinationCanva == null)
            return;

        parentCanva.enabled = false;
        destinationCanva.enabled = true;
    }
}
