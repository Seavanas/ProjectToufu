using System.Collections;
using System.Collections.Generic;
using UnityEngine;





/*
 * SCRIPT INFO:
 * 
 * Controls behaviour as well as implement DestroyByTouch
 * 
 * 
 * 
 * 
 */
public class Enemy2 : MonoBehaviour {
    
    private bool onlyTriggerOnce = false, onlyTriggerOnce2 = false;
    private float timeCount, timeSinceLastAttack;
    public GameObject Explosion;
    private BaseEnemyScript baseEnemyScript;

    public float Acceleration;
    public float AttackRate;
    public float MaxAngleOfShots;
    public int NumberOfShotsPerAttack;
    public GameObject ShotSpawn;
    public GameObject ShotType;
    // Use this for initialization
    void Start () {
        baseEnemyScript = GetComponent<BaseEnemyScript>();
        timeSinceLastAttack = Time.time + 2;
    }
	
	// Update is called once per frame
	void Update () {
        if (baseEnemyScript.health <= 0)
        {
            if (!onlyTriggerOnce)
            {
                onlyTriggerOnce = true;
                ConjureExplosions();
                baseEnemyScript.scoreAdd();
            }
            else if (Time.time > timeCount && !onlyTriggerOnce2)
            {
                onlyTriggerOnce2 = true;
                Animator thisAnimator = GetComponent<Animator>();
                thisAnimator.SetInteger("State", 1);
                transform.localScale *= 3.2f;
            }
        }
        else if (timeSinceLastAttack < Time.time)
        {
            Attack();
            timeSinceLastAttack = Time.time + AttackRate;
        }
        
    }

    void Attack()
    {
        for (int i = 0; i < NumberOfShotsPerAttack; i++)
        {
            Instantiate(ShotType, ShotSpawn.transform.position, Quaternion.Euler(0, 0, (-MaxAngleOfShots + ((MaxAngleOfShots/(NumberOfShotsPerAttack-1))*2*i) + 180)));
        }
    }


    void ConjureExplosions()//makes explosions shortly before dying
    {
        timeCount = Time.time + 0.2f;
        float rand = Random.Range(0, 3);
        if (rand < 1)
        {
            ConjOption1();
        }
        else if (rand < 2)
        {
            ConjOption2();
        }
        else ConjOption3();
    }

    void ConjOption1()
    {
        Instantiate(Explosion, transform.position + new Vector3(0.5f, 0, 0), Quaternion.identity);
        Instantiate(Explosion, transform.position + new Vector3(-0.9f, -0.09f, 0), Quaternion.identity);
    }

    void ConjOption2()
    {
        Instantiate(Explosion, transform.position + new Vector3(0.8f, -0.16f, 0), Quaternion.identity);
        Instantiate(Explosion, transform.position + new Vector3(-0.5f, 0.2f, 0), Quaternion.identity);
    }

    void ConjOption3()
    {
        Instantiate(Explosion, transform.position + new Vector3(0.5f, 0.03f, 0), Quaternion.identity);
        Instantiate(Explosion, transform.position + new Vector3(-0.5f, -0.15f, 0), Quaternion.identity);
    }

}

