using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarController : MonoBehaviour
{
    // editor set items
    public AudioSource idleSound;
    public AudioSource throttleSound;

    // user settings
    public bool player_controlled = false;
    public float acceleration = 10f;// engine acc
    
    // input
    float steering_input = 0;
    float acceleration_input = 0;
    
    // state
    public float steering_angle = 0f;
    public float speed = 0f;
    public float throttle = 0;
    public bool handbrake = false;
    public float forward_velocity = 0f;
    public float lateral_velocity = 0f;
    public float forward_to_lateral_ratio = 0f;
    public bool skidding = false;

    // settings
    float max_steering_angle = 45f;
    float steering_speed = 0.1f;
    float steering_force = 10f;
    float handbrakeDeceleration = 20000f;

    float wheel_base = 1.8f;// The distance between front and rear axle is known at the wheel base and denoted as L


    // private
    private float r;// radius from wheels to centre of turn
    private Rigidbody2D rb;

    GameObject fa, flw, frw, ra, rlw, rrw;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();

        fa = transform.Find("axleFront").gameObject;
        flw = fa.transform.Find("wheelLeft").gameObject;
        frw = fa.transform.Find("wheelRight").gameObject;

        flw.GetComponent<TrailRenderer>().emitting = false;
        frw.GetComponent<TrailRenderer>().emitting = false;


        ra = transform.Find("axleRear").gameObject;
        rlw = ra.transform.Find("wheelLeft").gameObject;
        rrw = ra.transform.Find("wheelRight").gameObject;
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
            throttle = input.y;
            steering_input = input.x;
            acceleration_input = input.y;
            handbrake = Input.GetButton("Jump");
            draw();
        }
    }

    

    void FixedUpdate()
    {
        engineForce();
        steering();
        if (handbrake)
        {
            handbrakeForce();
        }
        tire_skidding();


        sound();
        state();
    }

    void state()
    {
        speed = rb.velocity.magnitude; 
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
        r = Mathf.Abs(Mathf.Sin(steering_angle_rad)) > 0.0001f ? wheel_base / Mathf.Sin(steering_angle_rad) : Mathf.Infinity;

        // Calculate the angular velocity
        float w = rb.velocity.magnitude / r;
        if (steering_angle_rad > 0) w *= -1;


        // Compare velocity to forward direction to determine if reversing
        Vector2 forward = transform.up;
        float dotProduct = Vector2.Dot(rb.velocity.normalized, forward);
        if (dotProduct < 0) // Moving backward
        {
            w *= -1;
        }

        // Apply the angular velocity to the car
        rb.angularVelocity = steering_input * w * Mathf.Rad2Deg; // Convert angular velocity to degrees per second
    }

    void handbrakeForce()
    {
        Vector2 deceleration = -rb.velocity.normalized * handbrakeDeceleration;
        rb.AddForce(deceleration, ForceMode2D.Force);  
    }

    void tire_skidding()
    {
        rlw.GetComponent<TrailRenderer>().emitting = false;
        rrw.GetComponent<TrailRenderer>().emitting = false;

        // lateral skid
        Vector2 forward = transform.up;
        Vector2 right = transform.right;
        forward_velocity = Vector2.Dot(rb.velocity, forward);
        lateral_velocity = Vector2.Dot(rb.velocity, right);
        forward_to_lateral_ratio = forward_velocity / lateral_velocity;

        // forward acc skid
        float acceleration_magnitude = (rb.velocity.magnitude - speed) / Time.fixedDeltaTime;

        bool lateral_skid = Mathf.Abs(forward_to_lateral_ratio) < 2f && rb.velocity.magnitude > 1f;
        bool acceleration_skid = acceleration_magnitude > 6f;

        // skid condition
        if (handbrake || lateral_skid || acceleration_skid)
        {
            rlw.GetComponent<TrailRenderer>().emitting = true;
            rrw.GetComponent<TrailRenderer>().emitting = true;
            rb.velocity *= 0.999f;// skid drag
            skidding = true;
        }
        else
        {
            skidding = false;
        }
    }

    void sound()
    {
        if(throttle != 0 && !throttleSound.isPlaying)
        {
            throttleSound.Play();
            idleSound.Stop();
        }
        else if(throttle == 0)
        {
            throttleSound.Stop();
        }

        if(throttle == 0 && !idleSound.isPlaying)
        {
            throttleSound.Stop();
            idleSound.Play();
        }

        if (skidding && !rlw.GetComponent<AudioSource>().isPlaying)
        {
            rlw.GetComponent<AudioSource>().Play();
            //rrw.GetComponent<AudioSource>().Play();
        }
        else if(!skidding)
        {
            rlw.GetComponent<AudioSource>().Stop();
            //rrw.GetComponent<AudioSource>().Stop();
        }
    }

    void draw()
    {

        // tire forward
        //Debug.DrawLine(flw.transform.position - flw.transform.up / 2, flw.transform.position + flw.transform.up/2);
        //Debug.DrawLine(frw.transform.position - frw.transform.up / 2, frw.transform.position + frw.transform.up/2);

        // tire turning centre
        float right = -1;
        Debug.DrawLine(flw.transform.position, flw.transform.position + flw.transform.right * right * r, Color.white);
        Debug.DrawLine(frw.transform.position, frw.transform.position + frw.transform.right * right * r, Color.white);

        // velocity
        Debug.DrawLine(transform.position, transform.position + (Vector3)rb.velocity, Color.blue);
        Debug.DrawLine(transform.position, transform.position + transform.up * forward_velocity, Color.green);
        Debug.DrawLine(transform.position, transform.position + transform.right * lateral_velocity, Color.red);
    }
}
