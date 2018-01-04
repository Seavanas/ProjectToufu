using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TypingScript : MonoBehaviour {

    public float TypingSpeed; 
    public bool StartTyping = false;
    public GameObject[] NextText; //If there's another line of text somewhere else, then initiate it to start typing after this one ends

    private Text Text;
    private string TheString;
    private float TimeBeforeNextLetter = 0;
    private int CurrentLetter = 0;


	// Use this for initialization

    //Basically resets the texts
	void Start () {
        Text = GetComponent<Text>();
        TheString = Text.text;
        Text.text = "";
        CurrentLetter = 0;
	}

    public void Reset()
    {
        Start();
        StartTyping = false;
        foreach (GameObject text in NextText)
        {
            text.GetComponent<TypingScript>().Reset();
        }
    }
	
	// Update is called once per frame
	void Update () {
        if (StartTyping)
        {
            if (CurrentLetter < TheString.Length)
            {
                if (TimeBeforeNextLetter <= Time.time)
                {
                    Text.text += TheString[CurrentLetter];
                    TimeBeforeNextLetter = Time.time + TypingSpeed;
                    CurrentLetter++;
                }
            }
            else
            {
                StartTyping = false;
                foreach (GameObject text in NextText)
                {
                    text.GetComponent<TypingScript>().StartTyping = true;
                }
            }
        }
		
	}
}
