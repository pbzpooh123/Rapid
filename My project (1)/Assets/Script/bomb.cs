using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bomb : MonoBehaviour
{
    public int damage = 5; // Damage dealt by the projectile
    public float lifeTime = 1.5f; // Time before the projectile is destroyed

    private void Start()
    {
        // Destroy the projectile after a certain amount of time to prevent it from existing indefinitely
        Destroy(gameObject, lifeTime);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Check if the projectile hits the player
        if (collision.CompareTag("Player"))
        {
            Debug.Log("hit");
            // Access the player's health script and deal damage
            Playerhp playerHealth = collision.GetComponent<Playerhp>();
            if (playerHealth != null)
            {
                playerHealth.TakeDam(damage);
            }
            
            // Destroy the projectile after it hits the player
            Destroy(gameObject);
        }
    }
}
