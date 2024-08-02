using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarController : MonoBehaviour
{
    public bool player_controlled = false;
    public float acceleration = 10f;// engine acc
    
    // input
    float steering_input = 0;
    float acceleration_input = 0;
    
    // state
    public float steering_angle = 0f;

    // settings
    float max_steering_angle = 45f;
    float steering_speed = 0.1f;
    float steering_force = 10f;

    float wheel_base = 1.8f;// The distance between front and rear axle is known at the wheel base and denoted as L

    private Rigidbody2D rb;

    GameObject fa, flw, frw;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();

        fa = transform.Find("axleFront").gameObject;
        flw = fa.transform.Find("wheelLeft").gameObject;
        frw = fa.transform.Find("wheelRight").gameObject;
    }


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (player_controlled)
        {
            Vector2 input = Vector2.zero;
            input.x = Input.GetAxis("Horizontal");
            input.y = Input.GetAxis("Vertical");
            steering_input = input.x;
            acceleration_input = input.y;
        }
    }

    void FixedUpdate()
    {
        engineForce();
        steering();
        
    }

    void engineForce()
    {
        float force = rb.mass * acceleration;
        Vector2 dir = transform.up * (acceleration_input+0.0f) * force;
        rb.AddForce(dir, ForceMode2D.Force);
    }

    void steering()
    {
        steering_angle = max_steering_angle * steering_input * -1;
        //Debug.Log(steering_input);
        flw.transform.localRotation = Quaternion.Euler(0, 0, steering_angle);
        frw.transform.localRotation = Quaternion.Euler(0, 0, steering_angle);

        //rb.AddTorque(steering_angle * steering_force * rb.velocity.magnitude);


        // Calculate the turning radius
        // Ensure steering_angle is in radians for the trigonometric functions
        float steering_angle_rad = steering_angle * Mathf.Deg2Rad;

        // Avoid division by zero when steering_angle is zero
        float r = Mathf.Abs(Mathf.Sin(steering_angle_rad)) > 0.0001f ? wheel_base / Mathf.Sin(steering_angle_rad) : Mathf.Infinity;

        // Calculate the angular velocity
        float w = rb.velocity.magnitude / r;
        if (steering_angle_rad > 0) w *= -1;

        // Apply the angular velocity to the car
        rb.angularVelocity = steering_input * w * Mathf.Rad2Deg; // Convert angular velocity to degrees per second
    }


}
