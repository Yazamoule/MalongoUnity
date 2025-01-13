using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;

public class LeaderboardManager : MonoBehaviour
{
    private string filePath;
    private List<ScoreEntry> leaderboard;
    public List<ScoreEntry> Leaderboard { get => leaderboard; }

    [System.Serializable]
    public class ScoreEntry
    {
        public string name;
        public int score;

        public ScoreEntry(string name, int score)
        {
            this.name = name;
            this.score = score;
        }
    }

    void Awake()
    {
        GameManager.Instance.leaderboardManager = this;
        // Define the file path for the leaderboard CSV
        filePath = Path.Combine(Application.persistentDataPath, "leaderboard.csv");
        leaderboard = new List<ScoreEntry>();

        // Load leaderboard or reset to default if no file exists
        if (!File.Exists(filePath))
        {
            ResetToDefault();
        }
        else
        {
            Load();
        }
    }

    public void Save()
    {
        // Sort the leaderboard by score (descending) and take the top 10
        leaderboard = leaderboard.OrderByDescending(entry => entry.score).Take(10).ToList();

        using (StreamWriter writer = new StreamWriter(filePath))
        {
            // Write header
            writer.WriteLine("name,score");

            // Write each entry
            foreach (var entry in leaderboard)
            {
                writer.WriteLine($"{entry.name},{entry.score}");
            }
        }
    }

    public void Load()
    {
        if (!File.Exists(filePath))
        {
            Debug.LogError("Leaderboard file not found!");
            return;
        }

        leaderboard.Clear();

        using (StreamReader reader = new StreamReader(filePath))
        {
            // Skip the header line
            reader.ReadLine();

            // Read each entry
            while (!reader.EndOfStream)
            {
                string line = reader.ReadLine();
                if (string.IsNullOrEmpty(line)) continue;

                string[] parts = line.Split(',');
                if (parts.Length == 2)
                {
                    string name = parts[0];
                    int score;
                    if (int.TryParse(parts[1], out score))
                    {
                        leaderboard.Add(new ScoreEntry(name, score));
                    }
                }
            }
        }
    }

    public void ResetToDefault()
    {
        leaderboard.Clear();

        // Add 10 preset scores
        leaderboard.Add(new ScoreEntry("CANETTE", 10));
        leaderboard.Add(new ScoreEntry("CANETTE", 0));
        leaderboard.Add(new ScoreEntry("CANETTE", 0));
        leaderboard.Add(new ScoreEntry("CANETTE", 0));
        leaderboard.Add(new ScoreEntry("CANETTE", 0));
        leaderboard.Add(new ScoreEntry("CANETTE", 0));
        leaderboard.Add(new ScoreEntry("CANETTE", 0));
        leaderboard.Add(new ScoreEntry("CANETTE", 0));
        leaderboard.Add(new ScoreEntry("CANETTE", 0));
        leaderboard.Add(new ScoreEntry("CANETTE", 0));

        Save();
    }

    // Helper function to add a score
    public void AddScore(int score)
    {
        string name = GameManager.Instance.option.playerName;
        leaderboard.Add(new ScoreEntry(name, score));
        Save();
    }

    // Helper function to display the leaderboard in the console
    public void PrintLeaderboard()
    {
        foreach (var entry in leaderboard)
        {
            Debug.Log($"{entry.name}: {entry.score}");
        }
    }
}
