using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class Enemy : MonoBehaviour
{
    public int maxhp = 100;
    private int curhp;
    public Transform player;  // Reference to the player
    public float moveSpeed = 2f;
    public float detectionRange = 5f;
    public float attackRange = 2f;
    private bool isMovingBack = false;
    private bool isAttacking = false;
    public Image healthBar;

    public Victory win;

    private Rigidbody2D rb;
    public Playerhp php;
    public int dam = 5;
    

    void Start()
    {
        curhp = maxhp;
        rb = GetComponent<Rigidbody2D>();
    }

    public void TakeDam(int dam)
    {
        curhp -= dam;
        healthBar.fillAmount = curhp / 300f;

        if (curhp <= 0)
        {
            Die();
            
        }
    }

    void Die()
    {
        GetComponent<Collider2D>().enabled = false;
        this.enabled = false;
        win.Credits();
        Destroy(gameObject);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.G))            //---- For Check HpBar na kub -----// --- Alikato
        {
            TakeDam(20);
        }

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
        php.TakeDam(dam);
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
    
   

   
}
