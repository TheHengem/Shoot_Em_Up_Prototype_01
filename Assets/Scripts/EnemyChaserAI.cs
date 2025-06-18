using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyChaserAI : MonoBehaviour
{
    [Header("References")]
    public Transform player;               // Player's transform reference

    [Header("Movement Settings")]
    public float moveSpeed = 3f;           // Speed when approaching the player
    public float stoppingDistance = 0f;    // Distance at which enemy stops approaching and starts circling
    public float circleSpeed = 3f;         // Speed when circling the player

    // Distance threshold: enemy moves only if player is within activeRange.
    public float activeRange = 70f;

    // circleDirection will be either -1 (one way) or 1 (the other) when circling.
    private int circleDirection = 0;

    void Update()
    {
        if (player == null)
            return;

        // Always rotate to face the player.
        Vector3 directionToPlayer = (player.position - transform.position).normalized;
        float angle = Mathf.Atan2(directionToPlayer.y, directionToPlayer.x) * Mathf.Rad2Deg;
        // Rotate so that the Y-axis (up) faces the player.
        transform.rotation = Quaternion.Euler(0, 0, angle - 90f);

        // Calculate the distance from the enemy to the player.
        float distanceToPlayer = Vector3.Distance(transform.position, player.position);

        // Only process movement and shooting if the player is within activeRange (e.g., 70f).
        if (distanceToPlayer <= activeRange)
        {
            if (distanceToPlayer > stoppingDistance)
            {
                // Reset circle direction when not circling.
                circleDirection = 0;
                transform.position = Vector3.MoveTowards(transform.position, player.position, moveSpeed * Time.deltaTime);
            }
            else
            {
                // When in circling range, choose a random orbit direction if not already set.
                if (circleDirection == 0)
                {
                    circleDirection = Random.value < 0.5f ? -1 : 1;
                }

                // Calculate orbit (perpendicular) direction relative to the player.
                Vector3 orbitDirection = new Vector3(-directionToPlayer.y, directionToPlayer.x, 0f) * circleDirection;
                transform.position += orbitDirection * circleSpeed * Time.deltaTime;
            }
        }
        // If the player is beyond activeRange (e.g., 100f away), the enemy will not move or shoot.
    }
}
