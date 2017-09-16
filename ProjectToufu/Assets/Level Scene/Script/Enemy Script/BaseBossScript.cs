using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseBossScript : MonoBehaviour {

    private HUDFacade HUD;

    public string title;
	
	// Update is called once per frame
	void Update () {
		
	}

    //Starts coroutine that waits until singleton HUDFacade is done 
    public void InitiateHUD()
    {
        HUD = HUDFacade.HUDfacade;
        HUD.BossInfoScript.gameObject.SetActive(true);
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
