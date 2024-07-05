/**
 * Implements the logic to move a gameObject to a target 
 * the gameObject MUST contain a Movement class implementing a move_toward function to call every fixed frame
 * 
 * 
 * 
 * 
 * 
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementCommand : MonoBehaviour
{
    BoxCollider2D collider;
    EntityState state;
    Movement movement;
    PathFinding pathFinding;
    public Path path;
    public int pathIndex = 0;
    int MAX_FRAMES_STUCK = 50 * 5;//allow 5s of time being stuck than resolve
    int n_frames_stuck = 0;// n_frames stuck consecutively while trying to move
    
    public Vector3 target;// used to continue loading_path to the current target

    int MAX_SEARCHING_FRAMES = 50 * 10;// how many frames a target path is allowed to
    public int SEARCHING_FRAMES = 0;

    float waypoint_proximity = 0.01f;

    // consider other entity's during pathing
    bool check_collision_priority(GameObject other)
    {
        if (path == null)
            return false;
        // heuristic to choose which entity continues and which entity pauses and re-searches
        MovementCommand movementCommand2 = other.GetComponent<MovementCommand>();
        Path path2 = movementCommand2.path;
        if (path2 == null)
            return true;
        int pathIndex2 = movementCommand2.pathIndex;
        int pathSize2 = path2.size();
        int cost2 = path2.cost / Mathf.Max(1, (pathSize2 - pathIndex2));
        int cost = path.cost / Mathf.Max(1, (path.size() - pathIndex));
        return cost > cost2;
    }
    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.tag == "Entity" && check_collision_priority(col.gameObject))
        {
            state.set("colliding");
        }
    }
    void OnTriggerStay2D(Collider2D col)
    {
        if (col.gameObject.tag == "Entity" && check_collision_priority(col.gameObject))
        {
            state.set("colliding");
        }
    }
    void OnTriggerExit2D(Collider2D col)
    {
        if (col.gameObject.tag == "Entity" && check_collision_priority(col.gameObject))
        {
            state.set("searching");
        }
    }

    void Awake()
    { 
        collider = gameObject.GetComponent<BoxCollider2D>();
        state = gameObject.GetComponentInChildren<EntityState>();
        if (state == null)
        {
            Debug.LogError("EntityState component not found in children of " + gameObject.name);
        }
        state.set("idle");

        pathFinding = gameObject.GetComponent<PathFinding>();
        movement = gameObject.GetComponent<Movement>();
    }

    public void move_to_target(Vector3 target)
    {
        if (!World.cell_walkable(target))
            return;// dont move if target cell not walkable
        this.target = target;
        load_path();
    }

    void load_path()
    {
        if (!World.cell_walkable(this.target))
        {
            clear_path();
            return;
        }
            
        path = pathFinding.path(transform.position, this.target);
        pathIndex = 0;
        state.set("searching");
        if (path == null)// path too complex so didnt finish loading, change state back to idle
        {
            clear_path();
            state.set("searching");
        }
        else
        {
            state.set("moving");
        }
    }

    public void clear_path()
    {
        path = null;
        pathIndex = 0;
        state.set("idle");
    }

    void FixedUpdate()
    {
        if (n_frames_stuck > MAX_FRAMES_STUCK)// stuck too long so re-path
        {
            pathIndex = Mathf.Max(0, pathIndex-1);
            n_frames_stuck = 0;
            state.set("searching");
        }

        if (state.get() == "moving")// follow path
        {
            pathFinding.drawPath(path, pathIndex);
            Vector3Int path_waypoint = path.get(pathIndex).head;// current path arc to move to
            //if (World.snapToGrid(transform.position) == path_waypoint)// waypoint reached close
            if ((transform.position-path_waypoint).magnitude < waypoint_proximity)// waypoint reached within distance
            {
                n_frames_stuck = 0;
                pathIndex++;
                if (pathIndex >= path.size())//path complete
                    clear_path();
            }
            else
            {
                n_frames_stuck++;
                movement.move_toward(path_waypoint);
            }
        }
        else if (state.get() == "searching")// continue loading path
        {
            movement.stop_moving();
            load_path();
            SEARCHING_FRAMES++;
        }
        else if(state.get() == "idle")
        {
            movement.stop_moving();
        }
        else if (state.get() == "colliding")
        {
            StartCoroutine(pause_and_search(Random.Range(1f, 2f)));
        }
    }


    private IEnumerator pause_and_search(float waitTime)
    {
        collider.enabled = false;
        state.set("idle");
        yield return new WaitForSeconds(waitTime);
        state.set("searching");
        collider.enabled = true;
    }

}