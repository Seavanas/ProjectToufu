using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HUDFacade : MonoBehaviour {

    public GameObject ScoreText, LivesText, HealthBar;

    public void SetScore(float newscore)
    {
        ScoreText.GetComponent<ScoreScript>().SetScore(newscore);
    }

    public void SetLives(float lives)
    {
        LivesText.GetComponent<LivesScript>().SetLives(lives);
    }

    public void AddScore(float scoreGain)
    {
        ScoreText.GetComponent<ScoreScript>().AddScore(scoreGain);
    }

    public void AddLive(float liveGain)
    {
        LivesText.GetComponent<LivesScript>().AddLive(liveGain);
    }

    public void SetHealth(float health)
    {
        HealthBar.GetComponent<Health>().SetHealth(health);
    }
}
