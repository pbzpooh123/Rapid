using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public int maxhp = 100;
    private int curhp;
    public Transform player;  // Reference to the player
    public float moveSpeed = 2f;
    public float detectionRange = 5f;
    public float attackRange = 2f;
    public float jumpProbability = 0.1f; // Chance to jump

    private bool isMovingBack = false;
    private bool isAttacking = false;
    private bool isJumping = false;

    private Rigidbody2D rb;

    void Start()
    {
        curhp = maxhp;
        rb = GetComponent<Rigidbody2D>();
    }

    public void TakeDam(int dam)
    {
        curhp -= dam;

        if (curhp <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        GetComponent<Collider2D>().enabled = false;
        this.enabled = false;
    }

    private void Update()
    {
        if (!isAttacking)  // Prevent movement while attacking
        {
            float distanceToPlayer = Vector2.Distance(transform.position, player.position);

            // Handle AI behavior based on distance to player
            if (distanceToPlayer <= attackRange)
            {
                Attack();
            }
            else if (distanceToPlayer <= detectionRange)
            {
                if (distanceToPlayer <= attackRange)
                {
                    MoveBackwards();
                }
                else
                {
                    MoveTowardsPlayer();
                }
            }
            else
            {
                Idle();
            }

            float randomValue = UnityEngine.Random.Range(0f, 1f);

            // Occasionally jump randomly
            if (!isJumping && randomValue < jumpProbability)
            {
                Jump();
            }
        }
    }

    private void MoveTowardsPlayer()
    {
        if (!isMovingBack)
        {
            Vector2 direction = (player.position - transform.position).normalized;
            rb.velocity = new Vector2(direction.x * moveSpeed, rb.velocity.y);
        }
    }

    private void MoveBackwards()
    {
        isMovingBack = true;
        Vector2 direction = (transform.position - player.position).normalized;
        rb.velocity = new Vector2(direction.x * moveSpeed, rb.velocity.y);

        // Stop moving backward after a brief delay to prevent constant retreat
        Invoke("StopMovingBack", 1f);
    }

    private void StopMovingBack()
    {
        isMovingBack = false;
    }

    private void Attack()
    {
        isAttacking = true;
        rb.velocity = Vector2.zero;  // Stop movement during attack
        // Play attack animation and deal damage
        Debug.Log("Enemy Attacking");
        Invoke("ResetAttack", 5f); // Simulate attack cooldown
    }

    private void ResetAttack()
    {
        isAttacking = false;  // Reset attack state
    }

    private void Idle()
    {
        rb.velocity = new Vector2(0, rb.velocity.y);
    }

    private void Jump()
    {
        isJumping = true;
        rb.AddForce(Vector2.up * 5f, ForceMode2D.Impulse);
        Invoke("ResetJump", 1f);  // Prevent constant jumping
    }

    private void ResetJump()
    {
        isJumping = false;
    }
}
