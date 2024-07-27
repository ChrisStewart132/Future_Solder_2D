using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TankMovement : MonoBehaviour
{
    public float moveForce = 100f;       // Force applied to move the tank
    public float maxSpeed = 1f;         // Maximum speed of the tank
    public float rotationTorque = 100f; // Torque applied to rotate the tank

    private Rigidbody2D rb;

    void Start()
    {
        // Get the Rigidbody2D component attached to the tank
        rb = GetComponent<Rigidbody2D>();
    }

    void FixedUpdate()
    {
        // Get input for movement
        float moveInput = Input.GetAxis("Vertical");  // W and S keys
        float rotateInput = Input.GetAxis("Horizontal"); // A and D keys

        // Calculate movement and rotation
        Vector2 movement = transform.up * moveInput * moveForce;
        float rotation = -rotateInput * rotationTorque;

        // Apply movement force
        rb.AddForce(movement);

        // Clamp the tank's speed to the maximum speed
        if (rb.velocity.magnitude > maxSpeed)
        {
            rb.velocity = Vector2.ClampMagnitude(rb.velocity, maxSpeed);
        }

        // Apply rotation torque and damping
        rb.AddTorque(rotation);
    }
}
