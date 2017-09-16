using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseBossScript : MonoBehaviour {

    private HUDFacade HUD;

    public string title;
	// Use this for initialization
	void Start () {
        HUD = HUDFacade.HUDfacade;
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void InitiateHUD()
    {
        HUD.BossInfoScript.enabled = true;
        SetHealth(1);
        SetTitle(title);
    }

    public void SetHealth(float health)
    {
        HUD.SetBossHealth(health);
    }

    public void SetTitle(string title)
    {
        HUD.SetBossName(title);
    }
}
