using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Missile1Script : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    void AnimationEvent_Destroy()//Runs when animation event gets called by Missile2 Animation, and destroys object
    {
        Destroy(gameObject);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Enemy"))
            GetComponent<Mover>().SetSpeed(0);
    }

}
