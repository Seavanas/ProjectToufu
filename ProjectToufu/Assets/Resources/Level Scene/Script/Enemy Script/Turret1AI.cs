using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Turret1AI : MonoBehaviour {
    private Transform target;
    public Transform shotSpawnL;
    public Transform shotSpawnM;
    public Transform shotSpawnR;
    public GameObject shot;
    public float fireRate;
    private float nextFire;
    private BaseEnemyScript baseEnemyScript;

    void Start()
    {
        baseEnemyScript = GetComponent<BaseEnemyScript>();
    }
    void Update() {

        if (baseEnemyScript.health <= 0)
        {
            Destroy(gameObject);
            baseEnemyScript.scoreAdd();
        }

        target = GameObject.FindWithTag("Player").transform;

        if(target != null) {
            this.transform.up = target.transform.position - transform.position;

            if (Time.time > nextFire) {
                nextFire = Time.time + fireRate;
                Instantiate(shot, shotSpawnL.position, transform.rotation);
                Instantiate(shot, shotSpawnM.position, transform.rotation);
                Instantiate(shot, shotSpawnR.position, transform.rotation);
            }
        }
    }
}
