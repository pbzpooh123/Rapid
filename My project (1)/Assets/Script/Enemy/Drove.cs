using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Drove : MonoBehaviour
{
    public float lifeTime = 1.5f; // Time before the projectile is destroyed

    private void Start()
    {
        // Destroy the projectile after a certain amount of time to prevent it from existing indefinitely
        Destroy(gameObject, lifeTime);
    }
}
