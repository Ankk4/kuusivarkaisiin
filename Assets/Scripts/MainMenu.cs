using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour {

    public Button newGameButton;
    public Button creditsButton;
    public Button BackToMenuButton;
    public Text creditsText;

	// Use this for initialization
	void Start () 
    {
		
	}
	
	// Update is called once per frame
	void Update () 
    {
		
	}

    public void NewGame()
    {
        SceneManager.LoadScene("game", LoadSceneMode.Single);
    }

    public void Credits()
    {
        newGameButton.gameObject.SetActive(false);
        creditsButton.gameObject.SetActive(false);
        BackToMenuButton.gameObject.SetActive(true);
        creditsText.gameObject.SetActive(true);
    }

    public void BackToMenu()
    {
        newGameButton.gameObject.SetActive(true);
        creditsButton.gameObject.SetActive(true);
        BackToMenuButton.gameObject.SetActive(false);
        creditsText.gameObject.SetActive(false);    
    }

    public void OpenMainMenu()
    {
        SceneManager.LoadScene("MainMenu", LoadSceneMode.Single);
    }
}
