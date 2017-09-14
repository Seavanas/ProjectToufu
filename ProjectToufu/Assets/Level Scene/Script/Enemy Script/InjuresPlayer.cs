using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InjuresPlayer : MonoBehaviour {

    /*
     * SCRIPT INFO:
     * Checks upon collision if it hit player. If yes then reduce health by damageUponCollision
     * and set gameObject state to 1 if gameObject is an EnemyShot
     * If this gameobject is of tag EnemyShot, then also sets speed to 0 (through Mover.cs)
     * 
     * Please note that changing this will only change the damage of the object this is attached to. If you want to change damage of a ship's shots then
     * change the shots, not the damage for this.
     */

    //private LevelController levelController;
    public float damageUponCollision;
    
    void OnTriggerEnter2D(Collider2D other) {
        if (other.gameObject.CompareTag("Player") && !gameObject.CompareTag("Dead"))
        {
            //levelController.playerKilled(other.gameObject);
            other.GetComponent<BasePlayerScript>().TakeDamage(damageUponCollision);
            if (gameObject.CompareTag("EnemyShot"))
            {
                gameObject.tag = "Dead";
                gameObject.GetComponent<Animator>().SetInteger("State", 1);
                gameObject.GetComponent<Mover>().SetSpeed(0);
            }
        }
    }

    void AnimationEvent_Destroy()
    {
        Destroy(gameObject);
    }
}
