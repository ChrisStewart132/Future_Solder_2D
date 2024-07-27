using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Detonate : MonoBehaviour
{
    public GameObject detonation_go_prefab; // Assign in Inspector
    public float detonation_time = 0.4f;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        bool isSelected = gameObject.GetComponent<Selectable>().isSelected;
        if (isSelected && Input.GetMouseButtonDown(0))
        {
            detonate();
        }
    }

    void detonate()
    {
        // Hide this game object
        gameObject.SetActive(false);

        // Create a duplicate of the detonation game object
        GameObject detonation_go = Instantiate(detonation_go_prefab, transform.position, Quaternion.identity);

        // Play detonation sound
        AudioSource detonation_sound = detonation_go.GetComponent<AudioSource>();
        detonation_sound.Play();

        // Destroy the detonation
        Destroy(detonation_go, detonation_time);

        // Destroy this game object
        Destroy(gameObject);
    }

    
}