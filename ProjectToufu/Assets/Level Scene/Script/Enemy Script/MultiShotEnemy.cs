using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MultiShotEnemy : MonoBehaviour {
    //Shot
    public GameObject shot;
    public Transform shotSpawn;
    public int shotAmount;
    public int shotSpeed;
    public float shotAngle;
    public float fireRate;
    private float nextFire;

    //Stats
    public BaseEnemyScript baseEnemyScript;

    void Start ()
    {
        baseEnemyScript = GetComponent<BaseEnemyScript>();
        shot.GetComponent<Mover>().SetSpeed(shotSpeed);
    }

    void Update()
    {
        if (baseEnemyScript.health <= 0)
        {
            baseEnemyScript.scoreAdd();
            Destroy(gameObject);
        }
        else if (Time.time > nextFire)
        {
            float startAngle;
            if (shotAmount % 2 == 0)     //If shotAmount is even
            {
                startAngle = 180 - (shotAngle * (Mathf.Floor(shotAmount / 2) - 1)) - (shotAngle / 2);
            }
            else
                startAngle = 180 - (shotAngle * Mathf.Floor(shotAmount / 2));

            for (int i = 0; i < shotAmount; i++)
                Instantiate(shot, shotSpawn.position, Quaternion.Euler(0, 0, startAngle + shotAngle * i));

            nextFire = Time.time + fireRate;
        }
    }
}
