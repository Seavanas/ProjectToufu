using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Enemy3 : MonoBehaviour {

    //private static GameObject[] ListOfEnemies = new GameObject[10];
    public float Speed, AccelerationPerFrame;

    private BaseEnemyScript baseEnemyScript;
    private GameObject PlayerReference;
    private bool TriggerOnlyOnce = false;

    public float Diameter;
    public float SafetyBarrierDistance;

    private int MaxRecursion = 500;

    public List<GameObject> ListOfShots;

    private Vector2 dodgeVelocity;

    bool deaded = false;
	// Use this for initialization
	void Start () {
        PlayerReference = GameObject.FindGameObjectWithTag("Player");
        baseEnemyScript = GetComponent<BaseEnemyScript>();
        ListOfShots = new List<GameObject>();
        
	}
	
	// Update is called once per frame
	void Update () {
        if (baseEnemyScript.health <= 0)
        {
            if (!TriggerOnlyOnce)
            {
                GetComponent<Rigidbody2D>().velocity = new Vector2(0, 0);
                GetComponent<Animator>().SetInteger("State", 1);
                baseEnemyScript.scoreAdd();
                gameObject.transform.localScale *= 3.2f;
                TriggerOnlyOnce = true;
            }
        }
        
        else
        {
            if (PlayerReference == null)
            {
                PlayerReference = GameObject.FindGameObjectWithTag("Player");
                if (PlayerReference == null)
                {
                    return;
                }
            }
                
            gameObject.GetComponent<Rigidbody2D>().velocity = Movement();

            if (gameObject.GetComponent<Rigidbody2D>().velocity != new Vector2(0, 0))
                gameObject.transform.rotation = Quaternion.Euler(0, 0, Mathf.Atan2(gameObject.GetComponent<Rigidbody2D>().velocity.normalized.y, gameObject.GetComponent<Rigidbody2D>().velocity.normalized.x) * 180 / Mathf.PI + 90);
            else gameObject.transform.rotation = Quaternion.identity;
            //Debug.Log(gameObject.GetComponent<Rigidbody2D>().velocity.normalized.y + "," + gameObject.GetComponent<Rigidbody2D>().velocity.normalized.x);
            //Debug.Log(Mathf.Atan2(gameObject.GetComponent<Rigidbody2D>().velocity.normalized.y, gameObject.GetComponent<Rigidbody2D>().velocity.normalized.x) * 180 / Mathf.PI);
        }
        
	}


    //triggers if a shot capable of damaging the enemy comes into the range
    public void IncomingShot(GameObject shot)
    {
        if (baseEnemyScript.health <= 0)
            return;
        ListOfShots.Add(shot);
        RemoveNull();

        if (ListOfShots.Count > 1)
            SortShots(0);
        dodgeVelocity = CalculateBest();
    }
    public void ExitingShot(GameObject shot)
    {
        if (baseEnemyScript.health <= 0)
            return;
        ListOfShots.Remove(shot);
        RemoveNull();

        if (ListOfShots.Count > 1)
            SortShots(0);
        dodgeVelocity = CalculateBest();
    }

    public void RemoveNull()
    {
        bool allRemoved = false;
        while (!allRemoved)
        {
            allRemoved = true;
            for (int i = 0; i < ListOfShots.Count; i++)
            {
                if (ListOfShots[i] == null)
                {
                    ListOfShots.RemoveAt(i);
                    allRemoved = false;
                    break;
                }
            }

        }
    }
    


    //Sorts the shots in the list (ListOfShots)
    public void SortShots(int IterationCount)
    {

        if (IterationCount > MaxRecursion)
        {
            deaded = true;
        }
        if (IterationCount + 500 > MaxRecursion)
        {
            return;
        }
        if (deaded)//TEST
        {
            string asd = "";
            foreach (GameObject asdf in ListOfShots)
            {
                asd += Vector2.Distance(asdf.transform.position, transform.position);
                asd += ", ";
            }
            Debug.Log(asd);
        }
        


        bool sorted = true;
        float distanceOfPrevious = 0;
        foreach (GameObject shot in ListOfShots)
        {
            if (shot == null)
            {
                Debug.Log("SHOT IS NULL");
                return;
            }
                
            if (distanceOfPrevious > Vector2.Distance(shot.transform.position, transform.position))
            {


                sorted = false;
                float distance = Vector2.Distance(shot.transform.position, transform.position);
                int index = 1;

                //puts misplaced shot in the right place
                foreach (GameObject shot1 in ListOfShots)
                {
                    if (Vector2.Distance(shot1.transform.position, transform.position) > distance)
                    {
                            ListOfShots.Remove(shot);
                            ListOfShots.Insert(index - 1, shot);
                            break;
                    }
                    index++;
                }
                break;
            }
            else distanceOfPrevious = Vector2.Distance(shot.transform.position, transform.position);
        }

        if (sorted == false)
        {
            SortShots(++IterationCount);
        }
        
    }




    public Vector2 Movement()
    {
        //Vector2 ToPlayer = (PlayerReference.transform.position - gameObject.transform.position).normalized;
        Vector2 DesiredVelocity;
        if (dodgeVelocity != new Vector2(0, 0))
        {
            //Adds velocity to dodge object multiplied by acceleration to the current velocity
            DesiredVelocity = (gameObject.GetComponent<Rigidbody2D>().velocity.normalized + dodgeVelocity * AccelerationPerFrame).normalized * Speed;
        }
        //else adds velocity to (velocity towards player) multiplied by acceleration
        else if (PlayerReference != null) DesiredVelocity = (gameObject.GetComponent<Rigidbody2D>().velocity.normalized + (Vector2)(PlayerReference.transform.position - gameObject.transform.position).normalized * AccelerationPerFrame).normalized * Speed;

        else DesiredVelocity = (gameObject.GetComponent<Rigidbody2D>().velocity.normalized + new Vector2(0, -1) * AccelerationPerFrame);
        return DesiredVelocity;

    }









    //Finds a path to go given a list of shots (ListOfShots), returns a unit vector
    /*
     * Finds highest priority shot whose trajectory is aimed at it, then chooses a direction perpendicular to its velocity
     * to move away. Picks direction such that it does not go towards the closest current shot.
     * 
     * Assume that top of list of shots is closest shot, and assume that as time passes, each shot still in the list does not
     * change order of closest or not
     */
    public Vector2 CalculateBest()
    {
        if (ListOfShots.Count == 0)
            return new Vector2(0, 0);
        Vector2 option1 = new Vector2(0,0);
        foreach (GameObject shot in ListOfShots)
        {
            if (WithinTrajectory(shot))
            {
                if (shot.GetComponent<Rigidbody2D>().velocity.x == 0)
                {
                    option1 = new Vector2(1, 0);
                }
                else option1 = new Vector2(-shot.GetComponent<Rigidbody2D>().velocity.y/shot.GetComponent<Rigidbody2D>().velocity.x, 1).normalized;
            }
            break;
        }
        //if (option1 == new Vector2(0, 0))
            //return option1;

        if (Vector2.Distance((Vector2)ListOfShots[0].transform.position + ListOfShots[0].GetComponent<Rigidbody2D>().velocity, option1 + (Vector2)gameObject.transform.position) > Vector2.Distance((Vector2)ListOfShots[0].transform.position + ListOfShots[0].GetComponent<Rigidbody2D>().velocity, -option1 + (Vector2)gameObject.transform.position))
            return option1;
        return -option1;
    }

    //Math is all good, checks to see if an incoming shot will hit, assuming the shot velocity does not change its trajectory
    bool WithinTrajectory(GameObject shot)
    {
        Vector2 newPosition = gameObject.transform.position - shot.transform.position;
        if (shot.GetComponent<Rigidbody2D>().velocity.x != 0)
        {
            //Debug.Log(shot.GetComponent<Rigidbody2D>().velocity.y + "," + shot.GetComponent<Rigidbody2D>().velocity.x);
            //Debug.Log(newPosition.x + "," + newPosition.y);
            float slope = shot.GetComponent<Rigidbody2D>().velocity.y/ shot.GetComponent<Rigidbody2D>().velocity.x;
            if (-Mathf.Pow(newPosition.x,2)*Mathf.Pow(slope,2) + 2*(newPosition.x)*(newPosition.y)*slope - Mathf.Pow(newPosition.y,2) + Mathf.Pow(slope, 2)*Mathf.Pow(Diameter/2 + SafetyBarrierDistance, 2) + Mathf.Pow(Diameter/2 + SafetyBarrierDistance, 2)  >= 0) 
            {
                return true;
            }

            return false;
        }
        if (Mathf.Pow((Diameter / 2 + SafetyBarrierDistance), 2) - Mathf.Pow(newPosition.x, 2) >= 0)
            return true;
        return false;
        
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Shot")
            ListOfShots.Remove(other.gameObject);
        else if (other.tag == "Player")
        {
            baseEnemyScript.health = 0;
        }
    }
}
