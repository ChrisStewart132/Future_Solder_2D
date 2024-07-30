using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Future_War_2D.Interfaces;

/**
 * Armor Classes: Armor is categorized into different classes, ranging from Class 1 to Class 6. Each class represents a different level of protection:
    Class 1: Minimal protection, effective against low-caliber rounds.
    Class 2: Slightly better protection, effective against pistols.
    Class 3: Effective against intermediate rounds and some rifle rounds.
    Class 4: Protection against most common rifle rounds.
    Class 5: High-level protection against powerful rifle rounds.
    Class 6: The highest protection, resistant to almost all types of ammunition.
 */

public class Armour : MonoBehaviour
{
    public int armour_class = 1;
    public float durability;
    public float max_durability=100;

    private Collider2D col;
    private Rigidbody2D rb;


    void Awake()
    {
        col = GetComponent<Collider2D>();
        rb = GetComponent<Rigidbody2D>();
    }

    void Start()
    {
        // Initialize armor
        durability = max_durability;
    }

    void hit_by_projectile(IProjectile projectile)
    {

    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        IProjectile projectile = collision.gameObject.GetComponent<IProjectile>();
        if (projectile != null)
        {

        }
    }
}
