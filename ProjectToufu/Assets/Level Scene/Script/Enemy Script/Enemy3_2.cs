using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy3_2 : MonoBehaviour
{
    public GameObject ParentReference;
    // Use this for initialization
    void Start()
    {

    }
    
    void LateUpdate()
    {
        gameObject.transform.rotation = Quaternion.identity;
    }

    //catches incoming shots and lets Parent class know
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Shot")
        {
            ParentReference.GetComponent<Enemy3>().IncomingShot(other.gameObject);
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.tag == "Shot")
        {
            ParentReference.GetComponent<Enemy3>().ExitingShot(other.gameObject);
        }
    }
}
