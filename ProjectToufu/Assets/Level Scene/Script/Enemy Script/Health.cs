using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Health : MonoBehaviour {
    public float health = 1;
    public Image Bar;

	// Use this for initialization
	void Start () {
        SetHealth(health);
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void SetHealth(float health)
    {
        this.health = health;
        Bar.fillAmount = health;
    }
}
