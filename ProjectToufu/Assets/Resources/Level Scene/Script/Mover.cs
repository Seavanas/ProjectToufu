using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mover : MonoBehaviour {
    private Rigidbody2D rb;
    public float speed;

    void Start() {
        rb = GetComponent<Rigidbody2D>();
    }

    void FixedUpdate()
    {
        rb.velocity = transform.up * speed;
    }

    public void SetSpeed(int speed)
    {
        this.speed = speed;
        rb = GetComponent<Rigidbody2D>();
        rb.velocity = transform.up * speed;
    }
}
