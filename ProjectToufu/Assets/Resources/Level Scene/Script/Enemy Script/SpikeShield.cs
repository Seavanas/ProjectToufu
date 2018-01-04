using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpikeShield : MonoBehaviour
{
    //Attack
    public Vector3 oldSelfPosition;
    public Vector3 chargePosition;
    public bool chargeStart;
    public float chargeRate;
    private float nextCharge;
    private bool chargeStop;

    //Movement stuff
    public float chargeSpeed;
    public float turnSpeed;
    private Rigidbody2D rb;

    //Stats
    public BaseEnemyScript baseEnemyScript;
    public BaseBossScript baseBossScript;
    private float currentHealth;
    private bool enrage;

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

        //Experimenting
        if (currentHealth == 450)
        {
            enrage = true;
            chargeRate = 0.5f;
            chargeSpeed = 20;
            turnSpeed = 5;
        }
    }

    void FixedUpdate()
    {
        Transform target = GameObject.FindGameObjectWithTag("Player").transform;
        if (target != null)
        {
            if ((Time.time > nextCharge) && chargeStart && FarEnough(transform.position, target.position) || 
                    chargeStart && FarEnough(transform.position, target.position))     //Start charging or in the middle of charging
            {
                TurnToTarget(oldSelfPosition, chargePosition);      //Make sure to charge at "about" the chargePosition without turning back when charge past the target position

                if (chargeStop)      //Finish charging (hit the border or the player)
                {
                    chargeStart = false;
                    chargeStop = false;
                    rb.velocity = Vector2.zero;
                    nextCharge = Time.time + chargeRate;   //Charge attack goes on cold down
                }
                else if (Vector2.Angle(-transform.up, chargePosition - oldSelfPosition) < 1)  //make sure only charge when facing at "about" the chargePosition 
                {
                    rb.velocity = -transform.up * chargeSpeed;  //Charge forward
                }
            }
            else
            {
                TurnToTarget(transform.position, target.position);   //Try to face the target
            }
        }
    }

    void TurnToTarget(Vector3 self, Vector3 target)
    {
        float angleDifference = Vector2.SignedAngle(-transform.up, target - self);      //Closest angle to target, -transform because the init sprite
        transform.Rotate(0, 0, angleDifference * turnSpeed * Time.deltaTime);   //Turn turnSpeed*angle per sec, so the larger the angle the faster it turns
    }

    bool FarEnough(Vector3 self, Vector3 target)
    {
        if (Vector3.Distance(self, target) < 0.5)
        {
            return false;
        }

        return true;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Border"))   //To prevent player goes into border and let boss charge off
        {
            chargeStop = true;
        }
    }
    //Sometime when player quickly moves in and out of the ChargeScan hitbox boss won't attack, might be the update rate too slow
}
