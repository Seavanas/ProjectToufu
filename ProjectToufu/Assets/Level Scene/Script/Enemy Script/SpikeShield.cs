using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpikeShield : MonoBehaviour
{
    //Attack
    public Vector2 chargePosition;
    public bool targetFound;
    public float chargeRate;
    private float nextCharge;
    private bool charging;

    //Movement stuff
    public float chargeSpeed;
    private Rigidbody2D rb;
    //private GameObject target;

    //Stats
    public BaseEnemyScript baseEnemyScript;
    public BaseBossScript baseBossScript;
    private float currentHealth;

    void Start ()
    {
        rb = GetComponent<Rigidbody2D>();
        currentHealth = baseEnemyScript.health;
        baseBossScript.InitiateHUD();
    }
	
	void Update () {
        if (currentHealth != baseEnemyScript.health)
        {
            baseBossScript.SetHealth(baseEnemyScript.health / baseEnemyScript.GetMaxHealth());
            currentHealth = baseEnemyScript.health;
        }
        else
        {
            GameObject target = GameObject.FindGameObjectWithTag("Player");
            if (target != null)
            {
                if ((Time.time > nextCharge) && targetFound || charging)     //Start charging or in the middle of charging
                {
                    charging = true;    //Charging atm
                    transform.position = Vector2.MoveTowards(transform.position, chargePosition, chargeSpeed);  //Charge towards chargePosition
                    if ((Vector2)transform.position == chargePosition)      //Reached the chargePosition
                    {
                        charging = false;   //Not charging anymore
                        nextCharge = Time.time + chargeRate;   //Timer starts ticking once the charge is over (meaning that it starts counting once the enemy is at the bottom)
                    }
                }
                else
                {
                    TurnToTarget(target);   //Try to face the target
                }
            }
        }
    }

    void TurnToTarget(GameObject target)
    {
        float angleDifference = Vector2.SignedAngle(-transform.up, target.transform.position - transform.position);      //Closest angle to target, -transform because the init sprite
        transform.Rotate(0, 0, angleDifference * 2 * Time.deltaTime);   //Turn 2*angle per sec, so the larger the angle the faster it turns
    }
}
