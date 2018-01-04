using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasePlayerScript : MonoBehaviour {


    public float health;
    private float maxHealth;
    private HUDFacade HUD;

    void Start()
    {
        HUD = GameObject.FindGameObjectWithTag("Hud").GetComponent<HUDFacade>();
        maxHealth = health;
        UpdateHealth();
    }
    
    public void TakeDamage(float damage)
    {
        health -= damage;
        UpdateHealth();
    }

    public void AnimationEvent_Destroy()
    {
        Destroy(gameObject);
    }
    
    public void UpdateHealth()
    {
        HUD.SetHealth((health) / maxHealth);
    }
}
