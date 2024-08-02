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
    public bool show_moused_over_tile=false;

    // GameObject cell to render mouse grid pos
    public GameObject mouse_cell_object;

    // GameObject mouse selection rect for mouse down and drag selecting 'Selectable' GameObjects
    public GameObject mouse_selection_object;
    SelectionBox selectionBox;// selection box script contains a List<> of Selectables
    Vector3 mouse_down_position;// save mouse pos of last mouse down
    bool selecting = false;// flag for selection box 
    
    public static AudioSource audioData;

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

        audioData = gameObject.GetComponent<AudioSource>();
    }

    void Update()
    {
        Vector3 mouseWorldPosition = World.get_mouse_position();


        if (show_moused_over_tile)
        {
            mouse_cell_object.SetActive(true);
            Vector3Int cell_pos = World.snapToGrid(mouseWorldPosition);
            mouse_cell_object.transform.position = cell_pos;
        }
        else
        {
            mouse_cell_object.SetActive(false);
        }
        

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
            //selected_guns_shoot(mouseWorldPosition);
        }

        
    }

    void selected_guns_shoot(Vector3 pos)
    {
        List<Selectable> selectedObjects = selectionBox.GetSelectedObjects();
        for (int i = 0; i < selectedObjects.Count; i++)
        {
            Selectable selectable = selectedObjects[i];
            GameObject go = selectable.gameObject;
            Collider2D col = go.GetComponent<BoxCollider2D>();
            Gun gun = go.GetComponentInChildren<Gun>();
            if(gun != null)
            {
                Vector3 dir = pos - go.transform.position;
                dir = dir.normalized;                
                gun.shoot(dir);
                gun.colliders_ignored.Add(col);  
            }
        }
    }

    void move_selected_objects(Vector3 pos)
    {
        List<Selectable> selectedObjects = selectionBox.GetSelectedObjects();
        //List<Vector3> positions = get_positions_around_target(pos, selectedObjects.Count);
        for (int i = 0; i < selectedObjects.Count; i++)
        {
            Selectable selectable = selectedObjects[i];
            if(selectable.gameObject.GetComponent<MovementCommand>() != null && selectable.gameObject.tag != "Enemy")
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
            if (sprite.name == "ground" || true)
            {
                return 1;
            }
            else if (false)
            {
                return -1;
            }
            else
            {
                return 999;
            }
        }
        return 0;
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
        if (sprite.name == "ground" || true)
        {
            return true;
        }
        return false;
    }

    public static void interact(Vector3 pos)
    {
        Vector3Int cellPosition = grid.WorldToCell(pos);
        Sprite sprite = tilemap.GetSprite(cellPosition);
        if (sprite == null)
        {
            return;
        }
        if (sprite.name == "transmitter")
        {
            Debug.Log("interacted.");
            audioData.Play(0);
        }
    }
}