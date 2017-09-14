using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoubleShotEnemy : MonoBehaviour {
    //Shot stuff
    public GameObject shot;
    public Transform shotSpawn1, shotSpawn2;
    public float fireRate;
    private float nextFire;

    //Movement stuff
    private Rigidbody2D rb;
    public float overallSpeed;  //Overall speed when moving
    private Vector2 speed;       //How much the object is moving horizontally and vertically
    public Vector2 maxMovement; //Max sec the object is moving horizontally and vertically before it goes in the oppsite direction
    private float xNextMove;    //To determine if the object should change its x direction
    private float yNextMove;    //To determine if the object should change its y direction
    private Vector2 movement;
    private Vector2 attackPosition;

    //Stats
    public BaseEnemyScript baseEnemyScript;

    void Start ()
    {
        baseEnemyScript = GetComponent<BaseEnemyScript>();
        rb = GetComponent<Rigidbody2D>();
        
        float[] xSpeed = { -1f, 1f };
        Random random = new Random();
        attackPosition = new Vector2(Random.Range(-7f, 3f), Random.Range(5f, 7f));
        if ((attackPosition.x - maxMovement.x) < -7f)
            speed = new Vector2(-1, 0);
        else if ((attackPosition.x + maxMovement.x) > 3f)
            speed = new Vector2(1, 0);
        else
            speed = new Vector2(xSpeed[Random.Range(0,1)], 0);
    }
	
	void Update ()
    {
        if (baseEnemyScript.health <= 0)
        {
            baseEnemyScript.scoreAdd();
            Destroy(gameObject);
        }
        else if ((inAttackPosition()) && (Time.time > nextFire))
        {
            nextFire = Time.time + fireRate;
            Instantiate(shot, shotSpawn1.position, shotSpawn1.rotation);
            Instantiate(shot, shotSpawn2.position, shotSpawn2.rotation);
        }
    }

    void FixedUpdate()
    {
        if (inAttackPosition())
        { 
            if (Time.time > xNextMove)  //Object has moved enough in the x axis
            {
                xNextMove = Time.time + maxMovement.x;       //Reset the counter for max movement
                speed.x = -speed.x;       //Change x direction
            }
            if (Time.time > yNextMove)  //Object has moved enough in the y axis
            {
                yNextMove = Time.time + maxMovement.y;       //Reset the counter for max movement
                speed.y = -speed.y;       //Change y direction
            }
        }
        else
            transform.position = Vector2.MoveTowards(transform.position, attackPosition, 0.1f);

        Vector2 movement = new Vector2(speed.x, speed.y);
        rb.velocity = movement * overallSpeed;
    }

    private bool inAttackPosition()
    {
        if ((transform.position.x != attackPosition.x) && (transform.position.y != attackPosition.y))
            return false;
        return true;
    }

    //Gets called by animation event when firing animation finishes playing, resets the state to 0
    public void ResetState()
    {
        GetComponent<Animator>().SetInteger("State", 0);
    }
}
