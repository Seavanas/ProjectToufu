using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shot1Script : MonoBehaviour
{
    void Start()//Sets damage equal to the save object
    {
        this.GetComponent<InjuresEnemy>().damage = SaveInfo.saveInfo.SaveObject.MainAttackStrength;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Enemy"))
            GetComponent<Mover>().SetSpeed(0);
    }

   
}