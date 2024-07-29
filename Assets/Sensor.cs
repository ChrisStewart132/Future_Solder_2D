using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sensor : MonoBehaviour
{
    public float radius = 10f;
    public List<GameObject> sensedObjects = new List<GameObject>();
    public Color neutral_color = new Color(1f, 1f, 1f, 0.5f);
    public Color player_color = new Color(0f, 0f, 1f, 0.5f);
    public Color enemy_color = new Color(1f, 0f, 0f, 0.5f);

    CircleCollider2D col;

    void Awake()
    {
        col = GetComponent<CircleCollider2D>();
    }

    void Start()
    {
        col.radius = radius;
    }

    void FixedUpdate()
    {
        
        foreach (GameObject obj in sensedObjects)
        {
            if(obj == null)
                continue;

            Color color = neutral_color;
            if(obj.CompareTag("Player"))
            {
                color = player_color;
            } 
            else if (obj.CompareTag("Enemy"))
            {
                color = enemy_color;
            }
            Debug.DrawLine(transform.position, obj.transform.position, color);
        }
        //Debug.Log(sensedObjects.Count);
    }

    void OnTriggerEnter2D(Collider2D other)
    {

        // Ignore child colliders of the sensor's parent
        if (transform.parent != null && other.transform.IsChildOf(transform.parent))
        {
            return;
        }
        
        // Ignore collider of the sensor's parent
        if (transform.parent != null && other.gameObject == transform.parent.gameObject)
        {
            return;
        }


        if (!sensedObjects.Contains(other.gameObject))
        {
            sensedObjects.Add(other.gameObject);
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (sensedObjects.Contains(other.gameObject))
        {
            sensedObjects.Remove(other.gameObject);
        }
    }
}
