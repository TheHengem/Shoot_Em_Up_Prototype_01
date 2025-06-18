using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    public int maxHits = 3;                     // Total hits enemy can take before dying
    private int hitCount = 0;                   // Current hit count
    public float invulnerabilityDuration = 0.2f;  // Invulnerability time after being hit
    private bool isInvulnerable = false;         // Flag to check if enemy is currently invulnerable

    private SpriteRenderer spriteRenderer;       // To change enemy's color for flash effect
    public Color flashColor = Color.white;         // Color to flash when hit
    public float flashDuration = 0.1f;           // Duration of the flash effect

    void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        if (spriteRenderer == null)
        {
            Debug.LogWarning("Enemy does not have a SpriteRenderer component.");
        }
    }

    // Detect collision with bullets
    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("bullet"))
        {
            if (!isInvulnerable)
            {
                TakeHit();
            }
        }
    }

    // Process a hit on the enemy
    void TakeHit()
    {
        hitCount++;
        StartCoroutine(FlashRoutine());

        if (hitCount >= maxHits)
        {
            // Enemy dies after maxHits
            ScoreTracker.Instance.AddScore(10);
            Destroy(gameObject);
        }
    }

    // Coroutine that handles flashing and temporary invulnerability
    IEnumerator FlashRoutine()
    {
        isInvulnerable = true;
        Color originalColor = spriteRenderer.color;
        // Flash the enemy by changing its color
        spriteRenderer.color = flashColor;
        yield return new WaitForSeconds(flashDuration);
        // Revert to the original color
        spriteRenderer.color = originalColor;
        // Wait the remainder of the invulnerability period
        yield return new WaitForSeconds(invulnerabilityDuration - flashDuration);
        isInvulnerable = false;
    }
}
