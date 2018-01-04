using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class GameOverScript : MonoBehaviour {

    private Image image;
    private Text gameOverText;
    public GameObject GameOverText;
	// Use this for initialization
	void Start () {
        image = GetComponent<Image>();
        gameOverText = GameOverText.GetComponent<Text>();

	}
	
	// Update is called once per frame
    // Increases opacity until opaque
	void Update () {
        float alpha = image.color.a + 0.01f;
        if (alpha == 1)
            return;
        image.color = new Color(255f, 255f, 255f, alpha);
        gameOverText.color = new Color(0, 0, 0, alpha);

    }
}
