using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Combat : MonoBehaviour
{
    public float dashSpeed = 20f;        // Speed of the dash
    public float dashDuration = 0.2f;    // How long the dash lasts
    public float dashCooldown = 1f;      // Time before another dash can be used
    public int dashDamage = 20;          // Damage dealt to enemies on collision

    private Rigidbody2D rb;              // Reference to the Rigidbody2D
    private bool isDashing;              // Check if the player is dashing
    private float dashTime;              // Tracks dash time
    private float dashCooldownTimer;     // Tracks cooldown time

    void Start()
    {
        // Get the Rigidbody2D component
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        // Dash input (left mouse button)
        if (Input.GetMouseButtonDown(0) && !isDashing && dashCooldownTimer <= 0)
        {
            Debug.Log("Dash started");
            StartDash();
        }

        // Handle dashing state
        if (isDashing)
        {
            dashTime -= Time.deltaTime;

            if (dashTime <= 0)
            {
                StopDash();
            }
        }
        else
        {
            // Reduce cooldown timer
            if (dashCooldownTimer > 0)
                dashCooldownTimer -= Time.deltaTime;
        }
    }

    void StartDash()
    {
        isDashing = true;
        dashTime = dashDuration;
        dashCooldownTimer = dashCooldown;

        // Apply dash velocity in the forward direction of the player
        Vector2 dashDirection = transform.right * transform.localScale.x; // Use transform.right for 2D right direction
        rb.velocity = dashDirection * dashSpeed;
    }

    void StopDash()
    {
        isDashing = false;
        rb.velocity = Vector2.zero; // Reset the velocity to stop immediately
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (isDashing)
        {
            // Check if the collision is with an enemy
            if (collision.gameObject.CompareTag("Enemy"))
            {
                // Access the enemy's health component and deal damage
                Enemy enemyHealth = collision.gameObject.GetComponent<Enemy>();
                if (enemyHealth != null)
                {
                    enemyHealth.TakeDam(dashDamage); // Fixed the method name to 'TakeDamage'
                }
            }
        }
    }
}
