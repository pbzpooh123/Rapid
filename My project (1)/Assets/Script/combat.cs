using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class combat : MonoBehaviour
{
     public Animator anim;
    public Transform attackpoint;
    public float attackrange = 0.5f;
    public LayerMask enemylayer;

    public bool isAttacking = false;
    public bool isHAttacking = false;

    public static combat instace;

    public int baseDamage = 5; // Base damage
    public int attackCounter = 0; // Counter for consecutive attacks
    public int maxCounter = 2; // Maximum number of consecutive attacks before reset
    private float attackResetTime = 1.0f; // Time to reset the attack counter
    private float attackTimer = 0f; // Timer to reset the attack counter
    public int dam = 5;
    public int Hdam = 15;
    private float attackCooldown = 0.1f; // Cooldown time in seconds to prevent double input
    private float nextAttackTime = 0f;

    // Array of animation triggers for each combo stage
    private string[] comboTriggers = { "Attack1", "Attack2" }; // Add more if needed

    private void Awake()
    {
        instace = this;
    }

    void Update()
    {
        if (Time.time > attackTimer) // Reset attack counter if timer is expired
        {
            attackCounter = 0;
        }

        if (Input.GetMouseButtonDown(0) && !isAttacking && Time.time >= nextAttackTime)
        {
            nextAttackTime = Time.time + attackCooldown; // Set the next attack time
            StartCoroutine(Attack());
        }

        if (Input.GetMouseButtonDown(1) && !isHAttacking && Time.time >= nextAttackTime)
        {
            nextAttackTime = Time.time + attackCooldown; // Set the next attack time
            StartCoroutine(HAttack());
        }
    }

    IEnumerator Attack()
    {
        if (isAttacking) yield break; // Exit if already attacking

        isAttacking = true;

        // Determine the animation trigger based on the attack counter
        string trigger = comboTriggers[attackCounter % comboTriggers.Length];
        anim.SetTrigger(trigger);

        // Calculate damage based on attack counter
        int damage = baseDamage + (attackCounter * 2); // Increase damage with attack counter

        yield return new WaitForSeconds(0.1f); // Wait before registering the hit

        Collider2D[] hit = Physics2D.OverlapCircleAll(attackpoint.position, attackrange, enemylayer);

        foreach (Collider2D enemy in hit)
        {
            enemy.GetComponent<Enemy>().TakeDam(damage);
        }

        // Increment attack counter
        attackCounter++;
        if (attackCounter >= maxCounter) attackCounter = 0; // Reset counter if max is reached

        attackTimer = Time.time + attackResetTime; // Reset the attack timer

        yield return new WaitForSeconds(0.1f); // Adjust this to the length of your attack animation

        isAttacking = false; // Reset the attack state
    }

    IEnumerator HAttack()
    {
        isHAttacking = true;
        anim.SetTrigger("HAttack"); // Trigger heavy attack animation

        yield return new WaitForSeconds(0.2f); // Wait a short delay before registering the hit, adjust as needed

        Collider2D[] hit = Physics2D.OverlapCircleAll(attackpoint.position, attackrange, enemylayer);

        foreach (Collider2D enemy in hit)
        {
            enemy.GetComponent<Enemy>().TakeDam(Hdam);
        }

        yield return new WaitForSeconds(1.0f); // Wait for heavy attack cooldown or until animation ends

        isHAttacking = false; // Reset the heavy attack state
    }
    
    private void OnDrawGizmosSelected()
    {
        if (attackpoint == null)
        {
            return;
        }
        
        Gizmos.DrawWireSphere(attackpoint.position, attackrange);
    }
}
