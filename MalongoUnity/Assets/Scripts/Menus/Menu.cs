using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class Menu : MonoBehaviour
{
    GameManager gm;

    public const string SCENE_TO_LOAD_NAME = "Lvl";

    // Start is called before the first frame update
    void Start()
    {
        gm = GameManager.Instance;
        if (gm == null)
            Debug.LogError("GameManager null add it in the scene");

        gm.OnSceneChange += HandleSceneChange;

        gm.input.actions.FindAction("Cancel").started += OnInputQuit;
    }

    private void OnDestroy()
    {
        if (gm != null)
            gm.OnSceneChange -= HandleSceneChange;
    }

    private void HandleSceneChange()
    {
        if (gm.input != null)
        {
            gm.input.actions.FindAction("Cancel").started -= OnInputQuit;
        }
        else
        {
            Debug.LogWarning("action Not unsubscribed");
        }
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void Play()
    {
        gm.LoadScene(SCENE_TO_LOAD_NAME);
    }

    public void OnInputQuit(InputAction.CallbackContext context)
    {
        gm.Quit();
    }

    public void OnButtonQuit()
    {
        gm.Quit();
    }

}
