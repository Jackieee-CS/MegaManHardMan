using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{

    // Used to tell GameObject (Projectile) how fast to move
    public float speed;

    // Used to tell GameObject (Projectile) how long to live without colliding with anything
    public float lifetime;

    Rigidbody2D rb;


    // Use this for initialization
    void Start()
    {

        rb = this.gameObject.GetComponent<Rigidbody2D>();


        // Check if variable is set to something not 0
        if (lifetime <= 0)
        {
            // Set a default value to variable if not set in Inspector
            lifetime = 1.0f;

            // Prints a message to Console (Shortcut: Control+Shift+C)
            Debug.LogWarning("Lifetime not set on " + name + ". Defaulting to " + lifetime);
        }


        Debug.Log("The speed is " + speed);
        // Take Rigidbody2D component and change its velocity to value passed
        rb.velocity = new Vector2(speed, 0);

        // Destroy gameObject after 'lifeTime' seconds
        Destroy(gameObject, lifetime);
    }

    // Check for collisions with other GameObjects
    // - One or both GameObjects must have a Rigidbody2D attached
    // - Both need colliders attached
    void OnCollisionEnter2D(Collision2D c)
    {
        // Destory GameObject Script is attached to
        Destroy(gameObject);
    }
}