using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoubleShotEnemy : MonoBehaviour {
    //Shot stuff
    public GameObject shot;
    public Transform shotSpawn1, shotSpawn2;
    public float fireRate;
    private float nextFire;

    //Attack point limit, choose a point within the range
    public Vector2 minLimit;
    public Vector2 maxLimit;
    private Vector3 attackPoint;     //Store the chosen attack point

    //Movement stuff
    public float speed;
    private Rigidbody2D rb;
    private Vector3 oldPosition;
    /*
    public float overallSpeed;  //Overall speed when moving
    private Vector2 speed;       //How much the object is moving horizontally and vertically
    public Vector2 maxMovement; //Max sec the object is moving horizontally and vertically before it goes in the oppsite direction
    private float xNextMove;    //To determine if the object should change its x direction
    private float yNextMove;    //To determine if the object should change its y direction
    private Vector2 movement;
    */

    //Stats
    public BaseEnemyScript baseEnemyScript;

    void Start ()
    {
        baseEnemyScript = GetComponent<BaseEnemyScript>();
        rb = GetComponent<Rigidbody2D>();
        NewDirection();

        // float[] xSpeed = { -1f, 1f };
        /*
        if ((attackPosition.x - maxMovement.x) < -7f)
            speed = new Vector2(-1, 0);
        else if ((attackPosition.x + maxMovement.x) > 3f)
            speed = new Vector2(1, 0);
        else
            speed = new Vector2(xSpeed[Random.Range(0,1)], 0);
            */
    }

    void Update ()
    {
        if (baseEnemyScript.health <= 0)
        {
            baseEnemyScript.scoreAdd();
            Destroy(gameObject);
        }
        else if (Time.time > nextFire)
        {
            nextFire = Time.time + fireRate;
            Instantiate(shot, shotSpawn1.position, shotSpawn1.rotation);
            Instantiate(shot, shotSpawn2.position, shotSpawn2.rotation);
        }
    }

    void FixedUpdate()
    {
        rb.velocity = (attackPoint - oldPosition).normalized * speed;
        /*
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
        rb.velocity = movement * overallSpeed;*/
    }
    /*
    bool AtAttackPosition()
    {
        if (transform.position == attackPoint)
            return true;
        return false;
    }
    */
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Border"))
        {
            NewDirection();
        }
    }

    void NewDirection()
    {
        do
        {
            oldPosition = transform.position;
            Random random = new Random();
            attackPoint = new Vector3(Random.Range(minLimit.x, maxLimit.x), Random.Range(minLimit.y, maxLimit.y), 0);
        }
        while (System.Math.Abs(attackPoint.x - oldPosition.x) < 1);     //Make sure the object doesn't move too vertically
    }

    //Gets called by animation event when firing animation finishes playing, resets the state to 0
    public void ResetState()
    {
        GetComponent<Animator>().SetInteger("State", 0);
    }
}
