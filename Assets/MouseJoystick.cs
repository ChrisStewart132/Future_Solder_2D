using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseJoystick : MonoBehaviour
{
    float radius = 2f;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 mouse_pos = World.get_mouse_position();
        if (Input.GetMouseButtonDown(0))
        {
            Vector3 dir = mouse_pos - transform.position;
            if (dir.magnitude < radius)
            {
                // apply x y to axis input
            }
        }

    }
}
