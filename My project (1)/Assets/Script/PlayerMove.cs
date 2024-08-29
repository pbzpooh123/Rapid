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

    private void Awake()
    {
        body = GetComponent<Rigidbody2D>();
        boxcollider = GetComponent<BoxCollider2D>();
    }

    // Update is called once per frame
    void Update()
    {
        float horizontalinput = Input.GetAxis("Horizontal");
        body.velocity = new (Input.GetAxis("Horizontal") * speed,body.velocity.y);

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
}
