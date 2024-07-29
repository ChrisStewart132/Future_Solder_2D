using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class AI : MonoBehaviour
{



    private MovementCommand movementCommand;
    private EntityState state;
    private Sensor sensor;
    private Transform target;

    void Awake()
    {
        movementCommand = gameObject.GetComponent<MovementCommand>();
        state = gameObject.GetComponentInChildren<EntityState>();
        sensor = gameObject.GetComponentInChildren<Sensor>();
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    void find_target()
    {

    }

    void idle()
    {
        if (sense_target())
        {
            // set movement target
            movementCommand.target = target.position;
        }
        else
        {
            float distance = 8f;
            float random_x = Random.Range(-0.5f, 0.5f) * distance;
            float random_y = Random.Range(-0.5f, 0.5f) * distance;
            movementCommand.target = transform.position + new Vector3(random_x, random_y, 0);
        }
            

        // change to searching to init pathfinding
        state.set("searching");
    }

    bool sense_target()
    {
        if (sensor.sensedObjects.Count == 0)
            return false;

        foreach (GameObject obj in sensor.sensedObjects)
        {
            if (obj == null)
                continue;
            if (obj.CompareTag("Player"))
            {
                target = obj.transform;
                return true;
            }
            else if (obj.CompareTag("Enemy"))
            {

            }
            else // neutral
            {

            }
        }
        return false;
    }

    void FixedUpdate()
    {
        if (state.get() == "moving")
        {

        }
        else if (state.get() == "searching")
        {

        }
        else if (state.get() == "idle")
        {
            idle();
        }
        else if (state.get() == "colliding")
        {
            
        }
    }

    // Update is called once per frame
    void Update()
    {

    }
}
