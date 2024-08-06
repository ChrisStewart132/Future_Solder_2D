/**
 * Attach to Main Camera
 * 
 * Controls movement (wasd) and zoom (mouse scroll)
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public float moveSpeed = 50f; // Speed of camera movement
    public float zoomSpeed = 10f; // The speed of zooming
    public float minZoom = 5f;    // The minimum zoom level
    public float maxZoom = 50f;   // The maximum zoom level
    public bool wasd_movement = false;
    public bool mouse_movement = false;
    public bool anchored = true;
    public Transform anchor;

    // middle mouse dragging variables
    public bool middle_mouse_dragging = false;
    bool mouse_dragging_camera = false;
    Vector3 mouse_middle_mouse_position;

    public bool middle_scroll_zoom = false;

    private Camera cam;

    void Start()
    {
        cam = GetComponent<Camera>();
    }

    void Update()
    {
        if (wasd_movement)
        {
            Vector3 move = new Vector3(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"), 0);
            transform.position += move * moveSpeed * Time.deltaTime;
        }
        if (mouse_movement)
        {
            
        }
        if (anchored && cam.transform != null)
        {
            cam.transform.position = anchor.position;
        }

        if (middle_scroll_zoom)
        {
            float scrollInput = Input.GetAxis("Mouse ScrollWheel");
            if (scrollInput != 0f)
            {
                cam.orthographicSize -= scrollInput * zoomSpeed;
                cam.orthographicSize = Mathf.Clamp(cam.orthographicSize, minZoom, maxZoom);
            }
        }


        if (middle_mouse_dragging)
        {
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
                Camera.main.transform.position += difference / 10f;
            }
        }
        
    }
}
