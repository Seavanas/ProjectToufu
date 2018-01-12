using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class BaseEnemyScript : MonoBehaviour {

    /*
     * Contains health, score, and scoreBoard.
     * 
     * Meant to reduce code inside every unique enemyscript, as well as provide information that other objects can use (Ex: Shot collisions can reduce health)
     * 
     */

    public float health, score;
    private float maxHealth;
    private HUDFacade scoreBoard;

    void Start()
    {
        GameObject scoreObject = GameObject.FindWithTag("Hud");
        scoreBoard = scoreObject.GetComponent<HUDFacade>();
        maxHealth = health;
    }
    

    //Takes damage, and if health <= 0 then changes tag so that it doesn't interfere with other objects
	public void TakeDamage(float damage)
    {
        health -= damage;
        if (health <= 0)
        {
            gameObject.tag = "Dead";
            if (PrefabUtility.GetPrefabParent(this) != null)
            {
                string prefabName = PrefabUtility.GetPrefabParent(this).name;//gets name of prefab
                if (LevelController.levelController.EnemiesKilled.ContainsKey(prefabName))
                {
                    LevelController.levelController.EnemiesKilled[prefabName]++;
                }
                else LevelController.levelController.EnemiesKilled.Add(prefabName, 1);

                //Debug.Log(LevelController.levelController.EnemiesKilled[prefabName] + prefabName);
            }
        }
    }

    void AnimationEvent_Destroy()
    {
        Destroy(gameObject);
    }

    public void scoreAdd()
    {
        scoreBoard.AddScore(score);
    }

    public float GetMaxHealth()
    {
        return maxHealth;
    }
}
