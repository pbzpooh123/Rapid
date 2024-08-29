using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attack : MonoBehaviour
{
    public Transform attackpoint;
    public float attackrange = 0.5f;
    public LayerMask enemylayer;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.J))
        {
            Attack2();
        }
    }

    void Attack2()
    { 
        
        Collider2D[] hit = Physics2D.OverlapCircleAll(attackpoint.position,attackrange,enemylayer);
            
        foreach (Collider2D enemy in hit)
        {
            Debug.Log("Hit");
        }
        
    }
}
