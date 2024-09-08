using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class rangedenemy : MonoBehaviour
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
    public int dam = 5;
    [SerializeField] private floatingbar heathbar;

    // Reference to the projectile prefab
    public GameObject projectilePrefab;
    public float projectileSpeed = 10f;
    public float attackCooldown = 5f;

    // Reference to the projectile spawn point (above the head)
    public Transform projectileSpawnPoint; 

    private void Awake()
    {
        curhp = maxhp;
        rb = GetComponent<Rigidbody2D>();
        heathbar = GetComponentInChildren<floatingbar>();
    }

    void Start()
    {
        heathbar.UpdateHeathbar(curhp, maxhp);
    }

    public void TakeDam(int dam)
    {
        curhp -= dam;
        heathbar.UpdateHeathbar(curhp, maxhp);

        if (curhp <= 0)
        {
            Die();
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
        if (Input.GetKeyDown(KeyCode.G)) // For testing damage
        {
            TakeDam(20);
        }

        if (!isAttacking) // Prevent movement while attacking
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
        rb.velocity = Vector2.zero; // Stop movement during attack
        LaunchProjectile();
        Invoke("ResetAttack", attackCooldown); // Simulate attack cooldown
    }

    private void LaunchProjectile()
    {
        // Instantiate the projectile at the spawn point above the head
        GameObject projectile = Instantiate(projectilePrefab, projectileSpawnPoint.position, Quaternion.identity);

        // Calculate direction towards the player
        Vector2 direction = (player.position - transform.position).normalized;

        // Set projectile velocity
        Rigidbody2D projectileRb = projectile.GetComponent<Rigidbody2D>();
        projectileRb.velocity = direction * projectileSpeed;
    }

    private void ResetAttack()
    {
        isAttacking = false; // Reset attack state
    }

    private void Idle()
    {
        rb.velocity = new Vector2(0, rb.velocity.y);
    }

    private void Jump()
    {
        isJumping = true;
        rb.AddForce(Vector2.up * 5f, ForceMode2D.Impulse);
        Invoke("ResetJump", 1f); // Prevent constant jumping
    }

    private void ResetJump()
    {
        isJumping = false;
    }
}
