using TMPro;
using UnityEngine;

public class EndGameScreen : MonoBehaviour
{
    GameManager gm;
    LevelManager lm;

    [SerializeField] Canvas victoryCanvas = null;

    [SerializeField] TextMeshProUGUI scoreText = null;

    private void Awake()
    {
        gm = GameManager.Instance;
        lm = LevelManager.Instance;

        lm.endGameScreen = this;
    }

    private void Start()
    {
        lm.scoreManager.OnWin += OnWin;
    }

    private void OnWin()
    {
        ToggleVictoryScrean(true, lm.scoreManager.Score);
    }

    private void ToggleVictoryScrean(bool _toggle, float _score = 0)
    {
        if (_toggle)
        {
            gm.StopTime(true);
            gm.SetCursorVisibility(true);
            gm.input.SwitchCurrentActionMap("UI");

            scoreText.text = "Your Score is " + Mathf.Ceil(_score).ToString() + " wow";
            victoryCanvas.enabled = true;
        }
        else
        {
            gm.StopTime(false);
            gm.SetCursorVisibility(false);
            gm.input.SwitchCurrentActionMap("Player");

            victoryCanvas.enabled = false;
        }
    }

    public void ReturnToMenu()
    {
        gm.LoadScene("Menu");
    }

    public void Retry()
    {
        gm.LoadScene("Lvl");
    }

    public void Continue()
    {
        ToggleVictoryScrean(false);
    }


}
