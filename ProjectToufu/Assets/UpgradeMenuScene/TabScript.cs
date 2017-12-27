using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TabScript : MonoBehaviour {
    public GameObject othertab1, othertab2;
    public Color setOtherTabColor;
	// Use this for initialization
	void Start () {
	    
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void ButtonPressed()
    {
        GetComponent<Image>().color = new Color(255,255,255,255);
        othertab1.GetComponent<Image>().color = setOtherTabColor;
        othertab2.GetComponent<Image>().color = setOtherTabColor;

    }
}
