using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Future_War_2D.Interfaces;

public class Projectile : MonoBehaviour, IProjectile
{
    public int armour_class = 3;//1-6
    public float speed = 2f;
    public float lifespan = 10f;
    public float damage = 10f;
    public AudioSource ricochet_sound;

    private List<Collider2D> colliders_ignored;
    private Rigidbody2D rb;
    Collider2D col;

    public LayerMask layerMask; // Set this in the Inspector to include specific layers

    void Awake()
    {
        rb = gameObject.GetComponent<Rigidbody2D>();
        col = gameObject.GetComponent<Collider2D>();
    }

    // Start is called before the first frame update
    void Start()
    {
        Destroy(gameObject, lifespan);
    }

    void check_collision_ahead()
    {
        if(col.enabled == false) return;


        Vector2 direction = rb.velocity.normalized;
        Vector2 currentPosition = transform.position;
        Vector2 nextPosition = currentPosition + direction * speed * Time.fixedDeltaTime;

        //RaycastHit2D hit = Physics2D.Raycast(currentPosition, direction, Vector2.Distance(currentPosition, nextPosition), layerMask);
        RaycastHit2D[] hits = Physics2D.RaycastAll(currentPosition, direction, Vector2.Distance(currentPosition, nextPosition), layerMask);

        foreach (RaycastHit2D hit in hits)
        {
            if (hit.collider != null && !colliders_ignored.Contains(hit.collider))
            {
                // Generate a random ricochet speed within a specific range
                float minRicochetSpeed = 5f;
                float maxRicochetSpeed = 15f;
                float ricochet_speed = Random.Range(minRicochetSpeed, maxRicochetSpeed);

                // stop move to collision point
                rb.velocity = Vector2.zero; // Stop the projectile
                transform.position = hit.point; // Move to the point of collision

                Armour armour = hit.collider.gameObject.GetComponent<Armour>();
                IDamageable damageable = hit.collider.gameObject.GetComponent<IDamageable>();
                if (armour == null)
                {
                    if (damageable != null)
                    {
                        damage_hit(damageable);
                    }
                }
                else if (armour.durability <= 0)
                {
                    if (damageable != null)
                    {
                        damage_hit(damageable);
                    }
                }
                else if (armour_class < armour.armour_class)
                {
                    // ricochet
                    rb.velocity = hit.normal * ricochet_speed;
                    Destroy(gameObject, Time.fixedDeltaTime * 20);
                    ricochet_sound.Play();
                    return;
                }
                else if (armour_class == armour.armour_class)
                {
                    // damage armour
                    armour.durability--;
                }
                else
                {
                    // penetrate armor
                    if (damageable != null)
                    {
                        damage_hit(damageable);
                    }
                }
                col.enabled = false;
                Destroy(gameObject, 1);
                break;// break after finding the first valid collision in the raycast
            }
        }
    }

    void FixedUpdate()
    {
        check_collision_ahead();
    }

    public void set_colliders_ignored(List<Collider2D> colliders_ignored)
    {
        this.colliders_ignored = colliders_ignored;
    }

    public void shoot(Vector2 dir)
    {
        //Debug.Log(dir.ToString("0.000"));
        rb.velocity = dir.normalized * speed;
        check_collision_ahead();
    }

    void damage_hit(IDamageable damageable)
    {
        damageable.hit(damage);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // TODO only handle collisions using raycast in check collision ahead
        if(colliders_ignored.Contains(collision))
        {
            return;
        }
        /*
        IDamageable damageable = collision.gameObject.GetComponent<IDamageable> ();
        if(damageable != null)
        {
            hit(damageable);
        }*/
        //Destroy(gameObject);
        //Debug.Log(collision.gameObject.name + " hit");
    }

}
