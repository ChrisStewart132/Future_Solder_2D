/**
 * Attach to Entity gameObject
 * 
 * controls the entity's movement
 * 
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class MovementController : MonoBehaviour
{
    void Update()
    {
        Vector3 mouseWorldPosition = World.get_mouse_position();
        if (Input.GetMouseButtonUp(0))
        {
            //World.remove_tile(mouseWorldPosition);
        }
        else if (Input.GetMouseButtonUp(1))
        {
            gameObject.GetComponent<MovementCommand>().move_to_target(mouseWorldPosition);
        }
    }
}
