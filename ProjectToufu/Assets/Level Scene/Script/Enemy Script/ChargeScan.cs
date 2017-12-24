using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChargeScan : MonoBehaviour {

    void OnTriggerStay2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            GetComponentInParent<SpikeShield>().chargePosition = other.transform.position;  //Set the position to charge, use transform will use reference instead of values
            GetComponentInParent<SpikeShield>().targetFound = true;         //Begin charging
        }
    }
}
