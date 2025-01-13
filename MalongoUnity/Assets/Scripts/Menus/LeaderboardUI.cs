using TMPro;
using UnityEngine;

public class LeaderboardUI : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI rankAndNameText; 
    [SerializeField] TextMeshProUGUI scoreText;

    private Canvas canva;
    LeaderboardManager leaderboardManager;

    private void Awake()
    {

        canva = GetComponent<Canvas>();
    }

    private void Start()
    {
         leaderboardManager = GameManager.Instance.leaderboardManager;
        
    }

    public void PrintLeaderboard()
    {
        if (rankAndNameText == null || scoreText == null)
        {
            Debug.LogError("RankAndNameText or ScoreText is not assigned!");
            return;
        }

        // Clear both text fields
        rankAndNameText.text = "";
        scoreText.text = "";

        // Format and display each leaderboard entry
        int rank = 1;
        foreach (var entry in GameManager.Instance.leaderboardManager.Leaderboard)
        {
            // Add rank and name to the rankAndNameText field
            rankAndNameText.text += $"{rank}. {entry.name}\n";

            // Add formatted score (8 digits with leading zeros) to the scoreText field
            scoreText.text += $"{entry.score.ToString("D8")}\n";

            rank++;
        }
    }

    public void ResetToDefault()
    {
        leaderboardManager.ResetToDefault();
        PrintLeaderboard();
    }
}