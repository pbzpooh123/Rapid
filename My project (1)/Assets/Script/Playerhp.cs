using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Playerhp : MonoBehaviour
{
    public float maxhp = 100f;
    private float curhp;
    public Image hpBar;

    private bool isdead;
    public GameOver gameManager;

    void Start()
    {
        curhp = maxhp;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.H))
        {
            TakeDam(20);
        }
        
    }
   
    
    public void TakeDam(int dam)
    {
        curhp -= dam;
        hpBar.fillAmount = curhp / 100f;

        if (curhp <= 0)
        {
            Die();
        }
    }
    
    void Die()
    {
        GetComponent<Collider2D>().enabled = false;
        this.enabled = false;
        gameManager.gameOver();
        Destroy(gameObject);
    }
}
