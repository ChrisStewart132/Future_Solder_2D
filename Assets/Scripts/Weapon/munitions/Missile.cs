using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Missile : Rocket
{
    public Transform target; // Target to aim towards
    public float turnSpeed = 15f; // Speed at which the missile will turn

    void FixedUpdate()
    {
        // Update direction to aim towards the target
        if (target != null)
        {
            Vector2 targetDirection = (target.position - transform.position).normalized;
            //direction = Vector2.Lerp(direction, targetDirection, turnSpeed * Time.fixedDeltaTime);
            //direction = targetDirection;
            direction = MirrorDirection(direction, targetDirection);
            //Debug.Log("missile guiding");
        }
        base.rocket_update();
    }

    Vector2 MirrorDirection(Vector2 direction, Vector2 around)
    {
        direction.x *= -1;
        direction.y *= -1;
        return Vector2.Reflect(direction, around);
    }
}
