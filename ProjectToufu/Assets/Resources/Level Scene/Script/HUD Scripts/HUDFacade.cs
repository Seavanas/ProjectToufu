using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HUDFacade : MonoBehaviour {

    public GameObject ScoreText, LivesText, HealthBar;
    public static HUDFacade HUDfacade;
    public BossInfoScript BossInfoScript;


    void Awake()
    {
        if (HUDfacade == null)
        {
            HUDfacade = this;
        }
        else Destroy(this.gameObject);
    }

    public void SetBossName(string name)
    {
        BossInfoScript.BossTitle.text = name;
    }

    public void SetBossHealth(float health)
    {
        BossInfoScript.BossHealthbar.fillAmount = health;
    }

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
