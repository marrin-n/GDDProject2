using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileAttack : MonoBehaviour
{
    private Rigidbody2D rb; 
    private Vector2 dir; 
    private float projectileSpeed; 

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>(); 
        dir = new Vector2(0, 0); 
        projectileSpeed = 4f; 
    }

    void FixedUpdate()
    {
        rb.velocity = dir * projectileSpeed;
    }

    public void setProjectileDirection(Vector2 d) 
    {
        dir = d; 
        dir.Normalize(); 
    }
}
