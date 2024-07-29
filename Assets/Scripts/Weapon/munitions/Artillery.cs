using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Future_War_2D.Interfaces;

public class Artillery : MonoBehaviour, IProjectile
{
    public Vector2 start;
    public Transform target;
    public float speed = 1f;
    
    void Awake()
    {

    }

    void Start()
    {
        
    }

    void FixedUpdate()
    {
        
    }

    public void shoot(Vector2 dir)
    {
        start = transform.position;
    }

    public void set_colliders_ignored(List<Collider2D> colliders_ignored)
    {

    }

}
