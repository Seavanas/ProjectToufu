using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shot2Script : MonoBehaviour
{

    //Moves in a linear fashion

    private Rigidbody2D rigidBody;
    private bool TriggerOnlyOnce = false;
    public float Speed;
    // Use this for initialization
    void Start()
    {
        rigidBody = GetComponent<Rigidbody2D>();
    }


    //SpeedVector is the direction to set. Should be a unit vector (length = 1)
    void SetVelocity(Vector2 speedVector)
    {
        rigidBody.velocity = speedVector * Speed;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player") && !TriggerOnlyOnce)
        {
            gameObject.transform.localScale *= 1.4f;
            TriggerOnlyOnce = true;
        }
    }
}
