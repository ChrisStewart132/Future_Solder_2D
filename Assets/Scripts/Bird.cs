using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Future_War_2D.Interfaces;

public class Bird : MonoBehaviour, IMovement
{
    public bool flying = false;
    public float flying_speed = 3f;
    public float walking_speed = 1f;
    private Rigidbody2D rb;
    private Animator animator;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    public void Stop()
    {
        animator.SetFloat("Horizontal", 0);
        animator.SetFloat("Vertical", 0);
        animator.SetBool("Flying", flying);
        rb.velocity = Vector3.zero;
    }

    public void Move(Vector2 direction)
    {
        direction.Normalize();
        float moveHorizontal = direction.x;// Input.GetAxis("Horizontal");
        float moveVertical = direction.y;// Input.GetAxis("Vertical");

        // Create movement vector
        Vector2 movement = direction;// new Vector2(moveHorizontal, moveVertical).normalized;

        // Set Animator parameters
        animator.SetFloat("Horizontal", moveHorizontal);
        animator.SetFloat("Vertical", moveVertical);
        animator.SetBool("Flying", flying);

        // Move the character
        float speed = flying ? flying_speed : walking_speed;
        rb.velocity = movement * speed;
    }
}
