using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Future_War_2D.Interfaces;

public class Rocket : MonoBehaviour, IProjectile
{
    public Vector2 direction;
    public float lifespan = 10f;

    // first stage rocket
    public float thrust1 = 1000f;
    public float fuel1 = 1f;// in kg
    public float fuel_per_second1 = 100f;// in kg/s

    // second stage rocket
    public float thrust2 = 2f;
    public float fuel2 = 1f;// in kg
    public float fuel_per_second2 = 1f;// in kg/s

    // Initial mass of the rocket without fuel
    public float dry_mass = 2f;


    private Rigidbody2D rb;
    private List<Collider2D> colliders_ignored;
    

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Start()
    {
        Destroy(gameObject, lifespan);
        //direction = transform.up.normalized;
        rb.mass = dry_mass + fuel1 + fuel2;
    }
    protected void rocket_update()
    {
        Vector2 start = transform.position;
        Debug.DrawLine(start, start + direction);

        // rocket propulsion
        if (fuel1 > 0)
        {
            rb.AddForce(direction * thrust1, ForceMode2D.Force);
            float fuelConsumed = fuel_per_second1 * Time.fixedDeltaTime;
            fuel1 -= fuelConsumed;
            fuel1 = Mathf.Max(fuel1, 0); // Clamp fuel to not go below 0
            rb.mass = dry_mass + fuel1 + fuel2;
        }
        else if (fuel2 > 0)
        {
            rb.AddForce(direction * thrust2, ForceMode2D.Force);
            float fuelConsumed = fuel_per_second2 * Time.fixedDeltaTime;
            fuel2 -= fuelConsumed;
            fuel2 = Mathf.Max(fuel2, 0); // Clamp fuel to not go below 0
            rb.mass = dry_mass + fuel2;
        }
        else
        {
            rb.mass = dry_mass; // When all fuel is consumed, the mass is just the dry mass
            GetComponent<TrailRenderer>().enabled = false;
            GetComponentInChildren<TrailRenderer>().enabled = false;
        }

        // swaying
        Vector2 perpendicularDirection = new Vector2(-direction.y, direction.x);
        float amplitude = 6f;
        float frequency = 6f;
        float sway = Mathf.Sin(Time.time * frequency);
        // Round the sway to the nearest increment
        float roundingIncrement = 0.2f;
        sway = Mathf.Round(sway / roundingIncrement) * roundingIncrement;
        sway *= amplitude;
        rb.AddForce(perpendicularDirection * sway, ForceMode2D.Force);
    }

    void FixedUpdate()
    {
        rocket_update();
    }

    public void set_colliders_ignored(List<Collider2D> colliders_ignored)
    {
        this.colliders_ignored = colliders_ignored;
    }

    public void shoot(Vector2 dir)
    {
        direction = dir.normalized;
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (colliders_ignored.Contains(collision))
        {
            return;
        }
        Destroy(gameObject);
        //Debug.Log(collision.gameObject.name + " hit");
    }

    void OnDestroy()
    {
        GetComponent<Detonate>().detonate();
    }
}
