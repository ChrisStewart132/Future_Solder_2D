using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneTransition : MonoBehaviour
{
    // file->build settings... to see scene indexes to switch to
    public int next_scene_index;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log("scene transition collision with " + other.name);
        if(other.tag == "Player")
        {
            SceneManager.LoadScene(next_scene_index, LoadSceneMode.Single);
        }
    }
}
