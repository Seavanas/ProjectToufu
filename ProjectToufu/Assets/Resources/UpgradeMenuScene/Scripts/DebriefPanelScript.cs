using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.UI;

public class DebriefPanelScript : MonoBehaviour {
    private bool activateOnce = false;
    public GameObject enemyIconPrefab, contentBox;
    private float SpawnPrefabHereX = 60, SpawnPrefabHereY = -100;
    public float SecondsToWait;
	// Use this for initialization
	void Start ()
    {//TEST
        /*Dictionary<string, int> temp = SaveInfo.saveInfo.SaveObject.EnemiesKilledPrevious;
        temp.Add("Enemy1", 1);
        temp.Add("Enemy2", 1);
        temp.Add("Enemy3", 1);
        temp.Add("Agile Enemy", 1);
        temp.Add("Double Shot Enemy", 1);
        temp.Add("Multi Shot Enemy", 1);*/
	}
	
    void OnEnable()
    {
        Debug.Log("Test");
        activateOnce = false;
    }

	// Update is called once per frame
	void Update () {
		if (!activateOnce && Time.time - SecondsToWait >= 0)
        {
            activateOnce = true;
            StartCoroutine(LoadEnemiesKilled());
        }
	}

    public IEnumerator LoadEnemiesKilled()
    {
        SaveInfoObject temp = SaveInfo.saveInfo.SaveObject;
        if (temp.EnemiesKilledPrevious != null)
        {
            int count = 0, heightDelta = 70;
            Dictionary<string, int> lastlevel = temp.EnemiesKilledPrevious;
            foreach (KeyValuePair<string, int> prefab in lastlevel)
            {
                GameObject prefabObject = (GameObject)Resources.Load("Level Scene\\Prefab\\Enemy\\" + prefab.Key);
                Vector2 position1 = new Vector2(SpawnPrefabHereX, SpawnPrefabHereY);
                GameObject created = Instantiate(enemyIconPrefab, position1, Quaternion.identity);
                created.transform.SetParent(contentBox.transform);
                created.GetComponent<RectTransform>().anchorMax = new Vector2(0, 1);
                created.GetComponent<RectTransform>().anchorMin = new Vector2(0, 1);
                created.transform.localPosition = position1;

                //Temporarily removes children then resizes based on sprite dimensions. Reattaches children at the end
                Sprite prefabSprite = prefabObject.GetComponent<SpriteRenderer>().sprite;
                GameObject[] Children = new GameObject[created.transform.childCount];
                for (int i = 0; i < Children.Length; i++)
                {
                    Children[i] = created.transform.GetChild(i).gameObject;
                }
                created.transform.DetachChildren();

                float width = prefabSprite.rect.width, height = prefabSprite.rect.height;
                created.GetComponent<Image>().sprite = prefabSprite;
                Rect createdRect = created.GetComponent<RectTransform>().rect;

                if (width < height)
                {
                    created.GetComponent<RectTransform>().sizeDelta = new Vector2(createdRect.width * (width / height) * 1.1f, createdRect.height * 1.1f);
                }
                else if (width > height)
                {
                    created.GetComponent<RectTransform>().sizeDelta = new Vector2(createdRect.width * 1.1f, createdRect.height * (height / width) * 1.1f);
                }


                for (int i = 0; i < Children.Length; i++)
                {
                    Children[i].transform.SetParent(created.transform);
                }

                created.GetComponentInChildren<Text>().text = "Number Eliminated: " + prefab.Value;
                SpawnPrefabHereY -= 70;
                count++;

                if (count >= 5)
                {
                    //contentBox.GetComponent<RectTransform>().sizeDelta += new Vector2(0, 0);
                    contentBox.GetComponent<RectTransform>().sizeDelta += new Vector2(0, heightDelta);
                    //Debug.Log(heightDelta);
                    //heightDelta += 70;
                    
                }

                yield return new WaitForSeconds(0.3f);
            }
        }
    }
}
