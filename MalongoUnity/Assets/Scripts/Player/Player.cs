using UnityEngine;

public class Player : MonoBehaviour
{
    GameManager gm;
    LevelManager lm;

    [HideInInspector] public Movement move = null;
    

    private void Awake()
    {
        gm = GameManager.Instance;
        lm = LevelManager.Instance;
        lm.player = this;
    }

    private void Start()
    {
        move = GetComponent<Movement>();
    }


    private void Update()
    {

    }

}
