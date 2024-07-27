using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explosion : MonoBehaviour
{
    // Explosion force parameters
    public float explosionForce = 50000f;
    public float explosionTime = 0.1f;
    //public float explosionRadius = 5f;

    private bool applyAcceleration = false;
    private Rigidbody2D targetRb;
    private Vector2 accelerationDirection;

    void OnTriggerEnter2D(Collider2D other)
    {
        // Check if the parent of the other object has the "Entity" tag
        if(other.CompareTag("Entity") || other.CompareTag("Enemy"))
        //if (other.transform.parent != null && other.transform.parent.CompareTag("Entity"))
        {
            // Get the Rigidbody2D component of the parent object
            targetRb = other.GetComponent<Rigidbody2D>();

            if (targetRb != null)
            {
                accelerationDirection = (other.transform.position - transform.position).normalized;
                applyAcceleration = true;
                StartCoroutine(StopAccelerationAfterTime(explosionTime));
                Destroy(other.gameObject, explosionTime);
            }
        }
    }

    void FixedUpdate()
    {
        if (applyAcceleration && targetRb != null)
        {
            // Apply the continuous force (acceleration)
            targetRb.AddForce(accelerationDirection * explosionForce * Time.fixedDeltaTime);
        }
    }

    IEnumerator StopAccelerationAfterTime(float time)
    {
        yield return new WaitForSeconds(time);
        applyAcceleration = false;
    }
}
