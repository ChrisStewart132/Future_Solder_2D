/**
 * Attach to Grid
 * 
 * Singleton class to control tile logic
 *      remove_tile
 *      snapToGrid
 *      getCost
 *      cell_walkable
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.UI;

public class World : MonoBehaviour
{
    public static World Instance { get; private set; }
    public static Grid grid;
    public static Tilemap tilemap;

    // GameObject cell to render mouse grid pos
    public GameObject mouse_cell_object;

    // GameObject mouse selection rect for mouse down and drag selecting 'Selectable' GameObjects
    public GameObject mouse_selection_object;
    SelectionBox selectionBox;// selection box script contains a List<> of Selectables
    Vector3 mouse_down_position;// save mouse pos of last mouse down
    bool selecting = false;// flag for selection box 
    bool mouse_dragging_camera = false;// middle mouse drag camera flag
    Vector3 mouse_middle_mouse_position;// middle mouse drag initial dragging position to scale camera movement

    void Awake()
    {
        // Singleton
        if (Instance != null)
        {
            Debug.LogError("There is more than one instance!");
            return;
        }
        Instance = this;

        grid = gameObject.GetComponent<Grid>();
        tilemap = gameObject.GetComponentInChildren<Tilemap>();

        selectionBox = mouse_selection_object.GetComponent<SelectionBox>();
    }

    void Update()
    {
        Vector3 mouseWorldPosition = World.get_mouse_position();

        // show cell moused over
        Vector3Int cell_pos = World.snapToGrid(mouseWorldPosition);
        mouse_cell_object.transform.position = cell_pos;

        // left click 
        // show mouse down selection
        if (Input.GetMouseButtonDown(0))// starting a selection box
        {
            // deselect previous selection
            List<Selectable> selectedObjects = selectionBox.GetSelectedObjects();
            foreach (Selectable selectable in selectedObjects)
            {
                selectable.Deselect();
            }
            selectedObjects.Clear();


            selecting = true;
            mouse_down_position = mouseWorldPosition;
            mouse_selection_object.SetActive(true); // Show the selection box
        }
        else if (Input.GetMouseButton(0) && selecting)// dragging a selection box
        {
            update_selection_box(mouseWorldPosition);
        }
        else if(Input.GetMouseButtonUp(0) && selecting)// releasing a selection box and or left click selection
        { 
            // raycast to select last mouse position object(s)
            Vector3 direction = Vector3.forward; // Raycast along the Z axis in 3D space
            RaycastHit2D hit = Physics2D.Raycast(mouseWorldPosition, direction, Mathf.Infinity); // Set distance to Infinity for continuous raycast       
            if (hit.collider != null)// Check if the raycast hit something
            {
                GameObject hitObject = hit.collider.gameObject;
                Selectable objectSelectable = hitObject.GetComponent<Selectable>();
                if (objectSelectable != null)
                {
                    objectSelectable.Select();
                    List<Selectable> selectedObjects = selectionBox.GetSelectedObjects();
                    selectedObjects.Add(objectSelectable);
                }
            }
            else
            {
                // Raycast did not hit anything
            }

            // release selection box
            selecting = false;
            mouse_selection_object.SetActive(false); // Hide the selection box
        }
        else
        {

        }

        // right click
        // movement command all selected Selectable GameObjects
        if (Input.GetMouseButtonUp(1))
        {
            move_selected_objects(mouseWorldPosition);
        }

        // middle mouse button down
        if (Input.GetMouseButtonDown(2)) // 2 represents the middle mouse button
        {
            // Capture the starting position of the drag
            mouse_middle_mouse_position = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            mouse_dragging_camera = true;
        }
        // middle mouse button release
        if (Input.GetMouseButtonUp(2))
        {
            mouse_dragging_camera = false;
        }
        // If dragging, calculate the offset and move the camera
        if (mouse_dragging_camera)
        {
            Vector3 difference = mouse_middle_mouse_position - Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Camera.main.transform.position += difference/10f;
        }
    }

    void move_selected_objects(Vector3 pos)
    {
        List<Selectable> selectedObjects = selectionBox.GetSelectedObjects();
        //List<Vector3> positions = get_positions_around_target(pos, selectedObjects.Count);
        for (int i = 0; i < selectedObjects.Count; i++)
        {
            Selectable selectable = selectedObjects[i];
            selectable.gameObject.GetComponent<MovementCommand>().move_to_target(pos);
            //selectable.gameObject.GetComponent<MovementCommand>().move_to_target(positions[i]);
        }
    }

    // todo combine to formation then move
    List<Vector3> get_positions_around_target(Vector3 pos, int n)
    {
        List<Vector3> positions = new List<Vector3>();

        // square lxl positions
        float sqrt = Mathf.Sqrt(n);
        int length = Mathf.CeilToInt(sqrt);


        for (int i = 0; i < length; i++)
        {
            for(int j = 0; j < length; j++)
            {
                Vector3 v = pos + Vector3.right * (j - length / 2) + Vector3.down * (i - length / 2);
                if (World.cell_walkable(v))
                    positions.Add(v);
            }
        } 

        return positions;
    }

    void update_selection_box(Vector3 currentMousePosition)
    {
        Vector3 boxStart = mouse_down_position;
        Vector3 boxEnd = currentMousePosition;
        Vector3 center = (boxStart + boxEnd) / 2;
        center.z = 0;
        Vector3 magnitude = (boxEnd - boxStart);
        mouse_selection_object.transform.localScale = magnitude;
        mouse_selection_object.transform.position = center;
    }

    public static Vector3 get_mouse_position()
    {
        Vector3 mouseOnScreen = Camera.main.ScreenToViewportPoint(Input.mousePosition);
        Vector3 mouseWorldPosition = Camera.main.ViewportToWorldPoint(mouseOnScreen);
        return mouseWorldPosition;
    }

    public static void remove_tile(Vector3 pos)
    {
        Vector3Int cellPosition = grid.WorldToCell(pos);
        TileBase tile = tilemap.GetTile(cellPosition);
        if (tile != null)
        {
            tilemap.SetTile(cellPosition, null);
        }
    }

    public static Vector3Int snapToGrid(Vector3 pos)
    {
        return grid.WorldToCell(pos);
    }
          
    public static int getCost(Vector3 pos)
    {
        Vector3Int cellPosition = grid.WorldToCell(pos);
        Sprite sprite = tilemap.GetSprite(cellPosition);
        if(sprite != null)
        {
            if (sprite.name == "brick" || sprite.name == "brick_2")
            {
                return 9999;
            }
            else if (sprite.name == "silver")
            {
                return -1;
            }
            else if (sprite.name == "gold")
            {
                return -1;
            }
        }
        return 1;
    }

    //returns if a cell is walkable (i.e. false if building or entity occupying it
    public static bool cell_walkable(Vector3 pos)
    {
        Vector3Int cellPosition = grid.WorldToCell(pos);
        Sprite sprite = tilemap.GetSprite(cellPosition);
        if (sprite == null)
        { 
            return true;
        }

        if (sprite.name == "brick" || sprite.name == "brick_2")
        {
            return false;
        }
        return true;
    }
}