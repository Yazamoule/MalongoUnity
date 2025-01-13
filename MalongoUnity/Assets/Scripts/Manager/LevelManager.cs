using UnityEngine;
using UnityEngine.InputSystem;

public class LevelManager : MonoBehaviour
{
    #region eager singleton
    public static LevelManager Instance { get; private set; }
    private bool InitSingleton()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this.gameObject);
            return false;
        }
        Instance = this;
        return true;
    }
    #endregion

    #region refs
    [HideInInspector] public ScoreManager scoreManager = null;

    [HideInInspector] public EndGameScreen endGameScreen = null;

    [HideInInspector] public Hud hud = null;

    [HideInInspector] public Pause pause = null;
    #endregion


    GameManager gm = null;

    private void Awake()
    {
        if (!InitSingleton())
            return;

        gm = GameManager.Instance;

        gm.OnSceneChange += HandleSceneChange;
    }
    void Start()
    {
        gm.SetCursorVisibility(false);
        gm.input.SwitchCurrentActionMap("Player");
    }

    private void OnDestroy()
    {
        gm.OnSceneChange -= HandleSceneChange;

    }

    private void HandleSceneChange()
    {

    }


}
