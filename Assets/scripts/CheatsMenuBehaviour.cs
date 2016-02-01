using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class CheatsMenuBehaviour : MonoBehaviour {
    public GameObject CheatsButton;
    public InputField LivesInput;

	// Use this for initialization
	void Start () {
        // set game variables cause it's fun
        LivesInput.text    = GameVariables.defaultLife.ToString();
	}
	
	// Update is called once per frame
	void Update () {
        // disable cheats if player iz a n00bz0r
        CheatsButton.SetActive(GameVariables.areCheatsEnabled && GameVariables.isSettingsOpen);
	}
        
    void EnableCheats()
    {
        GameVariables.areCheatsEnabled = true;
    }

    public void SetLives()
    {
        GameVariables.defaultLife = int.Parse(LivesInput.text);
    }

    public void SetCheatsState(bool state)
    {
        GameVariables.isCheatsOpen = state;
    }

    public void DisableDying()
    {
        GameVariables.disableDying = true;
    }
}
