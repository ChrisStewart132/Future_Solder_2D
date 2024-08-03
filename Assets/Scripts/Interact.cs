using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Future_War_2D.Interfaces;

public class Interact : MonoBehaviour
{
    // selected objects can interact with their surroundings
    public bool isSelected = false;
    public float interaction_radius = 1f;
    public LayerMask layerMask; // Layer mask to filter the objects to detect

    void Start()
    {

    }

    void Update()
    {
        isSelected = isSelected || gameObject.GetComponent<Selectable>().isSelected;

        if (isSelected && Input.GetMouseButtonUp(1))
        {
            interact();
        }
    }

    void interact()
    {
        // check nearby grid cells to interact with
        interact_grid();

        // check nearby go's to interact with
        interact_radius();
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

    void interact_radius()
    {
        // Perform the circle cast and get all hits
        RaycastHit2D[] hits = Physics2D.CircleCastAll(transform.position, interaction_radius, Vector2.zero, 0, layerMask);

        // Check if there are any hits
        if (hits.Length > 0)
        {
            foreach (RaycastHit2D hit in hits)
            {
                // Retrieve the GameObject hit by the circle cast
                GameObject go = hit.collider.gameObject;

                // Check if the GameObject has an IInteractable component
                IInteractable interactable = go.GetComponent<IInteractable>();
                if (interactable != null)
                {
                    Debug.Log("Interacting with: " + go.name);
                    interactable.interaction(gameObject);
                    return;
                }
            }
        }
        else
        {
            Debug.Log("No objects hit (interaction).");
        }
    }
}
