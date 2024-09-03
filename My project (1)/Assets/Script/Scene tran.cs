using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scenetran : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            ScenceCollter.instace.NextLevel();
        }
    }
}
