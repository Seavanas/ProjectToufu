using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy1Script : MonoBehaviour {

    // Use this for initialization
    void Start()
    {
        baseEnemyScript = GetComponent<BaseEnemyScript>();
    }
    private BaseEnemyScript baseEnemyScript;
    private bool triggerOnceOnly = false;

	// Update is called once per frame
	void Update () {
        if (baseEnemyScript.health <= 0)
        {
            GetComponent<Animator>().SetInteger("State", 1);
            if (!triggerOnceOnly)
            {
                baseEnemyScript.scoreAdd();
                triggerOnceOnly = true;
            }
            
        }
            
	}
}
