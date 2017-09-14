using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InjuresEnemy : MonoBehaviour {
    /*
     * Changelog: No longer uses levelController, instead decrements health 
     * 
     * 
     * Info: Causes (damage) amount of damage to objects with "Enemy" tag.
     * If this script is attached to a "Shot" tag, sets State to 1 and set Mover component to 0
     * 
     * 
     * 
     */

    public float damage; //damage caused to other object upon collision
    private bool TriggerOnceOnly = false;
    void OnTriggerEnter2D(Collider2D other) {
        if (other.gameObject.CompareTag("Enemy") && !TriggerOnceOnly) {
            other.GetComponent<BaseEnemyScript>().TakeDamage(damage);
            TriggerOnceOnly = true;

            if (gameObject.CompareTag("Shot"))
            {
                GetComponent<Animator>().SetInteger("State", 1);
                GetComponent<Mover>().SetSpeed(0);
                //Destroy(this);
            }
        }
    }

    void AnimationEvent_Destroy()
    {
        Destroy(gameObject);
    }
}
