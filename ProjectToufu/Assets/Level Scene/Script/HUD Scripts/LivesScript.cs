using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LivesScript : MonoBehaviour {
    public float lives = 0;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {

	}

    public void SetLives(float newLives)
    {
        lives = newLives;
        GetComponent<Text>().text = "Lives: " + newLives;
    }

    public void AddLive(float liveGain)
    {
        lives += liveGain;
        GetComponent<Text>().text = "Lives: " + lives;
    }
}
