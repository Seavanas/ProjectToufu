using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwarmBossInfluence : MonoBehaviour
{


    /// <summary>
    /// Controls enemies that are inside the Circle of influence, such as attacks, organization, and movement
    /// </summary>
    public SwarmBoss SwarmBossScript;
    //public List<GameObject> EnemySwarm;
    public List<OrbitingEnemy> EnemySwarm;
    public bool StartFight = false;
    public float EnemyTurnAnglePerFrame, minimumRadius, EnemySpeed;
    List<PositionAngleBeforePoint> NonOrbitingEnemies;
    float TimeBeforeAttacking = 6;
    void Start()
    {
        EnemySwarm = new List<OrbitingEnemy>();
        NonOrbitingEnemies = new List<PositionAngleBeforePoint>();
        TimeBeforeAttacking += Time.time;
    }

    void FixedUpdate()
    {
        print(Vector2.SignedAngle(new Vector2(0,-1), GameObject.FindGameObjectWithTag("Player").transform.position - transform.position));

        List<OrbitingEnemy> NullObjects = new List<OrbitingEnemy>();
        foreach (OrbitingEnemy EnemySwarm1 in EnemySwarm)
        {
            if (EnemySwarm1.enemy != null)
            {
                VectorWithinRadius(EnemySwarm1);
            }
            else NullObjects.Add(EnemySwarm1);
        }
        foreach (OrbitingEnemy nullObj in NullObjects)
        {
            EnemySwarm.Remove(nullObj);
        }

        if (TimeBeforeAttacking < Time.time)
        {
            TimeBeforeAttacking = Time.time + Random.Range(0.2f,3);
            //float AngleToPlayer = (GameObject.FindGameObjectWithTag("Player").transform.position - transform.position) - 180;
            
            //NonOrbitingEnemies = SendWave(30 + 5*((int)Random.Range(0,8)), (int)Random.Range(3,7), );
        }

        foreach (PositionAngleBeforePoint enemy in NonOrbitingEnemies)
        {

            if (enemy.enemy != null)
            {
                //If desired angle to achieve is greater than 45 degrees away from current angle, then rotate towards the boss
                if (!enemy.TriggerOnceOnly && Mathf.Min(Mathf.Abs(enemy.enemy.transform.rotation.eulerAngles.z - enemy.angle), Mathf.Abs(enemy.enemy.transform.rotation.eulerAngles.z - enemy.angle - 360), Mathf.Abs(enemy.enemy.transform.rotation.eulerAngles.z - enemy.angle + 360)) > 12)
                {
                    //Debug.Log(Mathf.Min(Mathf.Abs(enemy.enemy.transform.rotation.eulerAngles.z - enemy.angle), Mathf.Abs(enemy.enemy.transform.rotation.eulerAngles.z - enemy.angle - 360), Mathf.Abs(enemy.enemy.transform.rotation.eulerAngles.z - enemy.angle + 360)));
                    if (enemy.CW)
                    {
                        if (enemy.enemy.transform.rotation.eulerAngles.z < enemy.angle)
                        {
                            enemy.enemy.transform.rotation = Quaternion.Euler(0, 0, Mathf.Clamp(enemy.angle, enemy.enemy.transform.rotation.eulerAngles.z + 360 - EnemyTurnAnglePerFrame, enemy.enemy.transform.rotation.eulerAngles.z + 360));
                        }
                        else
                            enemy.enemy.transform.rotation = Quaternion.Euler(0, 0, Mathf.Clamp(enemy.angle, enemy.enemy.transform.rotation.eulerAngles.z - EnemyTurnAnglePerFrame, enemy.enemy.transform.rotation.eulerAngles.z));
                    }
                    else
                    {
                        if (enemy.enemy.transform.rotation.eulerAngles.z >= enemy.angle)
                        {
                            enemy.enemy.transform.rotation = Quaternion.Euler(0, 0, Mathf.Clamp(enemy.angle + 360, enemy.enemy.transform.rotation.eulerAngles.z, enemy.enemy.transform.rotation.eulerAngles.z + EnemyTurnAnglePerFrame));
                        }
                        else
                            enemy.enemy.transform.rotation = Quaternion.Euler(0, 0, Mathf.Clamp(enemy.angle, enemy.enemy.transform.rotation.eulerAngles.z, enemy.enemy.transform.rotation.eulerAngles.z + EnemyTurnAnglePerFrame));
                    }
                }

                else
                {
                    enemy.TriggerOnceOnly = true;
                    //Calculates perpendicular line from angle at boss position, where slope is desired angle of enemy
                    int test = 0;
                    float Range = 0.02f*EnemySpeed;
                    //Equation of line, y = mx+b
                    do
                    {
                        Vector2 firstPos, secondPos;
                        if (enemy.angle != 0 && enemy.angle != 180)
                        {
                            float m = (Mathf.Tan((enemy.angle + 270) * Mathf.PI / 180));
                            float b = (enemy.endPosition.transform.position.y) - m * (enemy.endPosition.transform.position.x);
                            float a1 = (m * m + 1), b1 = 2 * (b - enemy.enemy.transform.position.y) * m - 2 * enemy.enemy.transform.position.x, c1 = Mathf.Pow((b - enemy.enemy.transform.position.y), 2) - (Range) * (Range) - Mathf.Pow(enemy.enemy.transform.position.x, 2);
                            float firstX = (-b1 + Mathf.Sqrt(b1 * b1 - 4 * a1 * c1)) / (2 * a1), secondX = (-b1 - Mathf.Sqrt(b1 * b1 - 4 * a1 * c1)) / (2 * a1);
                            //The x coordinates mark where the line going through boss intersects with a circle depicting where enemy can travel
                            firstPos = new Vector2(firstX, firstX * m + b);
                            secondPos = new Vector2(secondX, secondX * m + b);
                        }
                        else
                        {
                            float x = enemy.endPosition.transform.position.x;
                            float a1 = 1, b1 = -2 * enemy.enemy.transform.position.y, c1 = Mathf.Pow(enemy.enemy.transform.position.y, 2) - (Range * Range) + Mathf.Pow(x - enemy.enemy.transform.position.x, 2);
                            float firstY = (-b1 + Mathf.Sqrt(b1 * b1 - 4 * a1 * c1)) / (2 * a1), secondY = (-b1 - Mathf.Sqrt(b1 * b1 - 4 * a1 * c1)) / (2 * a1);
                            firstPos = new Vector2(x, firstY);
                            secondPos = new Vector2(x, secondY);
                        }

                        float angle1 = Vector2ToDegrees(firstPos - (Vector2)enemy.enemy.transform.position), angle2 = Vector2ToDegrees(secondPos - (Vector2)enemy.enemy.transform.position);
                        if ((angle1 >= 0 || angle1 < 0) && (angle2 >= 0 || angle2 < 0))
                        {
                            if (Mathf.Min(Mathf.Abs(enemy.enemy.transform.rotation.eulerAngles.z - angle1), Mathf.Abs(enemy.enemy.transform.rotation.eulerAngles.z - (angle1 + 360)), Mathf.Abs(enemy.enemy.transform.rotation.eulerAngles.z - (angle1 - 360))) < Mathf.Min(Mathf.Abs(enemy.enemy.transform.rotation.eulerAngles.z - angle2), Mathf.Abs(enemy.enemy.transform.rotation.eulerAngles.z - (angle2 + 360)), Mathf.Abs(enemy.enemy.transform.rotation.eulerAngles.z - (angle2 - 360))))
                            {
                                enemy.enemy.transform.rotation = Quaternion.Euler(0, 0, ClampAngle(angle1, enemy.enemy.transform.rotation.eulerAngles.z, EnemyTurnAnglePerFrame*2));
                            }
                            else
                            {
                                enemy.enemy.transform.rotation = Quaternion.Euler(0, 0, ClampAngle(angle2, enemy.enemy.transform.rotation.eulerAngles.z, EnemyTurnAnglePerFrame*2));
                            }
                            break;
                        }
                        Range += 0.1f;
                        test++;
                        if (test >= 1000)
                        {
                            Debug.Log("FUCK");
                            break;
                        }
                    } while (true);
                    
                }
                enemy.enemy.GetComponent<Rigidbody2D>().velocity = enemy.enemy.transform.up * EnemySpeed;
            }
        }


    }


    
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.GetComponent<Enemy1Script>() != null)//if other is an enemy1
        {
            foreach (OrbitingEnemy enemy1 in EnemySwarm)
            {
                if (enemy1.enemy == other.gameObject)
                {
                    return;
                }
            }
            EnemySwarm.Add(new OrbitingEnemy(other.gameObject, EnemySpeed));
            other.GetComponent<Mover>().enabled = false;
        }
    }

    //Returns closest desired angle less than or equal to 180
    float ClampAngle(float DesiredAngle, float CurrentAngle, float Offset)
    {
        if (Mathf.Abs(DesiredAngle - CurrentAngle) < Mathf.Abs(DesiredAngle - (CurrentAngle + 360)) && Mathf.Abs(DesiredAngle - CurrentAngle) < Mathf.Abs(DesiredAngle - (CurrentAngle - 360)))
        {
            return Mathf.Clamp(DesiredAngle, CurrentAngle - Offset, CurrentAngle + Offset);
        }
        else if (Mathf.Abs(DesiredAngle - (CurrentAngle + 360)) < Mathf.Abs(DesiredAngle - (CurrentAngle - 360)))
        {
            return Mathf.Clamp(DesiredAngle, (CurrentAngle + 360) - Offset, (CurrentAngle + 360) + Offset);
        }
        else
        {
            return Mathf.Clamp(DesiredAngle, (CurrentAngle - 360) - Offset, (CurrentAngle - 360) + Offset);
        }

    }
    //Turns an angle from 0 to 359 into a normal vector
    Vector2 DegreesToVector2(float angle)
    {
        double slope;
        if (angle != 0 && angle != 180)
        {
            slope = (Mathf.Tan((angle + 90) * Mathf.PI / 180));
            Vector2 AddTo;
            if (angle < 135 && angle > 45)
            {
                AddTo = (new Vector2(-10, (float)(slope * -10))).normalized;// x1 = -10, y1 = mx
            }
            else if (angle < 315 && angle > 225)
            {
                AddTo = (new Vector2(10, (float)(slope * 10))).normalized;

            }
            else if (angle <= 225 && angle >= 135)
            {
                AddTo = (new Vector2((float)(-10 / slope), -10)).normalized;
            }
            else
            {
                AddTo = (new Vector2((float)(10 / slope), 10)).normalized;
            }
            return AddTo;
        }
        else if (angle == 0)
        {
            return new Vector2(0, 1);
        }
        else return new Vector2(0, -1);
        
    }

    //Takes a vector (in the game) and turns it into an angle (that's relative to the game, cause 0 degrees isn't corresponding to proper mathematical standards)
    float Vector2ToDegrees(Vector2 vector)
    {
        float angle1;
        if (vector.x == 0)
        {
            if (vector.y > 0)
                angle1 = 0;
            else angle1 = 180;
        }
        else
        {
            angle1 = (Mathf.Atan(vector.y / vector.x) * 180 / Mathf.PI + 270) % 360;
            if (vector.x < 0)
            {
                if (vector.y != 0)
                    angle1 += 180;
                else angle1 = 90;
            }
        }
        
        return (angle1+360)%360;
    }

    List<PositionAngleBeforePoint> SendWave(float MaxAngle, float NumberOfShips, float AngleOffsetCCW)
    {
        List<PositionAngleBeforePoint> Send = new List<PositionAngleBeforePoint>();
        float CurrentAngle = MaxAngle;
        for (int i = 0; i < NumberOfShips; i++)
        {
            List<GameObject> enemiesInRange = new List<GameObject>();
            foreach (OrbitingEnemy enemy1 in EnemySwarm)
            {
                if (Vector2.Distance(enemy1.enemy.transform.position, gameObject.transform.position) <= minimumRadius + 1)
                    enemiesInRange.Add(enemy1.enemy);
            }
            if (enemiesInRange.Count != 0)
            {
                GameObject FoundShip = FindClosestAngle(CurrentAngle + AngleOffsetCCW, enemiesInRange);
                bool CW;

                Vector2 AddTo = DegreesToVector2(FoundShip.transform.rotation.eulerAngles.z + 45), AddTo2 = DegreesToVector2(FoundShip.transform.rotation.eulerAngles.z - 45);
                if (Vector2.Distance((Vector2)FoundShip.transform.position + AddTo, gameObject.transform.position) < (Vector2.Distance((Vector2)FoundShip.transform.position + AddTo2, gameObject.transform.position)))
                {
                    CW = false;
                }
                else CW = true;
                Send.Add(new PositionAngleBeforePoint(FoundShip, gameObject, (CurrentAngle + 180 + AngleOffsetCCW) % 360, CW));

                foreach (OrbitingEnemy enemy1 in EnemySwarm)
                {
                    if (enemy1.enemy == FoundShip)
                    {
                        EnemySwarm.Remove(enemy1);
                        break;
                    }
                }
            }
            CurrentAngle -= (MaxAngle / (NumberOfShips - 1) * 2);
        }
        return Send;
    }

    //Finds enemy1 in List that has the closest angle to Angle
    //List must be non-empty!
    GameObject FindClosestAngle(float Angle, List<GameObject> Enemies)
    {
        Angle = (Angle + 360) % 360;
        GameObject answer = Enemies[0];
        //finds distance between enemy angle and desired Angle
        float minAngle = Mathf.Min(Mathf.Abs(answer.transform.rotation.eulerAngles.z - Angle), Mathf.Abs(answer.transform.rotation.eulerAngles.z + 360 - Angle), Mathf.Abs(answer.transform.rotation.eulerAngles.z - 360 - Angle));
        foreach (GameObject enemy1 in Enemies)
        {
            float minAngle2 = Mathf.Min(Mathf.Abs(enemy1.transform.rotation.eulerAngles.z - Angle), Mathf.Abs(enemy1.transform.rotation.eulerAngles.z + 360 - Angle), Mathf.Abs(enemy1.transform.rotation.eulerAngles.z - 360 - Angle));            if (minAngle > minAngle2)
            {
                answer = enemy1;
                minAngle = minAngle2;
            }
        }
        return answer;
    }


    /*
     * Sets angle of enemy1 so that it flies within a radius of gameObject while not flying too far.
     * If enemy flies at a faster speed, then enemy should try to curve in more so it doesn't overshoot the radius, for example.
     * 
     */
    void VectorWithinRadius(OrbitingEnemy enemy1)
    {
        
        //Angle in Degrees to add to current angle
        float AngleToAdd = ((180 / Mathf.PI) * Mathf.Acos((Mathf.Pow(minimumRadius, 2) - Mathf.Pow(Vector2.Distance(gameObject.transform.position, enemy1.enemy.transform.position), 2) - Mathf.Pow(EnemySpeed / 4, 2)) / (-2 * EnemySpeed / 4 * Vector2.Distance(gameObject.transform.position, enemy1.enemy.transform.position))));
        float DesiredAngle;
        if (AngleToAdd > 0 || AngleToAdd <= 0)//if AngleToAdd is a number
        {
            //Current Angle in Degrees from enemy1 pointing to gameObject, Ie. Angle that makes enemy1 point to gameObject if you set enemy1's z rotation
            float AngleCurrent = (((Mathf.Atan2((gameObject.transform.position - enemy1.enemy.transform.position).y, (gameObject.transform.position - enemy1.enemy.transform.position).x)) / Mathf.PI * 180 - 90) + 360) % 360;

            //Picks the smallest angle to turn ship around (AngleCurrent - AngleToAdd or AngleCurrent + AngleToAdd) 
            if (Mathf.Min(Mathf.Abs(((AngleToAdd + AngleCurrent + 360) % 360) - enemy1.enemy.transform.rotation.eulerAngles.z), Mathf.Abs(((AngleToAdd + AngleCurrent + 360) % 360) - 360 - enemy1.enemy.transform.rotation.eulerAngles.z))
                <
                Mathf.Min(Mathf.Abs(((-AngleToAdd + AngleCurrent + 360) % 360) - enemy1.enemy.transform.rotation.eulerAngles.z), Mathf.Abs(((-AngleToAdd + AngleCurrent + 360) % 360) + 360 - enemy1.enemy.transform.rotation.eulerAngles.z)))
            {
                DesiredAngle = (AngleToAdd + AngleCurrent);
            }
            else
            {
                DesiredAngle = (AngleCurrent - AngleToAdd);
            }

            if (Mathf.Abs(enemy1.enemy.transform.rotation.eulerAngles.z - (DesiredAngle)) <= Mathf.Abs(enemy1.enemy.transform.rotation.eulerAngles.z - (DesiredAngle + 360)) && Mathf.Abs((enemy1.enemy.transform.rotation.eulerAngles.z - (DesiredAngle))) <= Mathf.Abs(enemy1.enemy.transform.rotation.eulerAngles.z - (DesiredAngle - 360)))
            {
                enemy1.enemy.transform.rotation = Quaternion.Euler(0, 0, Mathf.Clamp(DesiredAngle, enemy1.enemy.transform.rotation.eulerAngles.z - EnemyTurnAnglePerFrame, enemy1.enemy.transform.rotation.eulerAngles.z + EnemyTurnAnglePerFrame));
            }
            else if (Mathf.Abs(enemy1.enemy.transform.rotation.eulerAngles.z - (DesiredAngle + 360)) <= Mathf.Abs(enemy1.enemy.transform.rotation.eulerAngles.z - (DesiredAngle - 360)))
                enemy1.enemy.transform.rotation = Quaternion.Euler(0, 0, Mathf.Clamp(DesiredAngle, enemy1.enemy.transform.rotation.eulerAngles.z - (EnemyTurnAnglePerFrame + 360), enemy1.enemy.transform.rotation.eulerAngles.z - 360 + EnemyTurnAnglePerFrame));
            else enemy1.enemy.transform.rotation = Quaternion.Euler(0, 0, Mathf.Clamp(DesiredAngle, enemy1.enemy.transform.rotation.eulerAngles.z + 360 - (EnemyTurnAnglePerFrame), enemy1.enemy.transform.rotation.eulerAngles.z + 360 + EnemyTurnAnglePerFrame));

            float EnemySpeed1 = this.EnemySpeed + (enemy1.Seed / (8000f));
            enemy1.enemy.GetComponent<Rigidbody2D>().velocity  = enemy1.enemy.transform.up * EnemySpeed1;
        }
    }


    //Returns the closest perpendicular vector from (gameobject to enemy1)
    Vector2 GetPerpendicular(GameObject enemy1)
    {
        Vector2 enemy1Vector = enemy1.transform.position - gameObject.transform.position;
        Vector2 Option1;
        if (enemy1Vector.y != 0)
        {
            Option1 = new Vector2(1, -enemy1Vector.x / enemy1Vector.y);
        }
        else { Option1 = new Vector2(-enemy1Vector.y / enemy1Vector.x, 1); }
        //Option1 is now a perpendicular vector to the one connecting enemy1 and gameObject

        if (Vector2.Distance(enemy1.GetComponent<Rigidbody2D>().velocity, Option1) <= (Vector2.Distance(enemy1.GetComponent<Rigidbody2D>().velocity, -Option1)))
        {
            return Option1;
        }
        else
        {
            return -Option1;
        }
    }
}
public class PositionAngleBeforePoint
{
    public GameObject enemy;//object that is moving
    public GameObject endPosition;//position to reach
    public float angle;//Angle in Degrees
    public bool CW;//Should ship spin clockwise or counterclockwise first?
    public bool TriggerOnceOnly = false;
    public PositionAngleBeforePoint(GameObject enemy, GameObject endPosition, float angle, bool CW)
    {
        this.enemy = enemy;
        this.endPosition = endPosition;
        this.angle = angle;
        this.CW = CW;
    }
    public PositionAngleBeforePoint(GameObject enemy, float angle)
    {
        this.enemy = enemy;
        this.angle = angle;
    }
}

public class OrbitingEnemy
{
    public GameObject enemy;
    public int Seed;
    public float Speed;

    public OrbitingEnemy(GameObject enemy, float speed)
    {
        this.enemy = enemy;
        this.Speed = speed;

        Seed = Random.Range(0, 9999);
    }


}