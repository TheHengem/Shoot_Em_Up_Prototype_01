using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class PlayerHealth : MonoBehaviour
{
    [Header("Health Settings")]
    public int maxHealth = 100;      // Maximum health
    public int currentHealth;        // Current health
    public int damageAmount = 10;    // Amount of damage taken per hit

    [Header("Flash Settings")]
    private SpriteRenderer spriteRenderer;       // To change enemy's color for flash effect
    public Color flashColor = Color.white;         // Color to flash when hit
    public float flashDuration = 0.1f;           // Duration of the flash effect
    public float invulnerabilityDuration = 0.2f;  // Invulnerability time after being hit

    [Header("UI")]
    public TMP_Text healthText;      // Reference to the TextMeshPro text element
    public GameObject gameOverPanel;  // The entire game over UI panel
    public TMP_Text scoreText;        // Text to display the final score
    public Button restartButton;      // Restart button

    void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        if (spriteRenderer == null)
        {
            Debug.LogWarning("Enemy does not have a SpriteRenderer component.");
        }
    }
    void Start()
    {
        currentHealth = maxHealth;
        UpdateHealthUI();
        gameOverPanel.SetActive(false);
        restartButton.onClick.AddListener(RestartGame);
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        // Check if the collided object has the tag "enemybullet"
        if (collision.CompareTag("enemybullet"))
        {
            TakeDamage(damageAmount);
            Destroy(collision.gameObject); // Destroy the bullet upon collision
        }
    }

    // Call this method when the player takes damage.
    public void TakeDamage(int damage)
    {
        StartCoroutine(FlashRoutine());
        currentHealth -= damage;
        if (currentHealth < 0)
        {
            currentHealth = 0;
        }

        UpdateHealthUI();

        if (currentHealth == 0)
        {
            Die();
        }
    }

    // Update the TextMeshPro UI text with the current health.
    void UpdateHealthUI()
    {
        if (healthText != null)
        {
            healthText.text = "Health: " + currentHealth;
        }
    }

    // Called when health reaches zero.
    void Die()
    {
        Debug.Log("Player has died!");
        ShowGameOverScreen();
    }
    IEnumerator FlashRoutine()
    {
        Color originalColor = spriteRenderer.color;
        // Flash the player by changing its color
        spriteRenderer.color = flashColor;
        yield return new WaitForSeconds(flashDuration);
        // Revert to the original color
        spriteRenderer.color = originalColor;
        // Wait the remainder of the invulnerability period
        yield return new WaitForSeconds(invulnerabilityDuration - flashDuration);
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
        Time.timeScale = 0f;
    }
    void RestartGame()
    {
        // Reload the current scene
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
