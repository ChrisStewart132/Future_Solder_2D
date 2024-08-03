using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectionBox : MonoBehaviour
{
    public List<Selectable> selectedObjects = new List<Selectable>();

    // called when this GameObject collides with GameObject2.
    void OnTriggerEnter2D(Collider2D col)
    {
        Selectable selectable = col.GetComponent<Selectable>();
        if (selectable != null)
        {
            selectable.Select();
            selectedObjects.Add(selectable);
        }
    }

    void OnTriggerExit2D(Collider2D col)
    {

        /*
        Selectable selectable = col.GetComponent<Selectable>();
        if (selectable != null)
        {
            selectable.Deselect();
            selectedObjects.Remove(selectable);
        }*/
    }

    public List<Selectable> GetSelectedObjects()
    {
        return selectedObjects;
    }
}
