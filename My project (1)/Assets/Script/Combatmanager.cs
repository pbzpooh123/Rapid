using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Combatmanager : MonoBehaviour
{
    public Animator anim;
    public Transform attackpoint;
    public float attackrange = 0.5f;
    public LayerMask enemylayer;

    public bool isAttacking = false;

    public static Combatmanager instace;

    public Enemy Enemy;

    public int dam = 5;

    private void Awake()
    {
        instace = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.J) && !isAttacking)
        {
            Attack();
        }
    }

    void Attack()
    {
        isAttacking = true;

        Collider2D[] hit = Physics2D.OverlapCircleAll(attackpoint.position, attackrange,enemylayer );

        foreach (Collider2D enemy in hit)
        {
            enemy.GetComponent<Enemy>().TakeDam(dam);
        }
    }
    
}
