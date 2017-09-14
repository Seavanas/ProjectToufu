using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundScript : MonoBehaviour {

    public float speed;
    private float lengthDividedBy2 = 34;
    private float startingYPosition;
    
	// Use this for initialization
	void Start () {
        startingYPosition = gameObject.transform.position.y;
    }
	
	// Update is called once per frame
	void Update () {
        gameObject.transform.position = new Vector2(gameObject.transform.position.x, (gameObject.transform.position.y - speed));
        if (gameObject.transform.position.y + lengthDividedBy2 <= startingYPosition)
        {
            gameObject.transform.position = new Vector2(0, startingYPosition);
        }
	}
}
