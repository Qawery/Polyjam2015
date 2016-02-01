using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class SubmenuBehaviour : MonoBehaviour {
    public GameObject SettingBtns;

    public enum Types
    {
        Settings,
        Cheats
    }

    public Types submenuType;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        // update visibility according to submenu type
        switch (submenuType)
        {
            case Types.Settings: SettingBtns.SetActive(GameVariables.isSettingsOpen); break;
            case Types.Cheats: SettingBtns.SetActive(GameVariables.isCheatsOpen && GameVariables.isSettingsOpen); GameVariables.isCheatsOpen = SettingBtns.activeSelf; break;
        }
	}
}
