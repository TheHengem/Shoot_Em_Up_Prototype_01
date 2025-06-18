using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    [Header("References")]
    public Transform player;               // Player's transform reference
    public GameObject bulletPrefab;        // Bullet prefab to shoot
    public Transform firePoint;            // Muzzle transform from where bullets are fired

    [Header("Movement Settings")]
    public float moveSpeed = 3f;           // Speed when moving toward or away from the player
    public float stoppingDistance = 5f;    // Desired distance from the player
    public float circleSpeed = 3f;         // Speed when circling the player

    [Header("Shooting Settings")]
    public float fireRate = 1f;            // Bullets per second when circling
    public float bulletSpeed = 10f;        // Speed at which bullet is fired

    [Header("Activation Range")]
    public float activeRange = 70f;        // The enemy is active only when within this range from the player

    // A small tolerance to decide when the enemy is at the correct distance.
    public float distanceTolerance = 0.1f;

    private float fireTimer = 0f;
    // circleDirection will be either -1 (one way) or 1 (the other) when circling.
    private int circleDirection = 1;

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

        // Only process movement and shooting if the player is within activeRange.
        if (distanceToPlayer <= activeRange)
        {
            // If the enemy is farther than the desired stopping distance (+ tolerance), approach the player.
            if (distanceToPlayer > stoppingDistance + distanceTolerance)
            {
                circleDirection = 0; // Reset circling direction
                transform.position = Vector3.MoveTowards(transform.position, player.position, moveSpeed * Time.deltaTime);
            }
            // If the enemy is too close (closer than stoppingDistance - tolerance), retreat.
            else if (distanceToPlayer < stoppingDistance - distanceTolerance)
            {
                // Calculate a desired position at exactly stoppingDistance away from the player.
                Vector3 desiredPosition = player.position - directionToPlayer * stoppingDistance;
                transform.position = Vector3.MoveTowards(transform.position, desiredPosition, moveSpeed * Time.deltaTime);
            }
            // Otherwise, if the enemy is within the acceptable range, orbit and shoot.
            else
            {
                // When in circling state, choose a random orbit direction if not already set.
                if (circleDirection == 0)
                {
                    circleDirection = Random.value < 0.5f ? -1 : 1;
                }

                // Calculate orbit (perpendicular) direction relative to the player.
                Vector3 orbitDirection = new Vector3(-directionToPlayer.y, directionToPlayer.x, 0f) * circleDirection;
                transform.position += orbitDirection * circleSpeed * Time.deltaTime;

                // Handle shooting while orbiting.
                fireTimer += Time.deltaTime;
                if (fireTimer >= 1f / fireRate)
                {
                    Shoot();
                    fireTimer = 0f;
                }
            }
        }
        // If the player is beyond activeRange, do nothing.
    }

    void Shoot()
    {
        if (bulletPrefab == null || firePoint == null)
        {
            Debug.LogWarning("Bullet Prefab or Fire Point is not assigned.");
            return;
        }

        // Instantiate the bullet at the firePoint's position and rotation.
        GameObject bullet = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);

        // Ensure the bullet has a Rigidbody2D component for movement.
        Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            // Fire the bullet in the direction the firePoint is facing (its up direction).
            rb.velocity = firePoint.up * bulletSpeed;
        }
    }
}
