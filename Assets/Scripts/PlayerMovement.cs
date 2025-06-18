using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed = 5f;            // Normal movement speed
    public float sprintMultiplier = 1.5f;   // 50% faster when sprinting
    public float smoothTime = 0.1f;         // Smaller values = snappier, larger = smoother

    private Rigidbody2D rb;
    private Vector2 movement;
    private Vector2 currentVelocity;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        // Get raw input from WASD or arrow keys
        movement.x = Input.GetAxisRaw("Horizontal");
        movement.y = Input.GetAxisRaw("Vertical");
        movement = movement.normalized;
    }

    void FixedUpdate()
    {
        // Determine the target speed (in case of sprinting)
        float speed = Input.GetKey(KeyCode.LeftShift) ? moveSpeed * sprintMultiplier : moveSpeed;
        Vector2 targetVelocity = movement * speed;

        // Smoothly transition to the target velocity
        Vector2 smoothVelocity = Vector2.SmoothDamp(rb.velocity, targetVelocity, ref currentVelocity, smoothTime);
        rb.velocity = smoothVelocity;
    }
}
