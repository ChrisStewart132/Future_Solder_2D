using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
 * 
 * AI script that sits inside an empty parent go with the child go being the entity
 * 
 * 
 */
public class AI : MonoBehaviour
{
    public bool hostile = true;

    private MovementCommand movementCommand;
    private EntityState state;
    GameObject child_go;
    GameObject target_go;

    void Awake()
    {
        movementCommand = gameObject.GetComponentInChildren<MovementCommand>();
        state = gameObject.GetComponentInChildren<EntityState>();
        if (hostile)
        {
            Transform child = gameObject.transform.GetChild(0); // Assuming the child entity is the first child
            if (child != null)
            {
                child.tag = "Enemy";
                child_go = child.gameObject;
            }
        }
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    void find_target()
    {
        if (target_go == null)
        {

        }
    }


    void FixedUpdate()
    {
        if(child_go == null)
        {
            Destroy(gameObject);
            return;
        }
        if (state.get() == "moving")
        {

        }
        else if (state.get() == "searching")
        {

        }
        else if (state.get() == "idle")
        {
            find_target();
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
