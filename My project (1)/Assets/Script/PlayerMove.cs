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
    private int dashDamage = 20;
    private float dashTime = 0.2f;
    private float dashCooldown = 0.2f;
    public Transform attackpos;
    public float attackrange;
    public LayerMask enemy;
   

    [SerializeField] private TrailRenderer trailRenderer;

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
        trailRenderer.emitting = true;
        yield return new WaitForSeconds(dashTime);
        trailRenderer.emitting = false;
        isDashing = false;
        yield return new WaitForSeconds(dashCooldown);
        canDash = true;

        Collider2D[] enemydam = Physics2D.OverlapCircleAll(attackpos.position,attackrange,enemy);
        for (int i = 0; i < enemydam.Length; i++)
        {
            enemydam[i].GetComponent<beEnemy>().TakeDam(dashDamage);
        }
    }
    
   
}
