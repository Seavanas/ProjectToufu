using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AgileEnemy : MonoBehaviour {
    [System.Serializable]
    public class Boundary
    {
        public float xMin, xMax, yMin, yMax;
    }

    //Shot stuff
    public GameObject shot;
    public Transform shotSpawn1, shotSpawn2;
    public float attackRate;
    public float fireRate;
    private float nextFire;
    private bool attacking;
    private bool TriggerOnceOnly = false;

    //Movement stuff
    private Rigidbody2D rb;
    private float angle;
    public float radius;
    public float speed;
    private Vector2 movement;
    private GameObject target;
    public Boundary boundary;
    public bool orbitDirection;     //Count-clock if true, clock if false

    //Stats
    public BaseEnemyScript baseEnemyScript;
    Coroutine attackingCoroutine;//coroutine that makes enemy attack

    void Start()
    {
        baseEnemyScript = GetComponent<BaseEnemyScript>();
        rb = GetComponent<Rigidbody2D>();
        attackingCoroutine = StartCoroutine(attack());
    }

    void Update()
    {
        if (baseEnemyScript.health <= 0)
        {
            if (!TriggerOnceOnly)
            {
                StopCoroutine(attackingCoroutine);
                TriggerOnceOnly = true;
                baseEnemyScript.scoreAdd();
                GetComponent<Animator>().SetInteger("State", 2);
                transform.rotation = Quaternion.identity;
                transform.localScale *= 2.6f;
            }
        }
        else
        {
            target = GameObject.FindGameObjectWithTag("Player");

            if (target != null)
            {
                if (orbitDirection)
                    angle += (2 * Mathf.PI) / radius * Time.deltaTime * speed;
                else
                    angle -= (2 * Mathf.PI) / radius * Time.deltaTime * speed;

                float x = Mathf.Cos(angle) * radius + target.transform.position.x;
                float y = Mathf.Sin(angle) * radius + target.transform.position.y;
                transform.position = Vector2.MoveTowards(transform.position, new Vector2(x, y), 0.1f);
                rb.position = new Vector2(Mathf.Clamp(rb.position.x, boundary.xMin, boundary.xMax), Mathf.Clamp(rb.position.y, boundary.yMin, boundary.yMax));
                //Boundary: size:15x20, offset:(-1.9,0); this.boundary: (-8.6~4.9, -8.6~8.6)
                this.transform.up = target.transform.position - transform.position;
            }
        }
    }

    void FixedUpdate()
    {/*
        if (baseEnemyScript.health <= 0)
        {
            if (!TriggerOnceOnly)
            {
                StopCoroutine(attackingCoroutine);
                TriggerOnceOnly = true;
                baseEnemyScript.scoreAdd();
                GetComponent<Animator>().SetInteger("State", 2);
                transform.rotation = Quaternion.identity;
                transform.localScale *= 2.6f;
            }
        }
        else
        {
            target = GameObject.FindGameObjectWithTag("Player");

            if (target != null)
            {
                if (orbitDirection)
                    angle += (2 * Mathf.PI) / radius * Time.deltaTime * speed;
                else
                    angle -= (2 * Mathf.PI) / radius * Time.deltaTime * speed;

                float x = Mathf.Cos(angle) * radius + target.transform.position.x;
                float y = Mathf.Sin(angle) * radius + target.transform.position.y;
                Vector2 targetLocation = new Vector2(x, y);
                Vector2 movement = new Vector2(targetLocation.x - transform.position.x, targetLocation.y - transform.position.y);
                rb.velocity = movement * speed;
               // rb.position = new Vector2(Mathf.Clamp(rb.position.x, boundary.xMin, boundary.xMax), Mathf.Clamp(rb.position.y, boundary.yMin, boundary.yMax));
                this.transform.up = target.transform.position - transform.position;
            }
        }*/
    }

    IEnumerator attack()
    {
        while(true)
        {
            yield return new WaitForSeconds(attackRate);
            Instantiate(shot, shotSpawn1.position, shotSpawn1.rotation);
            Instantiate(shot, shotSpawn2.position, shotSpawn2.rotation);
            GetComponent<Animator>().SetInteger("State", 1);
            yield return new WaitForSeconds(fireRate);
            Instantiate(shot, shotSpawn1.position, shotSpawn1.rotation);
            Instantiate(shot, shotSpawn2.position, shotSpawn2.rotation);
            GetComponent<Animator>().SetInteger("State", 1);
            yield return new WaitForSeconds(fireRate);
            Instantiate(shot, shotSpawn1.position, shotSpawn1.rotation);
            Instantiate(shot, shotSpawn2.position, shotSpawn2.rotation);
            GetComponent<Animator>().SetInteger("State", 1);
        }
    }

    //Gets called by animation event when firing animation finishes playing, resets the state to 0
    public void ResetState()
    {
        GetComponent<Animator>().SetInteger("State", 0);
    }
}
