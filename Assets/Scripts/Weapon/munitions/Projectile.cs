using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    private Rigidbody2D rb;
    public Vector3 dir;
    public float speed = 2f;
    public float lifespan = 10f;
    public List<Collider2D> colliders_ignored;
    void Awake()
    {
        rb = gameObject.GetComponent<Rigidbody2D>();
    }
    // Start is called before the first frame update
    void Start()
    {
        Destroy(gameObject, lifespan);
    }
    public void shoot()
    {
        rb.velocity = dir.normalized * speed;
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(colliders_ignored.Contains(collision))
        {
            return;
        }
        Destroy(gameObject);
        Debug.Log(collision.gameObject.name + " hit");
    }
}
