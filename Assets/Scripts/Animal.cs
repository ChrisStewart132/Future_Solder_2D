using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Future_War_2D.Interfaces;

public class Animal : MonoBehaviour
{
    public float wanderRadius = 1f;
    public float wanderInterval = 2f;
    public float stopInterval = 4f;

    IMovement movement;

    void Awake()
    {
        movement = GetComponent<IMovement>();
    }

    void Start()
    {
        StartCoroutine(Wander());
    }

    void FixedUpdate()
    {

    }

    IEnumerator Wander()
    {
        while (true)
        {
            movement.Move(GetRandomPosition());
            yield return new WaitForSeconds(wanderInterval);
            movement.Stop();
            yield return new WaitForSeconds(stopInterval);
        }
    }

    Vector2 GetRandomPosition()
    {
        return (Vector2)transform.position + Random.insideUnitCircle * wanderRadius;
    }
}
