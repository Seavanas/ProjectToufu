using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/* 
 * Contains informations about each enemy wave in the entire level
 * - waveWait: Times in second waited before this wave spawns
 * - enemySpawn: Object that represents each enemy in an enemy wave
 * - origin: The spawning location in the level
 */
[System.Serializable]
public class EnemySpawnWave
{
    /* 
     * Contains informations about each enemies in the enemy wave
     * - enemy: Object that represents each enemy
     * - xOffset, yOffset, rotationOffset: Offsets about the enemy relative to the origin point
     * - spawnWait: Times in second waited before this enemy spawns
     */
    [System.Serializable]
    public class EnemySpawn
    {
        public GameObject enemy;
        public Vector2 offset;
        public float rotationOffset;
        public float spawnWait; 
    }

    public float waveWait;
    public Transform origin;
    public EnemySpawn[] enemySpawnList;
}

public class EnemySpawnController : MonoBehaviour
{
    //Enemy groups stuff
    public GameObject enemySpawns;           //Reference to the in scene object that contains all the waves, uses its transform as the origin enemy spawn point
    private List<Transform> enemyWaves;     //List that stores each individual wave objects as a cell

    protected bool[] waveControl;
    public EnemySpawnWave[] enemySpawnWave;
    public event EventHandler waveEndsEventHandler;

    //Game states stuff
    private LevelController levelController;
    private bool defeat;

    void Start()
    {
        /*
        waveControl = new bool[enemySpawnWave.Length];
        */
        GameObject levelControllerObject = GameObject.FindWithTag("LevelController");
        levelController = levelControllerObject.GetComponent<LevelController>();
        levelController.defeatEventHandler += new EventHandler(this.playerDefeat);
        defeat = false;
        prepareEnemyWave();
        levelStart();
    }

    private void prepareEnemyWave()
    {
        enemyWaves = new List<Transform>();
        for (int i = 0; i < enemySpawns.transform.childCount; i++)       //Adding each wave into the list
            enemyWaves.Add(enemySpawns.transform.GetChild(i));

        for (int i = 0; i < enemyWaves.Count; i++)       //Deactivate all the waves
        {
            for (int j = 0; j < enemyWaves[i].childCount; j++)
                enemyWaves[i].GetChild(j).gameObject.SetActive(false);      //Deactivate all the groups in each wave

            enemyWaves[i].gameObject.SetActive(false);
        } 
    }

    public void levelStart()
    {
        //StartCoroutine(spawnEnemy());
        StartCoroutine(waveStart());
    }

    IEnumerator waveStart()
    {
        for (int i = 0; i < enemyWaves.Count; i++)       //Go through the child list of enemyGroup (name in unity)
        {
            enemyWaves[i].gameObject.SetActive(true);       //Activate the wave

            for (int j = 0; j < enemyWaves[i].childCount; j++)      //Go through the child list of wave (chlid list = "Group" in unity)
            {
                Transform enemyGroup = enemyWaves[i].GetChild(j);
                float yEnemySpawns = enemySpawns.transform.position.y;
                float yGroup = enemyGroup.position.y;
                float groupWait = enemyGroup.transform.localPosition.z;

                if (groupWait != 0)      //Using the z values of each group to get the second waited before group starts
                    yield return new WaitForSeconds(groupWait);
                
                enemyWaves[i].GetChild(j).position = new Vector2(enemyGroup.position.x, yEnemySpawns);      //Set the spawn point
                enemyWaves[i].GetChild(j).gameObject.SetActive(true);       //Activate the groups in the wave

                if (defeat)   //Check if player is out of lives
                    break;
            }
            yield return new WaitUntil(() => (GameObject.FindGameObjectWithTag("Enemy") == null));
        }
        waveEndsEventHandler(this, new EventArgs());            //Needs a few second for the event to register from levelcomplete script
        yield break;
    }
    /*
    IEnumerator spawnEnemy()
    {
        for (int i = 0; i < enemySpawnWave.Length; i++)       //Go through the list of enemySpawnWave
        {
            if (enemySpawnWave[i].waveWait != 0)      //Waiting for 0 second some how will not behave as 0 second
                yield return new WaitForSeconds(enemySpawnWave[i].waveWait);
            
            EnemySpawnWave.EnemySpawn[] enemySpawnList = enemySpawnWave[i].enemySpawnList;
            foreach (EnemySpawnWave.EnemySpawn enemySpawn in enemySpawnList)       //Go through each enemy(enemySpawn) in this enemy wave list(enemySpawnList)
            {
                if (defeat)   //Check if player is out of lives
                    break;

                if (enemySpawn.spawnWait != 0)      //Waiting for 0 second some how will not behave as 0 second
                    yield return new WaitForSeconds(enemySpawn.spawnWait);

                Instantiate(enemySpawn.enemy,
                            new Vector2(enemySpawnWave[i].origin.position.x + enemySpawn.offset.x,
                                    enemySpawnWave[i].origin.position.y + enemySpawn.offset.y),
                            enemySpawnWave[i].origin.transform.rotation * Quaternion.Euler(0f, 0f, enemySpawn.rotationOffset));
            }
        }
        waveEndsEventHandler(this, new EventArgs());
    }
    */

    private void playerDefeat(object sender, EventArgs e)
    {
        defeat = true;
    }
}