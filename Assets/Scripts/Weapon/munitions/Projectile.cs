using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public float speed = 2f;
    public float lifespan = 10f;
    public List<Collider2D> colliders_ignored;// set by the gun

    private Rigidbody2D rb;
    

    void Awake()
    {
        rb = gameObject.GetComponent<Rigidbody2D>();
    }

    // Start is called before the first frame update
    void Start()
    {
        Destroy(gameObject, lifespan);
    }

    public void shoot(Vector2 dir)
    {
        //Debug.Log(dir.ToString("0.000"));
        rb.velocity = dir.normalized * speed;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(colliders_ignored.Contains(collision))
        {
            return;
        }
        Destroy(gameObject);
        //Debug.Log(collision.gameObject.name + " hit");
    }
}
