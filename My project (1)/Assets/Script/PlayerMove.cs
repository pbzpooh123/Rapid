using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    [SerializeField] private float speed;
    private Rigidbody2D body;
    private bool grounded;
    [SerializeField] private LayerMask groundlayer;
    private BoxCollider2D boxcollider;
    private bool canDash = true;
    private bool isDash;
    private float dashpower = 20f;
    private int dashdam = 20;
    private float dashTime = 0.2f;
    private float dashCooldown = 1f;

    [SerializeField] private TrailRenderer tr;

    private void Awake()
    {
        body = GetComponent<Rigidbody2D>();
        boxcollider = GetComponent<BoxCollider2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if (isDash)
        {
            return;
        }
        float horizontalinput = Input.GetAxis("Horizontal");
        body.velocity = new (Input.GetAxis("Horizontal") * 0,body.velocity.y);

        if (horizontalinput > 0.01f)
        {
            transform.localScale = Vector3.one;
        }
        else if (horizontalinput < -0.01f)
        {
            transform.localScale = new Vector3(-1, 1, 1);
        }

        if (Input.GetKey(KeyCode.Space) && isGrounded())
        {
            Jump();
        }
        
        if (Input.GetMouseButtonDown(0) && canDash)
        {
            StartCoroutine(Dash());
        }
        
    }

    void Jump()
    {
            body.velocity = new Vector2(body.velocity.x, speed * 1.5f);
            grounded = false;
    }
    

    private bool isGrounded()
    {
        RaycastHit2D raycastHit = Physics2D.BoxCast(boxcollider.bounds.center,boxcollider.bounds.size,0,Vector2.down,0.1f,groundlayer);
        return raycastHit.collider != null;
    }

    private IEnumerator Dash()
    {
        canDash = false;
        isDash = true;
        body.velocity = new Vector2(transform.localScale.x * dashpower,0f);
        tr.emitting = true;
        yield return new WaitForSeconds(dashTime);
        tr.emitting = false;
        isDash = false;
        yield return new WaitForSeconds(dashCooldown);
        canDash = true;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (isDash)
        {
            // Check if the collision is with an enemy
            if (collision.gameObject.CompareTag("Enemy"))
            {
                // Access the enemy's health component and deal damage
                Enemy enemyHealth = collision.gameObject.GetComponent<Enemy>();
                if (enemyHealth != null)
                {
                    enemyHealth.TakeDam(dashdam); // Fixed the method name to 'TakeDamage'
                }
            }
        }
    }
}
