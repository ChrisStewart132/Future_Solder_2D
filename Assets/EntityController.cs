using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntityController : MonoBehaviour
{
    // entity will orient themselves to face the mouse world pos
    public bool mouse_aim_point = false;



    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if(mouse_aim_point == true)
        {
            aim_at_mouse_point();
        }
    }

    void aim_at_mouse_point()
    {
        // Get the mouse position in world space
        Vector3 mouse_pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mouse_pos.z = 0f; // Ensure the z-position is zero for a 2D game

        // Calculate the direction from the object to the mouse position
        Vector3 direction = mouse_pos - transform.position;

        // Calculate the angle in degrees
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

        // Set the rotation of the object to face the mouse position
        transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle+90f));
    }
}
