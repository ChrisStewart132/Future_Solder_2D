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

    public GameObject mouse_cell_object;

    void Awake()
    {
        if (Instance != null)
        {
            Debug.LogError("There is more than one instance!");
            return;
        }
        Instance = this;
        grid = gameObject.GetComponent<Grid>();
        tilemap = gameObject.GetComponentInChildren<Tilemap>();
    }

    void FixedUpdate()
    {
        
        Vector3Int cell_pos = World.snapToGrid(World.get_mouse_position());
        mouse_cell_object.transform.position = cell_pos;
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
            if (sprite.name == "wall")
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

    public static bool cell_walkable(Vector3 pos)
    {
        Vector3Int cellPosition = grid.WorldToCell(pos);
        Sprite sprite = tilemap.GetSprite(cellPosition);
        if (sprite != null && sprite.name == "wall")
        {
            return false;
        }
        return true;
    }
}