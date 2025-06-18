using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ScoreTracker : MonoBehaviour
{
    public static ScoreTracker Instance; // Singleton instance for easy access

    [Header("Score Settings")]
    public int score = 0; // Player's current score
    public TMP_Text scoreText; // Reference to the TextMeshPro UI text element
    public float scoreIncreaseRate = 1f; // How often score increases (seconds)

    private void Awake()
    {
        // Ensure only one instance of ScoreManager exists
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }
    }

    private void Start()
    {
        UpdateScoreUI();
        InvokeRepeating(nameof(IncreaseScoreOverTime), 1f, scoreIncreaseRate);
    }

    // Increases score passively over time
    void IncreaseScoreOverTime()
    {
        score += 1;
        UpdateScoreUI();
    }

    // Call this function from enemy scripts when an enemy is destroyed
    public void AddScore(int amount)
    {
        score += amount;
        UpdateScoreUI();
    }

    // Updates the UI text
    void UpdateScoreUI()
    {
        if (scoreText != null)
        {
            scoreText.text = "Score: " + score;
        }
    }
}
