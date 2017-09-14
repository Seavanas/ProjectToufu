using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TankSpikeBoss : MonoBehaviour
{
    //Attack
    public GameObject shot;
    public Transform shotSpawn;
    public int shotAmount;
    public int shotSpeed;
    public float shotAngle;
    public float fireRate;
    public float chargeTimer;
    private float nextFire;
    private float nextCharge;

    //Movement stuff
    public Transform rightXPosition;        //rightXPosition & leftXPosition allow to move left and right constantly
    public Transform leftXPosition;
    public Transform bottomYPosition;       //Ideally the Y that the charge attack will be charge into before it returns back
    public float MoveSpeed;
    public float chargeSpeed;
    private Rigidbody2D rb;
    private GameObject target;
    private bool moveRight;

    //Stats
    public BaseEnemyScript baseEnemyScript;
    public float chargeWidth;        //enemy.x - chargeWidth <= target.x <= enemy.x + chargeWidth  ----> enemy will be charging
    public float chargeHeight;      //target.y <= enemy.y - chargeHeight ----> enemy will be charging
    private bool charging;    //T: Aim and charge, F: Spray shoot(Every at once, Every gradually, Only front, Only left & right sides)

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        baseEnemyScript = GetComponent<BaseEnemyScript>();
        shot.GetComponent<Mover>().SetSpeed(shotSpeed);
        moveRight = 0 == Random.Range(0, 2);
    }

    void Update()
    {
        if (baseEnemyScript.health <= 0)
        {
            baseEnemyScript.scoreAdd();
            Destroy(gameObject);
        }
        else
        {
            target = GameObject.FindGameObjectWithTag("Player");
            if (((Time.time > nextCharge) && TargetInChargeRange()) || charging)        //Charing attack
            {
                print("Hella charge");
                charging = true;
                MoveTo(new Vector2(transform.position.x, bottomYPosition.position.y), chargeSpeed);
                if (transform.position.y == bottomYPosition.position.y)
                {
                    charging = false;
                    nextCharge = Time.time + chargeTimer;   //Timer starts ticking once the charge is over (meaning that it starts counting once the enemy is at the bottom)
                }
            }
            else        //Shooting attack
            {
                print("Hella " + moveRight);
                if (transform.position.x == rightXPosition.position.x)      //If reached the right side
                    moveRight = false;
                else if (transform.position.x == leftXPosition.position.x)  //If reached the left side
                    moveRight = true;

                if (transform.position.y != rightXPosition.position.y)      //If not at the top
                    MoveTo(new Vector2(transform.position.x, rightXPosition.position.y), MoveSpeed);    //Move to top without moving left or right
                else
                {
                    if (moveRight)
                        MoveTo(new Vector2(rightXPosition.position.x, rightXPosition.position.y), MoveSpeed / 2);     //Move to right side
                    else
                        MoveTo(new Vector2(leftXPosition.position.x, leftXPosition.position.y), MoveSpeed / 2);       //Move to left side

                    if ((Time.time > nextFire))     //Shooting
                    {
                        AttackTargetDirection();
                        nextFire = Time.time + fireRate;
                    }
                }
            }
        }
    }

    private void MoveTo(Vector2 destination, float speed)
    {
        transform.position = Vector2.MoveTowards(transform.position, destination, speed);
    }

    //Can be improve using Anthony's range detector thing
    private void AttackTargetDirection()
    {
        if (target.transform.position.y < shotSpawn.transform.position.y)       //If target is at front
            SprayShoot(180);
        else if (target.transform.position.x < shotSpawn.transform.position.x)  //If target is on left
            SprayShoot(90);
        else if (target.transform.position.x > shotSpawn.transform.position.x)  //If target is on right
            SprayShoot(270);
        else
            SprayShoot(180);
    }

    private void SprayShoot(float originAngle)
    {
        float startAngle;
        if (shotAmount % 2 == 0)     //If shotAmount is even
        {
            startAngle = originAngle - (shotAngle * (Mathf.Floor(shotAmount / 2) - 1)) - (shotAngle / 2);
        }
        else
            startAngle = originAngle - (shotAngle * Mathf.Floor(shotAmount / 2));

        for (int i = 0; i < shotAmount; i++)
            Instantiate(shot, shotSpawn.position, Quaternion.Euler(0, 0, startAngle + shotAngle * i));
    }

    //Check if the target is in front and can be charge
    //Can be improve using Anthony's range detector thing
    private bool TargetInChargeRange()
    {
        float minChargeRange = transform.position.x - chargeWidth;
        float maxChargeRange = transform.position.x + chargeWidth;
        if ((transform.position.y - chargeHeight < target.transform.position.y) ||      //If target is not in front far enough
            (target.transform.position.x < minChargeRange) ||                           //If target is outside the min width
            (maxChargeRange < target.transform.position.x))                             //If target is outside the max width
                return false;
        
        return true;
    }
}
