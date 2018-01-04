using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwarmBoss : MonoBehaviour {

    public SwarmBossInfluence InfluenceScript;
    public BaseBossScript BaseBossScript;
    public BaseEnemyScript BaseEnemyScript;
    private float CurrentHealth;
    private float TopOfScreenYCoord = 11f, SpawnX1 = 0.4f, SpawnX2 = 1.2f, SpawnX3 = 3.2f;

    private WaitForSeconds ShortDelay = new WaitForSeconds(0.4f);

    private Coroutine SpawnPattern1;

    public GameObject enemy1Prefab, enemy2Prefab;
	// Use this for initialization
	void Start () {
        CurrentHealth = BaseEnemyScript.health;
        BaseBossScript.InitiateHUD();
    }
	
    
	// Update is called once per frame
	void Update () {
        if (CurrentHealth != BaseEnemyScript.health)
        {
            BaseBossScript.SetHealth(BaseEnemyScript.health / BaseEnemyScript.GetMaxHealth());
            CurrentHealth = BaseEnemyScript.health;
        }

        if (InfluenceScript.EnemySwarm.Count <= 40 && SpawnPattern1 == null)
        {
            SpawnPattern1 = StartCoroutine(SpawnEnemy1());
        }
        SetRadius();
    }

    
    IEnumerator SpawnEnemy1()
    {
        Instantiate(enemy1Prefab, new Vector2(transform.position.x, TopOfScreenYCoord), Quaternion.Euler(0,0,180));
        yield return ShortDelay;

        Instantiate(enemy1Prefab, new Vector2(SpawnX1 + transform.position.x, TopOfScreenYCoord), Quaternion.Euler(0,0,180));
        Instantiate(enemy1Prefab, new Vector2(-SpawnX1 + transform.position.x, TopOfScreenYCoord), Quaternion.Euler(0,0,180));
        yield return ShortDelay;

        Instantiate(enemy1Prefab, new Vector2(SpawnX2 + transform.position.x, TopOfScreenYCoord), Quaternion.Euler(0,0,180));
        Instantiate(enemy1Prefab, new Vector2(-SpawnX2 + transform.position.x, TopOfScreenYCoord), Quaternion.Euler(0,0,180));
        yield return ShortDelay;

        Instantiate(enemy1Prefab, new Vector2(SpawnX3 + transform.position.x, TopOfScreenYCoord), Quaternion.Euler(0,0,180));
        Instantiate(enemy1Prefab, new Vector2(-SpawnX3 + transform.position.x, TopOfScreenYCoord), Quaternion.Euler(0,0,180));
        SpawnPattern1 = null;
        yield break;
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
