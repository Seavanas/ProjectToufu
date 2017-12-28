using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HUDTextFacade : MonoBehaviour {
    public GameObject HUD;
	// Use this for initialization
	void Start () {
		if (HUD == null)
        {
            HUD = this.gameObject;
        }
	}
	
    public void SetText(string text)
    {
        HUD.GetComponent<Text>().text = text;
        HUD.GetComponent<TypingScript>().Reset();
        HUD.GetComponent<TypingScript>().StartTyping = true;
    }
}
