/**
 * Attach to a gameObject with rigidbody2D
 * 
 * 
 * 
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    public bool wasd_movement = false;
    public float maxSpeed = 4f;
    private Rigidbody2D rb;

    public void Start()
    {
        rb = gameObject.GetComponent<Rigidbody2D>();
    }

    void FixedUpdate()
    {
        if (wasd_movement)
        {
            // Get input from WASD keys (-1, 1) for each axis
            float moveHorizontal = Input.GetAxis("Horizontal");
            float moveVertical = Input.GetAxis("Vertical");

            // Create movement vector 
            Vector3 movement_direction = new Vector2(moveHorizontal, moveVertical);

            move_toward(transform.position + movement_direction);
        }
    }

    public void move_toward(Vector3 target)
    {
        rb.angularVelocity = 0f;
        Vector3 direction = target - transform.position;
        float d = direction.magnitude;
        if(d > maxSpeed * Time.deltaTime)
        {            
            rb.velocity = direction.normalized * maxSpeed;// full speed
        }
        else
        {
            rb.velocity = direction / Time.deltaTime;// ease to target
        }

        // face left(-) or right(+)
        /*
        if(rb.velocity.x >= 0)
        {
            gameObject.GetComponent<SpriteRenderer>().flipX = false;
        }
        else
        {
            gameObject.GetComponent<SpriteRenderer>().flipX = true;
        }*/

    }

    public void stop_moving()
    {
        rb.angularVelocity = 0f;
        rb.velocity = Vector3.zero;
    }
}
