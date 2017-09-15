using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwarmBoss : MonoBehaviour {

    public SwarmBossInfluence InfluenceScript;

	// Use this for initialization
	void Start () {
        //test
	}
	
	// Update is called once per frame
	void Update () {
        SetRadius();
        
	}


    //Sets orbiting radius for the swarm
    void SetRadius()
    {
        if (InfluenceScript.EnemySwarm.Count <= 20)
        {
            InfluenceScript.minimumRadius = 3;
        }
        else
        {
            InfluenceScript.minimumRadius = ((Mathf.Log((InfluenceScript.EnemySwarm.Count)))-3) / 2 + 3;
        }
    }
}
