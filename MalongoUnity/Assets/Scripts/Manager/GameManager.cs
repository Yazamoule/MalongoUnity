using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;
using Unity.VisualScripting;


public class GameManager : MonoBehaviour
{
    #region eager singleton
    public static GameManager Instance { get; private set; }

    private bool InitSingleton()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this.gameObject);
            return false;
        }
        Instance = this;

        DontDestroyOnLoad(this.gameObject);

        return true;
    }
    #endregion

    public delegate void SceneChangeEventHandler();
    public SceneChangeEventHandler OnSceneChange;

    #region refs
    [HideInInspector] public PlayerInput input = null;

    [HideInInspector] public OptionStruct option;

    [HideInInspector] public LeaderboardManager leaderboardManager = null;

    [HideInInspector] public SaveLoad saveLoad = null;

    [HideInInspector] public DebugCustom debug = null;
    #endregion

    [SerializeField] GameObject inputGameObject = null;

    [SerializeField] Image blackImage;

    [SerializeField] float timeOfFadeOut = 1.5f;

    [SerializeField] bool allowHideCursor = true;



    private void Awake()
    {
        if (!InitSingleton())
            return;

        input = Instantiate<GameObject>(inputGameObject, transform).GetComponent<PlayerInput>();

        saveLoad = new SaveLoad();

        OnSceneChange += HandleSceneChange;

    }

    private void Start()
    {
        input = FindFirstObjectByType<PlayerInput>();

        saveLoad.LoadOption();
        saveLoad.LoadInput();

        FMODUnity.RuntimeManager.GetBus("bus:/").setVolume(option.volumeMaster);
        FMODUnity.RuntimeManager.GetBus("bus:/Music").setVolume(option.volumeMusic);
        FMODUnity.RuntimeManager.GetBus("bus:/Ambience").setVolume(option.volumeAmbience);
        FMODUnity.RuntimeManager.GetBus("bus:/SFX").setVolume(option.volumeSFX);
    }

    private void OnDestroy()
    {
        if (Instance != this)
            return;

        OnSceneChange -= HandleSceneChange;
    }

    private void HandleSceneChange()
    {
        FMODUnity.RuntimeManager.GetBus("bus:/Ambience").stopAllEvents(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
        FMODUnity.RuntimeManager.GetBus("bus:/SFX").stopAllEvents(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
    }

    public void StopTime(bool _stopTime)
    {
        if (_stopTime)
            Time.timeScale = 0f;
        else
            Time.timeScale = 1f;
    }

    public void SetCursorVisibility(bool _showCursor)
    {
        if (!allowHideCursor)
            return;

        Cursor.visible = _showCursor;

        if (_showCursor)
            Cursor.lockState = CursorLockMode.None;
        else
            Cursor.lockState = CursorLockMode.Locked;
    }

    public void Quit()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#elif UNITY_WEBPLAYER
        Application.OpenURL(webplayerQuitURL);
#else
        Application.Quit();
#endif
    }

    #region Scene managment
    public void LoadScene(string _name)
    {
        StartCoroutine(LoadSceneAsync(_name));
    }

    IEnumerator LoadSceneAsync(string _name)
    {
        // Load the loading screen

        AsyncOperation loadingOperation = SceneManager.LoadSceneAsync("LoadingScene");
        loadingOperation.allowSceneActivation = false;

        // Load the target scene in the background
        AsyncOperation targetSceneOperation = SceneManager.LoadSceneAsync(_name);
        //AsyncOperation targetSceneOperation = NetworkManager.Singleton.SceneManager.LoadScene(_name, LoadSceneMode.Single);
        targetSceneOperation.allowSceneActivation = false;

        //pause
        StopTime(true);

        OnSceneChange?.Invoke();

        //Load loading screan
        while (!loadingOperation.isDone)
        {

            // If the loading scene is fully loaded, activate it
            if (loadingOperation.progress >= 0.9f)
            {
                StopTime(false);

                //stop all sound / fmod event
                FMOD.Studio.Bus masterBus;
                try
                {
                    masterBus = FMODUnity.RuntimeManager.GetBus("bus:/");
                }
                catch (FMODUnity.BusNotFoundException)
                {
                    masterBus = default; // Set to an invalid bus
                }

                masterBus.stopAllEvents(FMOD.Studio.STOP_MODE.IMMEDIATE);

                loadingOperation.allowSceneActivation = true;
            }
            yield return null;

        }

        //load Target Scene
        while (!targetSceneOperation.isDone)
        {
            // Update your loading screen progress here (e.g., loading bar)
            float progress = Mathf.Clamp01(targetSceneOperation.progress / 0.9f);

            // If the target scene is fully loaded, activate it
            if (targetSceneOperation.progress >= 0.9f)
            {
                StopTime(false);
                targetSceneOperation.allowSceneActivation = true;
            }

            yield return null;
        }

        //fadeOut

        float startTime = Time.unscaledTime;

        Color color = blackImage.color;
        color = blackImage.color;

        while (timeOfFadeOut > Time.unscaledTime - startTime)
        {
            float t = (Time.unscaledTime - startTime) / timeOfFadeOut; // Normalized time [0, 1]

            float alpha = Mathf.SmoothStep(1, 0, t);

            color.a = alpha;
            blackImage.color = color;

            yield return null;
        }

        color.a = 0;
        blackImage.color = color;
    }
    #endregion

    #region Debug
    public void DebugLine(bool _enabled, string _label, Color _color, Vector3 _vector, bool _drawWorld = true, bool _drawSideView = true, bool _drawTopView = true, Vector3 ? _start = null)
    {
#if UNITY_EDITOR
        if (debug != null && _enabled)
        {
            debug.DrawRay(_label, _color, _vector, _start, _drawWorld, _drawSideView, _drawTopView);
        }
#endif
    }
    #endregion


}
