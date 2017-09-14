using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LevelControllerMap : MonoBehaviour {

    //public GameObject PauseMenuPrefab;
    //public Canvas Canvas;
    public GameObject PauseMenu;
    
    private bool CurrentlyInMenu = false;

    public Button Quit, Continue, Settings, Save;
    public GameObject HeadText;//head of the text of game menu

    public Button PlayTEMPORARY;//temporary

	// Use this for initialization
	void Start () {
        Quit.onClick.AddListener(QuitButton);
        Continue.onClick.AddListener(ContinueButton);
        Settings.onClick.AddListener(SettingsButton);
        Save.onClick.AddListener(SaveButton);
        PauseMenu.SetActive(false);

        PlayTEMPORARY.onClick.AddListener(PlayLevel);
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetButtonDown("Cancel"))
        {
            if (!CurrentlyInMenu)
            {
                
                CurrentlyInMenu = true;
                PauseMenu.SetActive(true);
                HeadText.GetComponent<TypingScript>().StartTyping = true;
                //PauseMenu = Instantiate(PauseMenuPrefab);
                //PauseMenu.transform.SetParent(Canvas.transform, false);
                

            }
            else
            {
                ContinueButton();
            }
        }
	}


    //Button Functions

    private void QuitButton()
    {
        Debug.Log("Doop");
    }
    private void ContinueButton()
    {
        HeadText.GetComponent<TypingScript>().Reset();
        CurrentlyInMenu = false;
        PauseMenu.SetActive(false);
    }
    private void SettingsButton()
    {
        SaveInfo.saveInfo.Load();
    }
    private void SaveButton()
    {
        SaveInfo.saveInfo.Save();
    }
    private void PlayLevel()
    {
        SceneManager.LoadScene("Main");
    }

}
