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
        if (anchored)
        {
            cam.transform.position = anchor.position;
        }

        // scroll zoom
        float scrollInput = Input.GetAxis("Mouse ScrollWheel");
        if (scrollInput != 0f)
        {
            cam.orthographicSize -= scrollInput * zoomSpeed;
            cam.orthographicSize = Mathf.Clamp(cam.orthographicSize, minZoom, maxZoom);
        }
    }
}
