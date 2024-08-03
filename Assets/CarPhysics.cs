using UnityEngine;

public class CarPhysics : MonoBehaviour
{
    // car type
    public float wheelbase;
    public float mass;
    public float inertia;
    public float b;
    public float c;
    public float h;


    // car state
    public Vector2 position_wc;
    public Vector2 velocity_wc;
    public float angle;
    public float steerangle;
    public float angularvelocity;
    public float throttle;
    public float brake;


    // physics settings
    public float CA_F = -5.2f; // Front cornering stiffness
    public float CA_R = -5.0f; // Rear cornering stiffness
    public float MAX_GRIP = 2.0f; /* maximum (normalised) friction force, =diameter of friction circle */
    public float RESISTANCE = 30.0f; /* factor for rolling resistance */
    public float DRAG = 5.0f;
    public float max_steering_angle = 50f;
    public bool front_slip = true;
    public bool rear_slip = true;

    // unity
    private Rigidbody2D rb;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Start()
    {
        init();
    }

    void FixedUpdate()
    {
        processInput();
        DoPhysics(Time.fixedDeltaTime);

        // Apply the updated position and angle to the Rigidbody2D
        rb.position = position_wc;
        rb.rotation = angle * Mathf.Rad2Deg;
    }

    void init()
    {
        // init car type
        b = 1.0f;// front axle d from CoG
        c = 1.0f;// rear axle d from CoG
        wheelbase = b + c;
        h = 1.0f;// CoG height
        mass = 1500; // kg
        inertia = 1500; // kg.m

        // init car state
        position_wc = Vector2.zero;
        velocity_wc = Vector2.zero;
        angle = 0f;
        angularvelocity = 0f;
        steerangle = 0f;
        throttle = 0f;
        brake = 0f;
    }

    public void DoPhysics(float delta_t)
    {
        float sn = Mathf.Sin(angle);
        float cs = Mathf.Cos(angle);

        // SAE convention: x is to the front of the car, y is to the right, z is down
        Vector2 velocity;
        velocity.x = cs * velocity_wc.y + sn * velocity_wc.x;
        velocity.y = -sn * velocity_wc.y + cs * velocity_wc.x;

        float yawspeed = wheelbase * 0.5f * angularvelocity;

        float rot_angle = (velocity.x == 0) ? 0 : Mathf.Atan2(yawspeed, velocity.x);

        float sideslip = (velocity.x == 0) ? 0 : Mathf.Atan2(velocity.y, velocity.x);

        float slipanglefront = sideslip + rot_angle - steerangle;
        float slipanglerear = sideslip - rot_angle;

        float weight = mass * 9.8f * 0.5f;

        Vector2 flatf;
        flatf.x = 0;
        flatf.y = CA_F * slipanglefront;
        flatf.y = Mathf.Clamp(flatf.y, -MAX_GRIP, MAX_GRIP) * weight;
        if (front_slip) flatf.y *= 0.5f;

        Vector2 flatr;
        flatr.x = 0;
        flatr.y = CA_R * slipanglerear;
        flatr.y = Mathf.Clamp(flatr.y, -MAX_GRIP, MAX_GRIP) * weight;
        if (rear_slip) flatr.y *= 0.5f;

        Vector2 ftraction;
        ftraction.x = 100 * (throttle - brake * Mathf.Sign(velocity.x));
        ftraction.y = 0;
        if (rear_slip) ftraction.x *= 0.5f;

        Vector2 resistance;
        resistance.x = -(RESISTANCE * velocity.x + DRAG * velocity.x * Mathf.Abs(velocity.x));
        resistance.y = -(RESISTANCE * velocity.y + DRAG * velocity.y * Mathf.Abs(velocity.y));

        Vector2 force;
        force.x = ftraction.x + Mathf.Sin(steerangle) * flatf.x + flatr.x + resistance.x;
        force.y = ftraction.y + Mathf.Cos(steerangle) * flatf.y + flatr.y + resistance.y;

        float torque = b * flatf.y - c * flatr.y;

        Vector2 acceleration;
        acceleration.x = force.x / mass;
        acceleration.y = force.y / mass;

        float angular_acceleration = torque / inertia;

        Vector2 acceleration_wc;
        acceleration_wc.x = cs * acceleration.y + sn * acceleration.x;
        acceleration_wc.y = -sn * acceleration.y + cs * acceleration.x;

        velocity_wc.x += delta_t * acceleration_wc.x;
        velocity_wc.y += delta_t * acceleration_wc.y;

        position_wc.x += delta_t * velocity_wc.x;
        position_wc.y += delta_t * velocity_wc.y;

        angularvelocity += delta_t * angular_acceleration;
        angle += delta_t * angularvelocity;
        
    }

    public void processInput()
    {
        throttle = Input.GetAxis("Vertical") * 100;
        throttle = Mathf.Max(0, throttle);

        brake = Input.GetAxis("Vertical") * 100;
        brake = Mathf.Min(0, brake);

        steerangle = Input.GetAxis("Horizontal") * max_steering_angle;

    }
}
