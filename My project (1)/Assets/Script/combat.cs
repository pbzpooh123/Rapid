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

    public static combat instance;

    public int baseDamage = 5; // Base damage
    public int attackCounter = 0; // Counter for consecutive attacks
    public int maxCounter = 2; // Maximum number of consecutive attacks before reset
    private float attackResetTime = 1.0f; // Time to reset the attack counter
    private float lastAttackTime = 0f; // Time of the last attack input
    private float attackCooldown = 0.1f; // Cooldown time in seconds to prevent double input
    private float nextAttackTime = 0f;

    // Array of animation triggers for each combo stage
    private string[] comboTriggers = { "Attack1", "Attack2" }; // Add more if needed

    private void Awake()
    {
        instance = this;
    }

    void Update()
    {
        HandleComboInput();
        CheckComboTimeout();
    }

    private void HandleComboInput()
    {
        // Handle light attack input
        if (Input.GetMouseButtonDown(0) && !isAttacking && Time.time >= nextAttackTime)
        {
            nextAttackTime = Time.time + attackCooldown; // Set the next allowed attack time
            lastAttackTime = Time.time; // Update the last attack time
            StartCoroutine(Attack());
        }

        // Handle heavy attack input
        if (Input.GetMouseButtonDown(1) && !isHAttacking && Time.time >= nextAttackTime)
        {
            nextAttackTime = Time.time + attackCooldown; // Set the next allowed attack time
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
        int damage = baseDamage + (attackCounter * 2);
        Debug.Log("Light Attack Damage: " + damage); // Log damage amount
        // Increase damage with attack counter

        yield return new WaitForSeconds(0.1f); // Wait before registering the hit

        Collider2D[] hit = Physics2D.OverlapCircleAll(attackpoint.position, attackrange, enemylayer);

        foreach (Collider2D enemy in hit)
        {
            enemy.GetComponent<Enemy>().TakeDam(damage);
        }

        // Increment attack counter
        attackCounter++;
        if (attackCounter >= maxCounter) attackCounter = 0; // Reset counter if max is reached

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
            enemy.GetComponent<Enemy>().TakeDam(baseDamage);
        }

        yield return new WaitForSeconds(1.0f); // Wait for heavy attack cooldown or until animation ends

        isHAttacking = false; // Reset the heavy attack state
    }

    private void CheckComboTimeout()
    {
        // Only reset the combo if enough time has passed since the last attack
        if (Time.time - lastAttackTime > attackResetTime && attackCounter > 0)
        {
            ResetCombo();
        }
    }

    public void InterruptCombo()
    {
        ResetCombo();
    }

    private void ResetCombo()
    {
        isAttacking = false;
        attackCounter = 0;
        Debug.Log("Combo Interrupted or Timed Out");
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
