using UnityEngine;
using UnityEngine.InputSystem;

public class Pause : MonoBehaviour
{
    GameManager gm;
    LevelManager lm;

    public delegate void PauseToggleEventHandler(bool _toggle);
    public PauseToggleEventHandler OnPause;

    Canvas canvas;

    const string MENU_SCENE_NAME = "Menu";

    private void Awake()
    {
        gm = GameManager.Instance;
        lm = LevelManager.Instance;

        lm.pause = this;
    }

    void Start()
    {
        canvas = GetComponent<Canvas>();

        InputActionAsset inputActionAsset = gm.input.actions;
        inputActionAsset.FindAction("Pause").started += OnInputPause;
        inputActionAsset.FindAction("Cancel").started += OnInputResume;

        gm.OnSceneChange += HandleSceneChange;
    }

    private void OnDestroy()
    {
        gm.OnSceneChange -= HandleSceneChange;
    }

    private void HandleSceneChange()
    {
        if (gm.input != null)
        {
            InputActionAsset inputActionAsset = gm.input.actions;
            inputActionAsset.FindAction("Pause").started -= OnInputPause;
            inputActionAsset.FindAction("Cancel").started -= OnInputResume;
        }
        else
        {
            Debug.LogWarning("action Not unsubscribed");
        }
    }

    public void DoPause()
    {
        gm.SetCursorVisibility(true);
        gm.StopTime(true);

        if (gm.input != null)
        {
            //Clear the input buffer by enabeling end disabeling the playerInput
            gm.input.enabled = false;
            gm.input.enabled = true;
            gm.input.SwitchCurrentActionMap("UI");
        }

        canvas.enabled = true;

        OnPause?.Invoke(true);
    }

    public void DoResume()
    {
        gm.SetCursorVisibility(false);
        gm.StopTime(false);

        if (gm.input != null)
            gm.input.SwitchCurrentActionMap("Player");

        canvas.enabled = false;

        OnPause?.Invoke(false);
    }



    public void QuitToMenu()
    {
        gm.StopTime(false);

        if (gm.input != null)
            gm.input.SwitchCurrentActionMap("UI");

        gm.LoadScene(MENU_SCENE_NAME);
    }

    public void Quit()
    {
        gm.Quit();
    }

    public void OnInputPause(InputAction.CallbackContext context)
    {
        DoPause();
    }

    public void OnInputResume(InputAction.CallbackContext context)
    {
        if (canvas.enabled)
            DoResume();
    }

}