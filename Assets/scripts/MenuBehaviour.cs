using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class MenuBehaviour : MonoBehaviour {
    public Slider Music;
    public Slider Sound;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void OnGameBtn()
    {
        AutoFade.LoadLevel("GameScene", 1, (float)0.2, Color.black);

        // restore default values
        GameVariables.score = 0;
        GameVariables.life = GameVariables.defaultLife;
        GameVariables.flares = GameVariables.defaultFlares;
        GameVariables.cooldownProgress = 0;
        GameVariables.reloadProgress = GameVariables.reloadTime;
        GameVariables.oxygenState = GameVariables.oxygenMax / 2;
        GameVariables.isAlive = true;
        GameVariables.gameFinished = false;

        // kill submenus
        GameVariables.isCheatsOpen = false;
        GameVariables.isSettingsOpen = false;
    }

    public void OnTest()
    {
        GameVariables.score += 5;
    }

    public void OnReturnBtn()
    {
        HighScoreManager._instance.SaveHighScore(System.Environment.UserName,GameVariables.score);
        AutoFade.LoadLevel("MenuScene", 1, (float)0.2, Color.black);
    }

    public void OnJumpToPage(string SceneName)
    {
        AutoFade.LoadLevel(SceneName, 1, (float)0.2, Color.black);
    }

    public void OnExitBtn()
    {
        Application.Quit();
    }

    public void OnSettingsBtn()
    {
        GameVariables.isSettingsOpen = !GameVariables.isSettingsOpen;
    }

    public void OnHideSettings()
    {
        GameVariables.isSettingsOpen = false;
    }

    public void UnpauseGame()
    {
        Time.timeScale = 1;
    }

    public void setMusicVolume()
    {
        GameVariables.musicVolume = Music.value;
        AudioListener.volume = Music.value;
    }

    public void setSndVolume()
    {
        GameVariables.soundVolume = Sound.value;
    }
}
