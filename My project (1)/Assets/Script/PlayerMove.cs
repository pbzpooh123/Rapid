using System.Collections;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    [SerializeField] private float speed;
    private Rigidbody2D body;
    private BoxCollider2D boxCollider;
    private bool canDash = true;
    private bool isDashing;
    private float dashPower = 20f;
    private int dashDamage = 25;
    private float dashTime = 0.2f;
    private float dashCooldown = 1.25f;
    public Transform attackpos;
    public float attackrange;
    public LayerMask enemy;

    private void Awake()
    {
        body = GetComponent<Rigidbody2D>();
        boxCollider = GetComponent<BoxCollider2D>();

        // Set Rigidbody to continuous collision detection
        body.collisionDetectionMode = CollisionDetectionMode2D.Continuous;
    }

    private void Update()
    {
        if (isDashing)
        {
            return;
        }
        float horizontalInput = Input.GetAxis("Horizontal");
        body.velocity = new Vector2(0 * speed, body.velocity.y);

        if (horizontalInput > 0.01f)
        {
            transform.localScale = Vector3.one;
        }
        else if (horizontalInput < -0.01f)
        {
            transform.localScale = new Vector3(-1, 1, 1);
        }

        if (Input.GetMouseButtonDown(0) && canDash)
        {
            StartCoroutine(Dash());
        }
    }

    private IEnumerator Dash()
    {
        canDash = false;
        isDashing = true;
        body.velocity = new Vector2(transform.localScale.x * dashPower, 0f);
        yield return new WaitForSeconds(dashTime);
        isDashing = false;
        yield return new WaitForSeconds(dashCooldown);
        canDash = true;

        // Detect all enemies within the attack range once
        Collider2D[] enemiesInRange = Physics2D.OverlapCircleAll(attackpos.position, attackrange, enemy);

        // Apply damage to all detected enemies based on their type
        foreach (Collider2D enemyCollider in enemiesInRange)
        {
            // Check for each type and apply damage accordingly
            beEnemy beEnemyComponent = enemyCollider.GetComponent<beEnemy>();
            if (beEnemyComponent != null)
            {
                beEnemyComponent.TakeDam(dashDamage);
                continue; // Continue to next enemy if the component was found
            }

            Enemy enemyComponent = enemyCollider.GetComponent<Enemy>();
            if (enemyComponent != null)
            {
                enemyComponent.TakeDam(dashDamage);
                continue;
            }

            // If you have a third enemy type, add additional checks here
            rangedenemy thirdEnemyComponent = enemyCollider.GetComponent<rangedenemy>();
            if (thirdEnemyComponent != null)
            {
                thirdEnemyComponent.TakeDam(dashDamage);
            }
        }
    }
}
