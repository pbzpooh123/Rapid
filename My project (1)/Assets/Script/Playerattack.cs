using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Playerattack : MonoBehaviour
{
    public Animator anim;
    public Transform attackpoint;
    public float attackrange = 0.5f;
    public LayerMask enemylayer;

    public bool isAttacking = false;
    public bool isHAttacking = false;

    public static Playerattack instace;

    public int baseDamage = 5;
    public int comboStage = 0;
    public int maxCombo = 2;
    private float comboResetTime = 1.0f;
    private float comboTimer = 0f;
    public int dam = 5;
    public int Hdam = 15;
    private float attackCooldown = 0.5f; // Reduced cooldown to allow for chaining
    private float nextAttackTime = 0f;

    private string[] comboTriggers = { "Attack1", "Attack2" };

    private void Awake()
    {
        instace = this;
    }

    void Update()
    {
        if (Time.time > comboTimer) 
        {
            comboStage = 0; // Reset combo when timer expires
        }

        if (Time.time >= nextAttackTime && Input.GetMouseButtonDown(0) && !isAttacking)
        {
            // Allow chaining combos within a specific window
            if (comboStage == 0 || Time.time <= comboTimer)
            {
                nextAttackTime = Time.time + attackCooldown;
                StartCoroutine(Attack());
            }
        }

        if (Time.time >= nextAttackTime && Input.GetMouseButtonDown(1) && !isHAttacking)
        {
            nextAttackTime = Time.time + attackCooldown;
            StartCoroutine(HAttack());
        }
    }

    IEnumerator Attack()
    {
        if (isAttacking) yield break;

        isAttacking = true;

        string trigger = comboTriggers[comboStage % comboTriggers.Length];
        anim.SetTrigger(trigger);

        int damage = baseDamage + (comboStage * 2); // Scale damage by combo stage

        yield return new WaitForSeconds(0.1f); // Wait before registering the hit

        Collider2D[] hit = Physics2D.OverlapCircleAll(attackpoint.position, attackrange, enemylayer);

        foreach (Collider2D enemy in hit)
        {
            enemy.GetComponent<Enemy>().TakeDam(damage);
        }

        comboStage++;
        if (comboStage >= maxCombo) comboStage = 0;

        // Set combo reset timer to allow chaining within the window
        comboTimer = Time.time + comboResetTime;

        yield return new WaitForSeconds(attackCooldown); // Ensure the cooldown matches your chaining expectations

        isAttacking = false;
    }

    IEnumerator HAttack()
    {
        isHAttacking = true;
        anim.SetTrigger("HAttack");
        Collider2D[] hit = Physics2D.OverlapCircleAll(attackpoint.position, attackrange, enemylayer);

        foreach (Collider2D enemy in hit)
        {
            enemy.GetComponent<Enemy>().TakeDam(Hdam);
        }

        yield return new WaitForSeconds(1.0f); 

        isHAttacking = false;
    }

    private void OnDrawGizmosSelected()
    {
        if (attackpoint == null) return;

        Gizmos.DrawWireSphere(attackpoint.position, attackrange);
    }
}
