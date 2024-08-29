using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Combatmanager : MonoBehaviour
{
    public Animator anim;

    public bool isAttacking = false;

    public static Combatmanager instace;

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
        Attack();
    }

    void Attack()
    {
        if (Input.GetKeyDown(KeyCode.J) && !isAttacking)
        {
            isAttacking = true;
            Debug.Log("Test");
        }
    }
}
