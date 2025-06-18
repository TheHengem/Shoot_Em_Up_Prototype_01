using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class GameOverManager : MonoBehaviour
{
    [Header("UI Elements")]
    public GameObject gameOverPanel;  // The entire game over UI panel
    public TMP_Text scoreText;        // Text to display the final score
    public Button restartButton;      // Restart button

    private void Start()
    {
        // Ensure the panel is hidden at the start
        gameOverPanel.SetActive(false);

        // Assign restart function to the button
        restartButton.onClick.AddListener(RestartGame);
    }

    public void ShowGameOverScreen()
    {
        gameOverPanel.SetActive(true); // Show the panel

        // Fetch and display the final score from ScoreManager
        if (ScoreTracker.Instance != null)
        {
            scoreText.text = "Your Score: " + ScoreTracker.Instance.score;
        }
        else
        {
            scoreText.text = "Your Score: 0";
        }
    }

    void RestartGame()
    {
        // Reload the current scene
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
