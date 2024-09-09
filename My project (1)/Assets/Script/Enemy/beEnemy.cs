using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class beEnemy : MonoBehaviour
{
    public int maxhp = 100;
    private int curhp;
    public Transform player;  // Reference to the player
    public float moveSpeed = 2f;
    public float detectionRange = 5f;
    public float attackRange = 2f;
    private bool isMovingBack = false;
    private bool isAttacking = false;
    private Rigidbody2D rb;
    public Playerhp php;
    public int dam = 5;
    [SerializeField] private floatingbar heathbar;
    public GameObject projectilePrefab;
    public float projectileSpeed = 10f;
    public float attackCooldown = 5f;
    public Transform projectileSpawnPoint; 

    private void Awake()
    {
        curhp = maxhp;
        rb = GetComponent<Rigidbody2D>();
        heathbar = GetComponentInChildren<floatingbar>();
    }

    void Start()
    {
      
        heathbar.UpdateHeathbar(curhp,maxhp);
       
    }

    public void TakeDam(int dam)
    {
        curhp -= dam;
        heathbar.UpdateHeathbar(curhp,maxhp);

        if (curhp <= 0)
        {
            Die();
            LaunchDrvoe();
        }
    }

    void Die()
    {
        GetComponent<Collider2D>().enabled = false;
        this.enabled = false;
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
    
    private void LaunchDrvoe()
    {
        // Instantiate the projectile at the spawn point above the head
        GameObject projectile = Instantiate(projectilePrefab, projectileSpawnPoint.position, Quaternion.identity);

        // Calculate direction towards the player
        Vector2 direction = (player.position - transform.position).normalized;

        // Set projectile velocity
        Rigidbody2D projectileRb = projectile.GetComponent<Rigidbody2D>();
        projectileRb.velocity = direction * projectileSpeed;
    }

   
}
