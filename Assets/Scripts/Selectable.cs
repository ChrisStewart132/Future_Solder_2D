using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Selectable : MonoBehaviour
{
    public bool isSelected = false;
    EntityState es;
    public void Awake()
    {
        es = gameObject.GetComponentInChildren<EntityState>();
    }

    // Method to mark the object as selected
    public void Select()
    {
        isSelected = true;
        es.set("selected", true);
    }

    // Method to mark the object as deselected
    public void Deselect()
    {
        isSelected = false;
        es.set("selected", false);
    }

    // Optional: Method to check if the object is selected
    public bool IsSelected()
    {
        return isSelected;
    }
}
