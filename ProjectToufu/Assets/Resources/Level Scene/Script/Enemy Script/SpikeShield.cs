using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpikeShield : MonoBehaviour
{
    //Charge
    //private Vector3 oldSelfPosition;
    //private Vector3 chargePosition;
    public float chargeRate;
    public float chargeSpeed;
    private float nextCharge;
    //Charge to target
    private bool chargeStart;
    private bool chargeStop;
    //Enrage
    public GameObject bomb;
    public float bombRate;
    private float nextBomb;
    public float enrageHealth;

    //Movement stuff
    public float turnSpeed;
    private Rigidbody2D rb;

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
	
	void Update ()
    {
        if (currentHealth != baseEnemyScript.health)
        {
            baseBossScript.SetHealth(baseEnemyScript.health / baseEnemyScript.GetMaxHealth());
            currentHealth = baseEnemyScript.health;
        }
    }

    void FixedUpdate()
    {
        Transform target = GameObject.FindGameObjectWithTag("Player").transform;
        if (target != null)
        {
            if ((Time.time > nextCharge) && chargeStart && FarEnough(transform.position, target.position) || chargeStart)        //Target found
            {
                if (chargeStop)         //Finish charging (hit the border)
                {
                    chargeStart = false;
                    chargeStop = false;
                    rb.velocity = Vector2.zero;
                    nextCharge = Time.time + chargeRate;   //Charge attack goes on cold down
                }
                else
                {
                    ChargeAttack();
                    if (currentHealth < enrageHealth)
                    {
                        if ((Time.time > nextBomb))
                        {
                            Instantiate(bomb, transform.position, Quaternion.Euler(0, 0, transform.eulerAngles.z - 30));
                            Instantiate(bomb, transform.position, Quaternion.Euler(0, 0, transform.eulerAngles.z - 90));
                            Instantiate(bomb, transform.position, Quaternion.Euler(0, 0, transform.eulerAngles.z - 150));
                            Instantiate(bomb, transform.position, Quaternion.Euler(0, 0, transform.eulerAngles.z - 210));
                            Instantiate(bomb, transform.position, Quaternion.Euler(0, 0, transform.eulerAngles.z - 270));
                            Instantiate(bomb, transform.position, Quaternion.Euler(0, 0, transform.eulerAngles.z - 330));
                            nextBomb = Time.time + bombRate;
                        }
                    }
                }
            }
            else
            {
                TurnToTarget(transform.position, target.position);   //Try to face the target
            }
        }
    }

    void ChargeAttack()
    {
        rb.velocity = -transform.up * chargeSpeed;  //Charge forward
        //TurnToTarget(oldSelfPosition, chargePosition);      //Make sure to charge at "about" the chargePosition without turning back when charge past the target position

        //if (Vector2.Angle(-transform.up, chargePosition - oldSelfPosition) < 1)  //make sure only charge when facing at "about" the chargePosition
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

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Border"))   //To prevent player goes into border and let boss charge off
        {
            chargeStop = true;
        }
    }
    //Sometime when player quickly moves in and out of the ChargeScan hitbox boss won't attack, might be the update rate too slow

    //public void SetOldSelfPosition(Vector3 value) { oldSelfPosition = value; }
    //public void SetChargePosition(Vector3 value) { chargePosition = value; }
    public void SetChargeStart(bool value) { chargeStart = value; }
}
