using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Playerhp : MonoBehaviour
{
    public int maxhp = 100;
    private int curhp;
    void Start()
    {
        curhp = maxhp;
    }

    public void TakeDam(int dam)
    {
        curhp -= dam;

        if (curhp <= 0)
        {
            Die();
        }
    }
    
    void Die()
    {
        GetComponent<Collider2D>().enabled = false;
        this.enabled = false;
    }
}
