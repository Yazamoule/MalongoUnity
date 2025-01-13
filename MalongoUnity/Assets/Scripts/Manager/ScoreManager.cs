using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    GameManager gm;
    LevelManager lm;

    public delegate void ScoreChangeEventHandler(float _score);
    public ScoreChangeEventHandler OnScoreChange;

    public delegate void WinEventHandler();
    public WinEventHandler OnWin;

    bool gameOver = false;

    private float score = 0;
    public float Score { get => score; }

    [SerializeField] float winingScore = 100f;

    private void Awake()
    {
        gm = GameManager.Instance;
        lm = LevelManager.Instance;

        lm.scoreManager = this;
    }

    private void Start()
    {
        OnScoreChange += WinCondition;
        OnWin += SaveScore;
    }

    private void Update()
    {
        AddScore(10 * Time.deltaTime);
    }

    private void WinCondition(float _score)
    {
        if (!gameOver && score >= winingScore)
        {
            OnWin?.Invoke();
            gameOver = true;
        }
    }

    private void LoseCondition()
    {
    }

    public void AddScore(float _scoreToAdd, bool _callOnScoreChange = true)
    {
        score += _scoreToAdd;

        if (_callOnScoreChange)
            OnScoreChange?.Invoke(Score);
    }

    public void SetScore(float _score)
    {
        score = _score;
        OnScoreChange?.Invoke(Score);
    }

    public void ResetScore()
    {
        score = 0;
        OnScoreChange?.Invoke(Score);
        gameOver = false;
    }

    public void SaveScore()
    {
        gm.leaderboardManager.AddScore((int)Mathf.Ceil(score));
    }
}