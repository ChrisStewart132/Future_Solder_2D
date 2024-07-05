using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Selectable : MonoBehaviour
{
    public bool isSelected = false;
    Color selected_color;// = new Color(1,1,1,0.95f);
    Color unselected_color;
    public void Awake()
    {
        unselected_color = GetComponent<SpriteRenderer>().color;
        selected_color = unselected_color*0.8f;
        selected_color = selected_color + new Color(0, 0.5f, 0);
    }

    // Method to mark the object as selected
    public void Select()
    {
        isSelected = true;
        GetComponent<SpriteRenderer>().color = selected_color;
    }

    // Method to mark the object as deselected
    public void Deselect()
    {
        isSelected = false;
        GetComponent<SpriteRenderer>().color = unselected_color;
    }

    // Optional: Method to check if the object is selected
    public bool IsSelected()
    {
        return isSelected;
    }
}
