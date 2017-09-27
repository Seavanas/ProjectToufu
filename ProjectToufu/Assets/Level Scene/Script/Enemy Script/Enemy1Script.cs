using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy1Script : MonoBehaviour {
    private BaseEnemyScript baseEnemyScript;
    private bool triggerOnceOnly = false;

    private Rigidbody2D rb;
    private Vector2 perlinSeed;
    public Vector2 perlinRate;
    public float speed;

    // Use this for initialization
    void Start()
    {
        baseEnemyScript = GetComponent<BaseEnemyScript>();
        rb = GetComponent<Rigidbody2D>();
        perlinSeed = new Vector2(Random.Range(1, 10), Random.Range(1, 10));
    }

	// Update is called once per frame
	void Update () {
        if (baseEnemyScript.health <= 0)
        {
            GetComponent<Animator>().SetInteger("State", 1);
            if (!triggerOnceOnly)
            {
                baseEnemyScript.scoreAdd();
                triggerOnceOnly = true;
            }
        } 
	}

    void FixedUpdate()
    {
        float perlineValue = Mathf.PerlinNoise(perlinSeed.x + Time.time * perlinRate.x, perlinSeed.y + Time.time * perlinRate.y);
        rb.velocity = transform.up * speed * perlineValue;
    }
}
