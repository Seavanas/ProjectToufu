using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class L1LevelComplete : MonoBehaviour {
    //Spawning enemy stuff
    private EnemySpawnController enemySpawnController;

    //Player stuff
    public Transform cutScenePoint;
    public float cutSceneSpeed;

    //Game states stuff
    private LevelController levelController;
    private bool defeat;
    private bool victory;
    private bool waveEnds;

    void Start () {
        victory = false;
        waveEnds = false;

        GameObject levelControllerObject = GameObject.FindWithTag("LevelController");
        levelController = levelControllerObject.GetComponent<LevelController>();
        levelController.defeatEventHandler += new EventHandler(this.playerDefeat);

        GameObject enemySpawnControllerObject = GameObject.FindWithTag("EnemySpawnController");
        enemySpawnController = enemySpawnControllerObject.GetComponent<EnemySpawnController>();
        enemySpawnController.waveEndsEventHandler += new EventHandler(this.waveEndsMethod);
    }
	
	void Update () {
        if ((waveEnds) &&
            (GameObject.FindGameObjectWithTag("Enemy") == null) &&
            !defeat)
        {
            print("you win");
            GameObject playerObject = GameObject.FindWithTag("Player");
            //playerObject.GetComponent<PlayerController>().enabled = false;            this casues bug
            playerObject.transform.position = Vector2.MoveTowards(playerObject.transform.position, cutScenePoint.position, cutSceneSpeed);

            //sometimes the playerobject is killed or not found, need to fix
        }
    }

    private void playerDefeat(object sender, EventArgs e)
    {
        defeat = true;
        //Maybe do defeat screen here? so each level can have different defeat screen
    }

    private void waveEndsMethod(object sender, EventArgs e)
    {
        waveEnds = true;
    }
}
