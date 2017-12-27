using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChargeScan : MonoBehaviour {

    void OnTriggerStay2D(Collider2D other)      //On enter won't work, will sometime stop charging 
    {
        if (other.CompareTag("Player") && !GetComponentInParent<SpikeShield>().chargeStart)      //Avoid changing direction when the charge attack is in progress
        {
            GetComponentInParent<SpikeShield>().oldSelfPosition = GetComponentInParent<SpikeShield>().transform.position;
            GetComponentInParent<SpikeShield>().chargePosition = other.transform.position;  //Set the position to charge, use transform will use reference instead of values
            GetComponentInParent<SpikeShield>().chargeStart = true;         //Begin charging
        }
    }
}
