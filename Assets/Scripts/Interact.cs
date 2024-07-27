using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interact : MonoBehaviour
{

    // Start is called before the first frame update
    void Start()
    {
        // todo only selected units can interact
    }

    // Update is called once per frame
    void Update()
    {
        bool isSelected = gameObject.GetComponent<Selectable>().isSelected;

        if (isSelected && Input.GetMouseButtonDown(1))
        {
            interact();
        }
    }

    void interact()
    {
        // check nearby grid cells to interact with
        interact_grid();

        // check nearby go's

    }

    void interact_grid()
    {
        Vector3Int cell_pos = World.snapToGrid(transform.position);
        for (int i = -1; i < 2; i++)
        {
            for (int j = -1; j < 2; j++)
            {
                World.interact(cell_pos + new Vector3Int(i, j, 0));
            }
        }
    }
}
