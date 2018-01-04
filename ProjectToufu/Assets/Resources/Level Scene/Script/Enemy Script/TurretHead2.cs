using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretHead2 : MonoBehaviour {
    
    public GameObject missile1, missile2, missile3, missile4, missileClip, missilePrefab;

    private GameObject target; //target to shoot at

    public int MissileSpeed;

    public float turnSpeed, reloadTime;

    private int whichMissile = 1; // Determines which missile to fire

    private float timeBeforeNextShot, timeDelayToFireNextShot = 0.2f, timeBeforeDoneReloading;

    private bool currentlyFiring = false, currentlyReloading = false;

    private BaseEnemyScript baseEnemyScript;

	// Use this for initialization
    // Set missile speed to 0 at first
	void Start () {
        baseEnemyScript = GetComponent<BaseEnemyScript>();
        
        missile1.GetComponent<Mover>().SetSpeed(0);
        missile2.GetComponent<Mover>().SetSpeed(0);
        missile3.GetComponent<Mover>().SetSpeed(0);
        missile4.GetComponent<Mover>().SetSpeed(0);
    }
	
	// Update is called once per frame
    // Finds player and moves turret head to point towards player
	void Update () {

        if (baseEnemyScript.health <= 0)
        {
            baseEnemyScript.scoreAdd();
            Destroy(gameObject);
        }
        //Debug.Log(transform.up);
        //transform.up = target.transform.position-transform.position;
        // Use clamp to get value
        // Find angle between this and target
        target = GameObject.FindGameObjectWithTag("Player");
        if (target != null)
        {
            Vector2 difference = target.transform.position - transform.position;
            float angle = Vector2.Angle(transform.up, difference);
            float tempAngle = angle;
            transform.Rotate(0, 0, angle);
            if (Vector2.Angle(transform.up, difference) > 0.1) // calculates angle between direction turret is facing and difference, accomodates for the information lost due to float rounding
            {
                //Debug.Log("Switching Sign since angle is "  + Vector2.Angle(transform.up, difference));
                angle *= -1;
            }
            transform.Rotate(0, 0, -tempAngle);

            transform.Rotate(0, 0, Mathf.Clamp(angle, -turnSpeed, turnSpeed));

            if (!currentlyReloading)
            {
                //Checks if turret is aimed at player, and if so then fire
                if (Vector2.Angle(transform.up, difference) <= 0.1)
                {
                    if (!currentlyFiring && !currentlyReloading)
                    {
                        currentlyFiring = true;
                        timeBeforeNextShot = Time.time;
                    }

                }
                FireBarrage();
            }
        }

        if (currentlyReloading && timeBeforeDoneReloading < Time.time)
        {
            //Reloads all missiles
            Destroy(missileClip);
            missileClip = Instantiate(missilePrefab, transform.position, transform.rotation);
            missileClip.transform.SetParent(this.transform);

            missile1 = missileClip.transform.Find("MissileForTurret2_0 1").gameObject;
            missile2 = missileClip.transform.Find("MissileForTurret2_0 2").gameObject;
            missile3 = missileClip.transform.Find("MissileForTurret2_0 3").gameObject;
            missile4 = missileClip.transform.Find("MissileForTurret2_0 4").gameObject;

            currentlyReloading = false;
            Start();
        }



    }

    void FireBarrage()
    {
        if (currentlyFiring && timeBeforeNextShot < Time.time)
        {
            FireSingular();
            if (whichMissile == 1) // this means missiles have all been launched
            {
                currentlyReloading = true;
                currentlyFiring = false;
                timeBeforeDoneReloading = Time.time + reloadTime;
            }
            else timeBeforeNextShot = timeDelayToFireNextShot + Time.time;
        }

    }

    // Shoot 1 rocket
    void FireSingular()
    {
        switch (whichMissile)// each case determines which missile to fire, sets speed and animation to reasonable speed, 
        {
            case 1:
                missile1.transform.parent = null;
                missile1.GetComponent<Missile2Script>().missileSpeed = (MissileSpeed);
                missile1.GetComponent<Animator>().SetInteger("State", 2);
                missile1.GetComponent<BoxCollider2D>().enabled = true;
                break;
            case 2:
                missile2.transform.parent = null;
                missile2.GetComponent<Missile2Script>().missileSpeed = (MissileSpeed);
                missile2.GetComponent<Animator>().SetInteger("State", 2);
                missile2.GetComponent<BoxCollider2D>().enabled = true;
                break;
            case 3:
                missile3.transform.parent = null;
                missile3.GetComponent<Missile2Script>().missileSpeed = (MissileSpeed);
                missile3.GetComponent<Animator>().SetInteger("State", 2);
                missile3.GetComponent<BoxCollider2D>().enabled = true;
                break;
            case 4:
                missile4.transform.parent = null;
                missile4.GetComponent<Missile2Script>().missileSpeed = (MissileSpeed);
                missile4.GetComponent<Animator>().SetInteger("State", 2);
                missile4.GetComponent<BoxCollider2D>().enabled = true;
                break;
        }
        if (++whichMissile == 5)
            whichMissile = 1;
    }
}
