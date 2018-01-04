using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreScript : MonoBehaviour {

    private float score = 0;

    public void SetScore(float newScore)
    {
        score = newScore;
        this.GetComponent<Text>().text = "Score: " + newScore;
    }

    public void AddScore(float scoreGain)
    {
        score += scoreGain;
        this.GetComponent<Text>().text = "Score: " + score;
    }
}
