/**
 * Attach to a gameObject with multiple SpriteRenderers, one for each state
 * 
 * controls spriteRenderers to show the state of a gameObject
 * 
 * 
 * 
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntityState : MonoBehaviour
{
    SpriteRenderer[] states;
    void Awake()
    {
        states = gameObject.GetComponentsInChildren<SpriteRenderer>();
        reset();
    }

    void reset()
    {
        for (int i = 0; i < states.Length; i++)
        {
            if (states[i].gameObject.name != "selected")
            {
                states[i].enabled = false;
            }
        }
    }

    public void set(string state, bool status=true)
    {
        if (state != "selected")
            reset();
        for (int i = 0; i < states.Length; i++)
        {
            if (states[i].gameObject.name == state)
                states[i].enabled = status;
        }
    }


    public string get()
    {
        for (int i = 0; i < states.Length; i++)
        {
            if (states[i].enabled)
                 return states[i].name;
        }
        return "";
    }
}
