using TMPro;
using UnityEngine;

public class Hud : MonoBehaviour
{
    GameManager gm;
    LevelManager lm;

    Canvas hubCanvas;

    [SerializeField] private TextMeshProUGUI scoreText = null;


    private void Awake()
    {
        gm = GameManager.Instance;
        lm = LevelManager.Instance;

        lm.hud = this;

        hubCanvas = GetComponent<Canvas>();
    }
    private void Start()
    {
        lm.scoreManager.OnScoreChange += UpdateScore;
        lm.scoreManager.OnWin += OnWin;

        lm.pause.OnPause += OnPause;
    }

    private void OnWin()
    {
        hubCanvas.enabled = false;
    }

    private void OnPause(bool _pause)
    {
        if (_pause)
            hubCanvas.enabled = false;
        else
            hubCanvas.enabled = true;
    }

    private void UpdateScore(float _score)
    {
        scoreText.text = "score: " + Mathf.Ceil(_score).ToString();
    }
}
