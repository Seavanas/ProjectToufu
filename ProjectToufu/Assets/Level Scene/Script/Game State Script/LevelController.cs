using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelController : MonoBehaviour {
    //Player stuff
    public Transform playerSpawnPoint;
    public GameObject playerShip;
    public float playerLives;
    public float playerRespawnWait;
    public float playerLiveDecreaseAmount;

    //Hud stuff
    private HUDFacade hud;

    //Game state stuff
    public event EventHandler defeatEventHandler;
    public GameObject defeatCanvas;
    public float defeatScreenWait;

    void Start() {
        GameObject hudObject = GameObject.FindWithTag("Hud");
        hud = hudObject.GetComponent<HUDFacade>();
        hud.SetLives(playerLives);
        spawnPlayer();
        //GameObject.FindWithTag("EnemySpawnController").GetComponent<EnemySpawnController>().levelStart();
    }

    void spawnPlayer()
    {
        Instantiate(playerShip, playerSpawnPoint.position, playerSpawnPoint.rotation);
    }

    public void playerKilled(GameObject player) {
        hud.AddLive(-playerLiveDecreaseAmount);
        if (hud.LivesText.GetComponent<LivesScript>().lives > 0)
        {
            Invoke("spawnPlayer", playerRespawnWait);
        }
        else if (hud.LivesText.GetComponent<LivesScript>().lives < 1)
        {
            defeatEventHandler(this, new EventArgs());
            Invoke("spawnDefeatCanvas", defeatScreenWait);
        }
        if (player.GetComponent<Animator>() != null)
        {
            player.GetComponent<Animator>().SetInteger("State", 1);//Set destroy animation to player
            player.tag = "Dead";
        }
    }

    void spawnDefeatCanvas()
    {
        Instantiate(defeatCanvas);
    }
}
