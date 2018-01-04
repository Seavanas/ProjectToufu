using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.UI;

public class DebriefPanelScript : MonoBehaviour {
    private bool activateOnce = false;
    public GameObject enemyIconPrefab, contentBox;
    private float SpawnPrefabHereX = -100, SpawnPrefabHereY = -100;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if (!activateOnce)
        {
            activateOnce = true;
            SaveInfoObject temp = SaveInfo.saveInfo.SaveObject;
            //if (temp.EnemiesKilledPrevious != null)
            {
                //Dictionary<string, int> lastlevel = temp.EnemiesKilledPrevious;
                //foreach (KeyValuePair<string, int> prefab in lastlevel)
                {
                    //Debug.Log("Enemy1");
                    GameObject prefabObject = GameObject.Find("Enemy1");
                    Vector2 position = new Vector2(SpawnPrefabHereX, SpawnPrefabHereY);
                    GameObject created = Instantiate(enemyIconPrefab, position, Quaternion.identity);
                    created.transform.parent = contentBox.transform;
                    created.transform.localPosition = position;
                    created.GetComponent<Image>().sprite = prefabObject.GetComponent<SpriteRenderer>().sprite;
                    SpawnPrefabHereY -= 50;
                }
            }
        }
	}
}
